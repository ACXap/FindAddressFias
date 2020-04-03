using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FindAddressFias.Data;

namespace FindAddressFias
{
    public class MainWindowModel
    {
        #region PrivateField

        private const string _fileOktmo = "dataO.csv";
        private const string _fileAddress = "dataA.csv";
        private const string _fileFias = "dataF.csv";
        private const int _countAttempt = 5;

        private readonly IRepositoryFias _repository = new RepositorySiteFias();
        private readonly object _lockCountReady = new object();

        private int _countReady = 0;

        #endregion PrivateField

        #region PublicProperties
        public ObservableCollection<EntityAddress> CollectionAddress { get; private set; }
        #endregion PublicProperties

        #region PrivateMethod

        private void GetData(string fileName)
        {
            if (File.Exists(fileName))
            {
                var allString = File.ReadAllLines(fileName, Encoding.Default);
                CollectionAddress = new ObservableCollection<EntityAddress>(allString.Select(x =>
                {
                    var a = x.Split(';');
                    return new EntityAddress()
                    {
                        Oktmo = a[0],
                        Address = a[1],
                        Fias = a[2]
                    };
                }).ToList());
            }
        }

        private void FindDuplicate()
        {
            Parallel.ForEach(CollectionAddress, item =>
            {
                if (CollectionAddress.Count(x => x.Fias == item.Fias && !string.IsNullOrEmpty(item.Fias)) > 1)
                {
                    item.ErrorLog += " Дубль фиас";
                }
            });
        }

        private void SaveFile(string file)
        {
            try
            {
                File.WriteAllLines(file, CollectionAddress.Select(x =>
                {
                    return $"{x.Oktmo};{x.Address};{x.OktmoWeb};{x.AddressMun};{x.Fias};{x.Error};{x.ErrorLog};{x.Status}";
                }));
            }
            catch (Exception ex)
            {

            }
        }

        #endregion PrivateMethod

        #region PublicMethod

        public ObservableCollection<EntityAddress> SelectFile()
        {
            OpenFileDialog fd = new OpenFileDialog
            {
                Multiselect = false
            };

            if (fd.ShowDialog() == true)
            {
                GetData(fd.FileName);

                return CollectionAddress;
            }

            return new ObservableCollection<EntityAddress>();
        }

        private async Task GetData(Action<int> callbackCount, string whatDo)
        {
            await Task.Factory.StartNew(() =>
             {
                 ParallelOptions po = new ParallelOptions()
                 {
                     MaxDegreeOfParallelism = 10
                 };

                 Parallel.ForEach(CollectionAddress, po, (item) =>
                 {
                     lock (_lockCountReady)
                     {
                         _countReady++;
                     }

                     callbackCount(_countReady);

                     if (item == null) return;

                     var count = _countAttempt;
                     while (count-- > 0)
                     {
                         try
                         {
                             EntityAddress address = null;

                             if (whatDo == "oktmo")
                             {
                                 address = _repository.GetAddressByOktmo(item.Oktmo);
                             }
                             else if (whatDo == "address")
                             {
                                 address = _repository.GetAddressByAddress(item.Address);
                             }
                             else if(whatDo == "fias")
                             {
                                 address = _repository.GetAddressByFias(item.Fias);
                             }

                             item.AddressMun = address.AddressMun;
                             item.Fias = address.Fias;
                             item.Status = address.Status;
                             item.OktmoWeb = address.OktmoWeb;
                             item.Error = address.Error;
                             item.ErrorLog = address.ErrorLog;

                             if (item.Oktmo != item.OktmoWeb && !string.IsNullOrEmpty(item.OktmoWeb))
                             {
                                 item.ErrorLog += " Не совпадает октмо";
                             }

                             break;
                         }
                         catch (WebException exWeb)
                         {
                             if (count <= 0)
                             {
                                 item.Error = exWeb.Message;
                                 break;
                             }
                             Thread.Sleep(5000);
                         }
                         catch (Exception ex)
                         {
                             item.Error = ex.Message;
                             break;
                         }
                     }
                 });

                 FindDuplicate();
             });
        }

        public async Task GetDataByFias(Action<int> callbackCount)
        {
            await GetData(callbackCount, "fias");

            SaveFile(_fileFias);
        }

        public async Task GetDataByOktmo(Action<int> callbackCount)
        {
            await GetData(callbackCount, "oktmo");

            SaveFile(_fileOktmo);
        }

        public async Task GetDataByAddress(Action<int> callbackCount)
        {
            await GetData(callbackCount, "address");

            SaveFile(_fileAddress);
        }

        #endregion PublicMethod

        public void AddTestAddress(EntityAddress address)
        {
            CollectionAddress = new ObservableCollection<EntityAddress>()
            {
                address
            };
        }
    }
}
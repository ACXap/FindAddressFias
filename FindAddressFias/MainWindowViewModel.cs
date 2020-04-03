using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using HtmlAgilityPack;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ServicePointManager.DefaultConnectionLimit = 100;
        }

        #region PrivateField
        private readonly MainWindowModel _model = new MainWindowModel();

        private ReadOnlyObservableCollection<EntityAddress> _collectionAddress;

        private bool _isStart = false;
        private int _countReady = 0;

        private RelayCommand _commandSelectFile;
        private RelayCommand _commandStartByOktmo;
        private RelayCommand _commandStartByAddress;
        #endregion PrivateField

        #region PublicProperties
        public ReadOnlyObservableCollection<EntityAddress> CollectionAddress
        {
            get => _collectionAddress;
            set => Set(ref _collectionAddress, value);
        }

        public bool IsStart
        {
            get => _isStart;
            set => Set(ref _isStart, value);
        }

        public int CountReady
        {
            get => _countReady;
            set => Set(ref _countReady, value);
        }

        #endregion PublicProperties

        #region Command
        public RelayCommand CommandSelectFile =>
        _commandSelectFile ?? (_commandSelectFile = new RelayCommand(
                    () =>
                    {
                        CollectionAddress = new ReadOnlyObservableCollection<EntityAddress>(_model.SelectFile());
                        CountReady = 0;
                    }));

        public RelayCommand CommandStartByOktmo =>
        _commandStartByOktmo ?? (_commandStartByOktmo = new RelayCommand(
                    async () =>
                    {
                        IsStart = true;
                        await _model.GetDataByOktmo((count)=>
                        {
                            CountReady = count;
                        });

                        IsStart = false;
                    }, ()=> _collectionAddress!=null && _collectionAddress.Any()));

        public RelayCommand CommandStartByAddress =>
       _commandStartByAddress ?? (_commandStartByAddress = new RelayCommand(
                   async () =>
                   {
                       IsStart = true;

                       await _model.GetDataByAddress((count) =>
                       {
                           CountReady = count;
                       });

                       IsStart = false;
                   }, () => _collectionAddress != null && _collectionAddress.Any()));

        private RelayCommand _commandStartByFias;
        public RelayCommand CommandStartByFias =>
        _commandStartByFias ?? (_commandStartByFias = new RelayCommand(
                    async () =>
                    {
                        IsStart = true;

                        await _model.GetDataByFias((count) =>
                        {
                            CountReady = count;
                        });

                        IsStart = false;
                    }, () => _collectionAddress != null && _collectionAddress.Any()));

        #endregion Command
    }
}
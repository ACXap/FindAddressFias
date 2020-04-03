using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FindAddressFias.Data
{
    public class RepositorySiteFias : IRepositoryFias
    {
        #region PrivateField
        private string _urlExtendedSearch = "https://fias.nalog.ru/ExtendedSearch/PubExtSearch";
        #endregion PrivateField

        #region PrivateMethod

        private string RequestPost(string url, string requestBody)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.Method = "POST";
            request.Timeout = 5000;

            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(requestBody);
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
        }

        private string RequestGet(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = "application/json";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            request.Method = "GET";
            request.Timeout = 5000;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
        }

        private string GetJsonStringFindOktmo(string oktmo)
        {
            var a = new JsonRequest()
            {
                Division = "1",
                MunHierarchyRegion = "",
                MunHierarchySettlement = "",
                MunHierarchyIntownTerritory = "",
                MunHierarchyPlanningStructure = "",
                MunHierarchyStreet = "",
                OktmoCode = oktmo,
                OktmoComboBox = "",
                Skip = 0,
                Take = 5,
                AdmHierarchy = "{}",
                MunHierarchy = "{}",
                ObjectLevelFias = "6",
                OnlyActualAddressObjects = "true",
                CadastrNumber = "",
                Guid = "",
                Section = "",
                OkatoComboBox = "",
                OkatoCode = "",
                PostalIndex = "",
                MunHierarchyRoom = "",
                MunHierarchyApartment = "",
                MunHierarchyCarPlace = "",
                MunHierarchyHouse = "",
                MunHierarchyStead = ""
            };

            string json = JsonConvert.SerializeObject(a).Replace("\"{}\"", "{}");

            return json;
        }

        private string GetJsonStringFindFias(string fias)
        {
            var a = new JsonRequest()
            {
                Division = "1",
                MunHierarchyRegion = "",
                MunHierarchySettlement = "",
                MunHierarchyIntownTerritory = "",
                MunHierarchyPlanningStructure = "",
                MunHierarchyStreet = "",
                OktmoCode = "",
                OktmoComboBox = "",
                Skip = 0,
                Take = 5,
                AdmHierarchy = "{}",
                MunHierarchy = "{}",
                ObjectLevelFias = "6",
                OnlyActualAddressObjects = "true",
                CadastrNumber = "",
                Guid = fias,
                Section = "",
                OkatoComboBox = "",
                OkatoCode = "",
                PostalIndex = "",
                MunHierarchyRoom = "",
                MunHierarchyApartment = "",
                MunHierarchyCarPlace = "",
                MunHierarchyHouse = "",
                MunHierarchyStead = ""
            };

            string json = JsonConvert.SerializeObject(a).Replace("\"{}\"", "{}");

            return json;
        }

        private string GetUrlSearching(string address)
        {
            return $"https://fias.nalog.ru/Search/Searching?text={address}&division=1&filter[filters][0][value]={address}&filter[filters][0][field]=PresentRow&filter[filters][0][operator]=contains&filter[filters][0][ignoreCase]=true&filter[logic]=and";
        }

        private EntityAddress GetAddressForStringJson(string json)
        {
            var address = new EntityAddress();

            var listObj = JsonConvert.DeserializeObject<RootObject>(json);

            if (listObj.Data.Any())
            {
                var ad = listObj.Data.SingleOrDefault(x => x.LevelId == 6);

                if (ad != null)
                {
                    address.Status = ad.IsActive.ToString();
                    address.OktmoWeb = ad.Params?.OKTMO;
                    address.Fias = ad.GUID;
                    address.AddressMun = ad.Name;
                }
                else
                {
                    address.ErrorLog = "Не найден НП с данным ОКТМО";
                }
            }
            else
            {
                address.ErrorLog = "Данный ОКТМО не найден";
            }

            return address;
        }
        
        #endregion PrivateMethod

        #region PublicMethod
        public EntityAddress GetAddressByAddress(string address)
        {
            var addr = new EntityAddress();

            var url = GetUrlSearching(address);
            var str = RequestGet(url);

            var listObj = JsonConvert.DeserializeObject<List<JsonResponsByAddress>>(str);

            if (!listObj.Any())
            {
                char[] chars = address.ToCharArray();
                chars[address.Length - 1] = 'О';
                address = new string(chars);
                url = GetUrlSearching(address);
                str = RequestGet(url);
                listObj = JsonConvert.DeserializeObject<List<JsonResponsByAddress>>(str);
            }

            if (listObj.Any())
            {
                var ad = listObj[0];

                addr.Status = ad.IsActive.ToString();
                addr.Id = ad.ObjectId.ToString();
                addr.AddressMun = ad.PresentRow;
            }
            else
            {
                addr.ErrorLog = "Данный адрес не найден";
            }

            if (string.IsNullOrEmpty(addr.Id) || !string.IsNullOrEmpty(addr.ErrorLog)) return addr;

            url = $"https://fias.nalog.ru/Search/SearchObjInfo?objId={addr.Id}";

            str = RequestGet(url);

            var document = new HtmlDocument();
            document.LoadHtml(str);
            addr.AddressMun = HtmlEntity.DeEntitize(document.GetElementbyId("moDivisionLabel").InnerText);

            addr.Status = HtmlEntity.DeEntitize(document.GetElementbyId("statusLabel").InnerText);
            addr.Fias = document.GetElementbyId("guidLabel").InnerText;
            addr.OktmoWeb = HtmlEntity.DeEntitize(document.GetElementbyId("oktmoLabel").InnerText);

            return addr;
        }

        public EntityAddress GetAddressByFias(string fias)
        {
            var obj = RequestPost(_urlExtendedSearch, GetJsonStringFindFias(fias));

            return GetAddressForStringJson(obj);
        }

        public EntityAddress GetAddressByOktmo(string oktmo)
        {
            var obj = RequestPost(_urlExtendedSearch, GetJsonStringFindOktmo(oktmo));

            return GetAddressForStringJson(obj);
        }
        #endregion PublicMethod
    }
}
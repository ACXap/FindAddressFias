using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAddressFias.Data
{
    public class Params
    {
        public string TerritorialIfnsFlCode { get; set; }
        public string TerritorialIfnsUlCode { get; set; }
        public string IFNSUL { get; set; }
        public string IFNSFL { get; set; }
        public string PostIndex { get; set; }
        public string OKATO { get; set; }
        public string OKTMO { get; set; }
        public object CadastrNum { get; set; }
        public object RegionCode { get; set; }
        public string Code { get; set; }
        public string PlainCode { get; set; }
        public string ReesterNum { get; set; }
        public object OfficialName { get; set; }
    }

    public class Datum
    {
        public string Name { get; set; }
        //public int Id { get; set; }
        //public int ObjectId { get; set; }
        //public int OperationTypeId { get; set; }
        public bool IsActual { get; set; }
        public bool IsActive { get; set; }
        //public int ChangeId { get; set; }
        //public int PrevId { get; set; }
        //public int NextId { get; set; }
        //public DateTime CreateDate { get; set; }
        //public DateTime StartDatetime { get; set; }
        //public DateTime EndDatetime { get; set; }
        public Params Params { get; set; }
        public string GUID { get; set; }
        public int LevelId { get; set; }
        public string RegionCode { get; set; }
        public string ObjType { get; set; }
    }

    public class RootObject
    {
        public List<Datum> Data { get; set; }
        public int Total { get; set; }
        public object AggregateResults { get; set; }
        public object Errors { get; set; }
    }
}
using GalaSoft.MvvmLight;

namespace FindAddressFias.Data
{
    public class EntityAddress:ViewModelBase
    {

        private string _address = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Address
        {
            get => _address;
            set => Set(ref _address, value);
        }

        private string _oktmo = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Oktmo
        {
            get => _oktmo;
            set => Set(ref _oktmo, value);
        }

        private string _fias = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Fias
        {
            get => _fias;
            set => Set(ref _fias, value);
        }

        private string _addressMun = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string AddressMun
        {
            get => _addressMun;
            set => Set(ref _addressMun, value);
        }

        private string _id = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        private string _status = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Status
        {
            get => _status;
            set => Set(ref _status, value);
        }

        private string _error = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Error
        {
            get => _error;
            set => Set(ref _error, value);
        }

        private string _errorLog = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string ErrorLog
        {
            get => _errorLog;
            set => Set(ref _errorLog, value);
        }

        private string _oktmoWeb = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string OktmoWeb
        {
            get => _oktmoWeb;
            set => Set(ref _oktmoWeb, value);
        }


    }
}
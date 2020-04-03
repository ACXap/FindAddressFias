using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAddressFias.Test
{
    [TestFixture]
    public class TestMainWindowModel
    {
        private MainWindowModel _model;

        private string _goodOktmo = "38602404101";
        private string _goodOktmoAddress = "Курская область, Беловский  м.р-н, с.п. Беличанский сельсовет, с Белица";
        private string _goodOktmoFias = "91162323-e593-47cf-980a-215e02f02f60";

        private string _goodAddress = "КУРСКАЯ ОБЛ, БЕЛОВСКИЙ, БЕЛИЧАНСКИЙ СЕЛЬСОВЕТ, С БЕЛИЦА";
        private string _goodAddressAddress = "Курская область, Беловский  муниципальный район, сельское поселение Беличанский сельсовет, село Белица";

        private string _notFoundOktmo = "29615430123";
        private string _notFountAddress = "КАЛУЖСКАЯ ОБЛ, ИЗНОСКОВСКИЙ, ДЕРЕВНЯ ОРЕХОВНЯ, Д НОВЫЕ КЛИНЫ";

        [SetUp]
        public void Init()
        {
            _model = new MainWindowModel();
        }

        [Test]
        public async Task GetDataGoodOktmoAsync()
        {
            _model.AddTestAddress(new Data.EntityAddress()
            {
                Oktmo = _goodOktmo
            });

            await _model.GetDataByOktmo((c)=>
            {

            });

            var address = _model.CollectionAddress[0];

            Assert.IsEmpty(address.Error);
            Assert.IsEmpty(address.ErrorLog);
            Assert.AreEqual(address.AddressMun, _goodOktmoAddress);
            Assert.AreEqual(address.Fias, _goodOktmoFias);
            Assert.AreEqual(address.Status, "True");
            Assert.AreEqual(address.OktmoWeb, _goodOktmo);
        }

        [Test]
        public async Task GetAddressByOktmo_GootAddress()
        {
            _model.AddTestAddress(new Data.EntityAddress()
            {
                Oktmo = _goodOktmo,
                Address = _goodAddress
            });

            await _model.GetDataByAddress((c) =>
            {

            });

            var address = _model.CollectionAddress[0];

            Assert.IsEmpty(address.Error);
            Assert.IsEmpty(address.ErrorLog);
            Assert.AreEqual(address.AddressMun, _goodAddressAddress);
            Assert.AreEqual(address.Fias, _goodOktmoFias);
            Assert.AreEqual(address.Status, "Актуальна");
            Assert.AreEqual(address.OktmoWeb, _goodOktmo);
        }

        [Test]
        public async Task GetAddressByOktmo_NotFoundOktmo()
        {
            _model.AddTestAddress(new Data.EntityAddress()
            {
                Oktmo = _notFoundOktmo
            });

            await _model.GetDataByOktmo((c) =>
            {

            });

            var address = _model.CollectionAddress[0];

            Assert.AreEqual(address.ErrorLog, "Данный ОКТМО не найден");
            Assert.IsEmpty(address.AddressMun);
            Assert.IsEmpty(address.Fias, _goodOktmoFias);
            Assert.IsEmpty(address.Status);
            Assert.IsEmpty(address.OktmoWeb);
            Assert.IsEmpty(address.Error);
        }

        [Test]
        public async Task GetAddressByOktmo_NotFoundAddress()
        {
            _model.AddTestAddress(new Data.EntityAddress()
            {
                Oktmo = _notFoundOktmo,
                Address = _notFountAddress
            });

            await _model.GetDataByAddress((c) =>
            {

            });

            var address = _model.CollectionAddress[0];

            Assert.AreEqual(address.ErrorLog, "Данный адрес не найден");
            Assert.IsEmpty(address.AddressMun);
            Assert.IsEmpty(address.Fias, _goodOktmoFias);
            Assert.IsEmpty(address.Status);
            Assert.IsEmpty(address.OktmoWeb);
            Assert.IsEmpty(address.Error);
        }
    }
}
using NUnit.Framework;
using FindAddressFias.Data;

namespace FindAddressFias.Test
{
    [TestFixture]
    public class TestRepositorySiteFias
    {
        private IRepositoryFias _repository;

        private string _goodOktmo = "38602404101";
        private string _goodOktmoAddress = "Курская область, Беловский  м.р-н, с.п. Беличанский сельсовет, с Белица";
        private string _goodOktmoFias = "91162323-e593-47cf-980a-215e02f02f60";

        private string _goodAddress = "КУРСКАЯ ОБЛ, БЕЛОВСКИЙ, БЕЛИЧАНСКИЙ СЕЛЬСОВЕТ, С БЕЛИЦА";
        private string _goodAddressAddress = "Курская область, Беловский  муниципальный район, сельское поселение Беличанский сельсовет, село Белица";

        private string _notFoundOktmo = "29615430123";
        private string _notFountAddress = "КАЛУЖСКАЯ ОБЛ, ИЗНОСКОВСКИЙ, ДЕРЕВНЯ ОРЕХОВНЯ, Д НОВЫЕ КЛИНЫ";

        private string _goodFias = "a00dbf21-1bc1-4ca8-aa75-825a6dc06826";
        private string _goodFiasAddress = "Курская область, Горшеченский  м.р-н, с.п. Богатыревский сельсовет, х Частая Дубрава";

        [SetUp]
        public void Init()
        {
            _repository = new RepositorySiteFias();
        }
        
        [Test]
        public void GetAddressByOktmo_GootOktmo()
        {
            var address = _repository.GetAddressByOktmo(_goodOktmo);

            Assert.IsEmpty(address.Error);
            Assert.IsEmpty(address.ErrorLog);
            Assert.AreEqual(address.AddressMun, _goodOktmoAddress);
            Assert.AreEqual(address.Fias, _goodOktmoFias);
            Assert.AreEqual(address.Status, "True");
            Assert.AreEqual(address.OktmoWeb, _goodOktmo);
        }

        [Test]
        public void GetAddressByOktmo_GootAddress()
        {
            var address = _repository.GetAddressByAddress(_goodAddress);
            address.Oktmo = _goodOktmo;

            Assert.IsEmpty(address.Error);
            Assert.IsEmpty(address.ErrorLog);
            Assert.AreEqual(address.AddressMun, _goodAddressAddress);
            Assert.AreEqual(address.Fias, _goodOktmoFias);
            Assert.AreEqual(address.Status, "Актуальна");
            Assert.AreEqual(address.OktmoWeb, _goodOktmo);
        }

        [Test]
        public void GetAddressByOktmo_NotFoundOktmo()
        {
            var address = _repository.GetAddressByOktmo(_notFoundOktmo);

            Assert.AreEqual(address.ErrorLog, "Данный ОКТМО не найден");
            Assert.IsEmpty(address.AddressMun);
            Assert.IsEmpty(address.Fias, _goodOktmoFias);
            Assert.IsEmpty(address.Status);
            Assert.IsEmpty(address.OktmoWeb);
            Assert.IsEmpty(address.Error);
        }

        [Test]
        public void GetAddressByOktmo_NotFoundAddress()
        {
            var address = _repository.GetAddressByAddress(_notFountAddress);
            address.Oktmo = _goodOktmo;

            Assert.AreEqual(address.ErrorLog, "Данный адрес не найден");
            Assert.IsEmpty(address.AddressMun);
            Assert.IsEmpty(address.Fias, _goodOktmoFias);
            Assert.IsEmpty(address.Status);
            Assert.IsEmpty(address.OktmoWeb);
            Assert.IsEmpty(address.Error);
        }

        [Test]
        public void GetAddressByOktmo_GootFias()
        {
            var address = _repository.GetAddressByFias(_goodFias);

            Assert.IsEmpty(address.Error);
            Assert.IsEmpty(address.ErrorLog);
            Assert.AreEqual(address.AddressMun, _goodFiasAddress);
            //Assert.AreEqual(address.Fias, _goodOktmoFias);
            Assert.AreEqual(address.Status, "True");
            //Assert.AreEqual(address.OktmoWeb, _goodOktmo);
        }
    }
}
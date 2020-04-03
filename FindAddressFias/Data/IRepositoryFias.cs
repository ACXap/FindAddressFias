using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindAddressFias.Data
{
    public interface IRepositoryFias
    {
        EntityAddress GetAddressByOktmo(string oktmo);
        EntityAddress GetAddressByAddress(string address);
        EntityAddress GetAddressByFias(string fias);
    }
}
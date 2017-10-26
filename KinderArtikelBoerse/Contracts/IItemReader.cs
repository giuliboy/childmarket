using KinderArtikelBoerse.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KinderArtikelBoerse.Contracts
{

    public interface IItemReader
    {
        Seller ReadItems( string filePath );
    }
}

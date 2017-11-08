using KinderArtikelBoerse.Models;

namespace KinderArtikelBoerse.Contracts
{

    public interface IItemReader
    {
        Seller ReadItems( string filePath );
    }
}

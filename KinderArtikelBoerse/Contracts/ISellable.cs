namespace KinderArtikelBoerse.Contracts
{
    public interface ISellable
    {
        int Id { get; }

        string ItemIdentifier { get; set; }

        string Description { get; set; }

        string Size { get; set; }

        float Price { get; set; }

        bool IsSold { get; set; }

        int SellerId { get; }
    }
}

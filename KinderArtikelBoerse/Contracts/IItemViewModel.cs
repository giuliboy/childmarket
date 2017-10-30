namespace KinderArtikelBoerse.Contracts
{
    public interface IItemViewModel
    {
        string ItemIdentifier { get; }

        string Description { get; set; }

        bool IsSold { get; set; }
    }
}

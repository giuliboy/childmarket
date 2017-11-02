namespace KinderArtikelBoerse.Contracts
{
    public interface ISeller
    {
        int Id { get; set; }

        string Name { get; set; }

        string FirstName { get; set; }

        float FamilientreffPercentage { get; set; }
    }
}

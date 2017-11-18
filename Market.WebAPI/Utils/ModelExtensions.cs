using Market.Data;
using Market.WebAPI.ViewModel;
using System.Linq;

namespace Market.WebAPI.Utils
{
    public static class ModelExtensions
    {
        public static IQueryable<UserViewModel> ToViewModels( this IQueryable<Seller> sellers )
        {
            return sellers.Select( u => ToViewModel( u ) );
        }

        public static UserViewModel ToViewModel( this Seller u )
        {
            return new UserViewModel()
            {
                Name = u.Name,
                FirstName = u.FirstName,
                Email = u.Email,
            };
        }
        
    }

}

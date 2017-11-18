using Market.Data;
using Market.WebAPI.ViewModel;
using System.Collections.Generic;
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

        public static IEnumerable<ItemViewModel> ToViewModels( this IEnumerable<Item> items )
        {
            return items.Select( u => ToViewModel( u ) );
        }

        public static ItemViewModel ToViewModel( this Item i )
        {
            return new ItemViewModel()
            {
                ItemIdentifier = i.ItemIdentifier,
                Description = i.Description,
                Size = i.Size,
                Price = i.Price
            };
        }

    }

}

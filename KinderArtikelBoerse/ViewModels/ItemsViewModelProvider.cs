﻿using KinderArtikelBoerse.Contracts;
using Market.Service.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace KinderArtikelBoerse.Viewmodels
{
    public class ItemsViewModelProvider : IItemsProvider
    {
        public ItemsViewModelProvider( IMarketService dataService )
        {
            Items = new ObservableCollection<ItemViewModel>( dataService.Items
                .Select( i => new ItemViewModel( i ) )
               );
            //enumerate, else the consumers will enumerate again and instanciate additional item viewmodels
            //which is not the desired behavior
        }

        public IList<ItemViewModel> Items { get; }
    }

    
}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using ExtensibleCatalog.Models;
using ExtensibleCatalog.ViewModels;

namespace SimpleItem
{
    [Export(typeof(CatalogItem))]
    public class SimpleItem : CatalogItem, ListableItem, ItemDetail
    {
        public ItemListView ListView()
        {
            return new ItemListView()
            {
                Id = Id.ToString(),
                Name = Name,
                Price = Price.ToString(),
                Description = Name
            };
        }

        public VirtualFile DetailView(string virtualPath)
        {
            return new ExtensibleCatalog.Common.VirtualFileFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("SimpleItem.Detail.cshtml")
                                                                   , virtualPath);
        }

        public void FillViewBag(dynamic bag)
        {
            bag.Name = Name;
        }

        public string Name { get; set; }
        public decimal Price { get; set; }

    }
}

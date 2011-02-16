using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Hosting;
using ExtensibleCatalog.Models;
using ExtensibleCatalog.ViewModels;

namespace ComplexItem
{
    [Export(typeof(CatalogItem))]
    public class ComplexItem : CatalogItem, ListableItem, ItemDetail
    {

        public ItemListView ListView()
        {
            return new ItemListView()
            {
                Id = Id.ToString(),
                Name = Name,
                Price = Price.ToString(),
                Description = Description
            };
        }

        public VirtualFile DetailView(string virtualPath)
        {
            return new ExtensibleCatalog.Common.VirtualFileFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("ComplexItem.Detail.cshtml")
                                                                   , virtualPath);
        }

        public void FillViewBag(dynamic bag)
        {
            bag.Name = Name;
            bag.Sizes = Sizes;
            bag.Styles = Styles.Select(x => x.Name);
        }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<Style> Styles { get; set; }

    }
}

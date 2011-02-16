using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using ExtensibleCatalog.Common;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace ExtensibleCatalog.Controllers
{
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
    public class ItemController : Controller
    {

        MongoRepository _repo;

        [ImportingConstructor]
        public ItemController(MongoRepository repo)
        {
            _repo = repo;
        }

        public ActionResult Index()
        {
            var loadedItemNames = Items.Select(x => new BsonString(x.GetType().Name)).ToArray();
            var items = _repo.Items.FindAs<Models.CatalogItem>(Query.In("_t", loadedItemNames));
            var viewData = items.Cast<Models.ListableItem>().Select(x => x.ListView());

            return View(viewData);
        }

        public ActionResult Detail(string id)
        {

            var item = _repo.Items.FindOneAs<Models.CatalogItem>(Query.EQ("_id", new ObjectId(id)));
            return View(item.GetType().Name, item as Models.ItemDetail);

        }

        private ActionResult View(string name, Models.ItemDetail details)
        {
            if (details == null)
                return HttpNotFound();
            else
            {
                details.FillViewBag(ViewBag);
                return View(name);
            }
        }

        [ImportMany(AllowRecomposition = true)]
        public IEnumerable<Models.CatalogItem> Items { get; set; }

        public ActionResult Create()
        {

            //var server = MongoServer.Create(Properties.Settings.Default.MongoDBConn);
            //var db = server.GetDatabase("dyncat");
            //var catalog = db.GetCollection<Models.CatalogItem>("Catalog");

            //var one = new Models.BasicItem()
            //              {
            //                  Name = "A basic shirt",
            //                  Price = 780
            //              };

            //var two = new Models.ComplexItem()
            //              {
            //                  Name = "A complex jacket",
            //                  Description = "a description",
            //                  Price = 32,
            //                  Sizes = new List<string>() {"small", "medium", "large"},
            //                  Styles = new List<Models.Style>() {new Models.Style() {Name = "One", Color = "Red"}}
            //              };

            //catalog.Insert(one);
            //catalog.Insert(two);

            return new HttpStatusCodeResult(200);

        }

    }
}

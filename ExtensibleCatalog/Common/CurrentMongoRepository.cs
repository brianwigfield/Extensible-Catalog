using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace ExtensibleCatalog.Common
{
    [Export(typeof(MongoRepository))]
    public class CurrentMongoRepository : MongoRepository
    {
        readonly MongoDatabase _db;

        #region MongoRepository Members

        public CurrentMongoRepository()
        {
            var server = MongoServer.Create(Properties.Settings.Default.MongoDBConn);
            _db = server.GetDatabase("dyncat");
        }

        public MongoCollection<Models.CatalogItem> Items
        {
            get
            {
                return _db.GetCollection<Models.CatalogItem>("Catalog");
            }
        }

        #endregion
    }
}
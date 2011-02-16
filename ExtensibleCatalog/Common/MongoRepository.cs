using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtensibleCatalog.Common
{
    public interface MongoRepository
    {
        MongoDB.Driver.MongoCollection<Models.CatalogItem> Items { get; }
    }
}
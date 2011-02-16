using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ExtensibleCatalog.Common
{

    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MefDependencyResolver : IDependencyResolver
    {
        const string Key = "MefContainer";
        private readonly ComposablePartCatalog[] _catalogs;

        public MefDependencyResolver(params ComposablePartCatalog[] catalogs)
        {
            _catalogs = catalogs;
        }

        protected CompositionContainer Container
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                if (!HttpContext.Current.Items.Contains(Key))
                {
                    foreach (var catalog in _catalogs.Where(catalog => (catalog as DirectoryCatalog) != null))
                    {
                        ((DirectoryCatalog)catalog).Refresh();
                    }

                    try
                    {
                        HttpContext.Current.Items.Add(Key,
                            new CompositionContainer(new AggregateCatalog(_catalogs)));
                    }
                    catch (Exception ex)
                    {
                        //TODO for some reason the add will toss duplicate key error the first time when running after dropping in a new assembly in the part catalog regardless of the contains check returing false
                    }
                }   

                return (CompositionContainer)HttpContext.Current.Items[Key];
            }
        }

        public object GetService(Type serviceType)
        {
            var exports = this.Container.GetExports(serviceType, null, null);
            return exports.Any() ? exports.First().Value : null;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            var exports = this.Container.GetExports(serviceType, null, null);
            return exports.Any() ? exports.Select(e => e.Value).AsEnumerable() : Enumerable.Empty<object>();
        }
    }
}
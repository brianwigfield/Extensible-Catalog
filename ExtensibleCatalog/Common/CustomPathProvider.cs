using System.Web.Mvc;

namespace ExtensibleCatalog.Common
{
    public class CustomPathProvider : System.Web.Hosting.VirtualPathProvider
    {

        public override bool FileExists(string virtualPath)
        {
            //Basic check to prevent iteration
            if (virtualPath.StartsWith("/Views/Item/") == false)
                base.FileExists(virtualPath);

            var items = DependencyResolver.Current.GetServices<Models.CatalogItem>();
            foreach (var catalogItem in items)
            {
                if (virtualPath == string.Format("/Views/Item/{0}.cshtml", catalogItem.GetType().Name))
                {
                    var details = catalogItem as Models.ItemDetail;
                    if (details != null)
                        return true;
                }
            }

            return base.FileExists(virtualPath);
        }

        public override System.Web.Hosting.VirtualFile GetFile(string virtualPath)
        {
            var items = DependencyResolver.Current.GetServices<Models.CatalogItem>();
            foreach (var catalogItem in items)
            {
                if (virtualPath == string.Format("/Views/Item/{0}.cshtml", catalogItem.GetType().Name))
                {
                    var details = catalogItem as Models.ItemDetail;
                    if (details != null)
                        return details.DetailView(virtualPath);
                }
            }

            return base.GetFile(virtualPath);

        }

    }
}
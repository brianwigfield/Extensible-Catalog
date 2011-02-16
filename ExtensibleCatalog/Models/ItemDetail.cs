namespace ExtensibleCatalog.Models
{
    public interface ItemDetail
    {
        System.Web.Hosting.VirtualFile DetailView(string virtualPath);
        void FillViewBag(dynamic bag);
    }
}

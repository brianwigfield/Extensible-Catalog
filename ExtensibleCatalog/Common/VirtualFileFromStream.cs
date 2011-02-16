namespace ExtensibleCatalog.Common
{
    public class VirtualFileFromStream : System.Web.Hosting.VirtualFile
    {
        private readonly System.IO.Stream _stream;

        public VirtualFileFromStream(System.IO.Stream stream, string virtualPath) : base(virtualPath)
        {
            _stream = stream;
        }

        public override System.IO.Stream Open()
        {
            return _stream;
        }

    }
}
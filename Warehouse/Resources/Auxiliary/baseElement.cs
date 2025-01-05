namespace Warehouse.Resources.Auxiliary
{
    public class BaseElement
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public byte[]? image { get; set; }
        public BaseElement(byte[]? image = null, string? name = null, string? description = null) 
        {
            this.image = image;
            this.name = name;
            this.description = description;
        }

        public static ImageSource ConvertBytesToImageSource(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var stream = new MemoryStream(bytes);

            return ImageSource.FromStream(() => stream);
        }
    }
}

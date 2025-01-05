namespace Warehouse.Resources.Auxiliary
{
    public class BaseElement
    {
        public static int index {  get; set; }
        public int objectIndex { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public byte[]? image { get; set; }
        public BaseElement(byte[]? image = null, string? name = null, string? description = null) 
        {
            objectIndex = index;
            this.image = image;
            this.name = name;
            this.description = description;
            index++;

        }
        ~BaseElement()
        {
            index--;
        }

        public static ImageSource? ConvertBytesToImageSource(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var stream = new MemoryStream(bytes);

            return ImageSource.FromStream(() => stream);
        }
    }
}

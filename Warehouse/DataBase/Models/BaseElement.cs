using System.ComponentModel.DataAnnotations.Schema;
using DB = Warehouse.DataBase.WarehouseStaticContext;

namespace Warehouse.DataBase.Models
{
    public class BaseElement
    {
        [NotMapped]
        public static int countOfElements { get; set; } //general count of all elements getting max id from DB for synchronization 
        public int Id { get; set; }
        [NotMapped]
        public int objectIndex { get; set; } //current specific index of specific object
        public string? name { get; set; }
        public string? description { get; set; }
        public byte[]? image { get; set; } // array of bytes which could be converted to some image... Perhaps .png or .jpg
        public BaseElement()
        {
            Id = countOfElements;
            objectIndex = countOfElements;
            countOfElements++;
        }
        public BaseElement(byte[]? image = null, string? name = null, string? description = null)
        {
            Id = countOfElements;
            objectIndex = countOfElements;
            this.image = image;
            this.name = name;
            this.description = description;
            countOfElements++;

        }
        ~BaseElement()
        {
            countOfElements--;
        }

        public static ImageSource? ConvertBytesToImageSource(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
                return null;

            var stream = new MemoryStream(bytes);

            return ImageSource.FromStream(() => stream);
        }

        public bool notNull() => image != null && description != null && name != null;
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.PortableExecutable;

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

        [NotMapped]
        public ImageSource ImageSource
        {
            get
            {
                if (image == null)
                {
                    string unknownIconPath = Path.Join(AppContext.BaseDirectory,"..","..","..","..","..", "Resources", "Images", "unknown_object.png");
                    byte[]? unknown_icon = File.ReadAllBytes(unknownIconPath);
                    return ImageSource.FromStream(() => new MemoryStream(unknown_icon));
                }
                return ImageSource.FromStream(() => new MemoryStream(image));
            }
        }

        public BaseElement()
        {
            Id = countOfElements;
            objectIndex = countOfElements;
            countOfElements++;
        }
        public BaseElement(byte[]? image = null, string? name = null, string? description = null, int? id = null)
        {
            Id = (int)(id is null ? countOfElements : id);
            objectIndex = countOfElements;
            this.image = image;
            this.name = name;
            this.description = description;
            countOfElements++;

        }
        public BaseElement(BaseElement element)
        {
            if (element == null)
                throw new Exception("Element is null. Directory: " + Path.Join(AppContext.BaseDirectory, "DataBase", "Models", "BaseElement.cs"));
            Id = element.Id;
            objectIndex = element.objectIndex;
            image = element.image;
            name = element.name;
            description = element.description;
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

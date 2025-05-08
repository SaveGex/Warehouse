using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.DataBase.Models
{
    public class BaseElement : ObservableObject
    {
        public int Id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        [NotMapped]
        private byte[]? _image; 
        public byte[]? image
        {
            get => _image;
            set
            {
                SetProperty(ref _image, value);
                OnPropertyChanged(nameof(ImageSource)); // 🔄 тригер оновлення ImageSource
            }
        } // array of bytes which could be converted to some image... Perhaps .png or .jpg

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

        public BaseElement() { }

        public BaseElement(byte[]? image = null, string? name = null, string? description = null)
        {
            this.image = image;
            this.name = name;
            this.description = description;
        }

        public BaseElement(int id, byte[]? image = null, string? name = null, string? description = null)
        {
            this.Id = id;
            this.image = image;
            this.name = name;
            this.description = description;
        }

        public BaseElement(BaseElement element) // copying constructor
        {
            if (element == null)
                throw new Exception("Element is null. Directory: " + Path.Join(AppContext.BaseDirectory, "DataBase", "Models", "BaseElement.cs"));
            Id = element.Id;
            image = element.image;
            name = element.name;
            description = element.description;
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

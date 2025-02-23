using SQLite;
using System.ComponentModel.DataAnnotations.Schema;

namespace Warehouse.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("BaseElements")]
    public class BaseElement
    {
        [PrimaryKey]
        [AutoIncrement]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        public int id { get; set; } //id in the table with autoincrement. It's something like unique jey of each element... Perhaps....
        //[System.ComponentModel.DataAnnotations.Schema.Column("count_of_elements")]
        public static int countOfElements { get; set; } //general count of all elements
        [System.ComponentModel.DataAnnotations.Schema.Column("objectIndex")]
        public int objectIndex { get; set; } //current specific index of specific object
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        public string? name { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("description")]
        public string? description { get; set; }
        [System.ComponentModel.DataAnnotations.Schema.Column("image")]
        public byte[]? image { get; set; } // array of bytes which could be converted to some image... Perhaps .png or .jpg

        public BaseElement()
        {
            objectIndex = countOfElements;
            countOfElements++;
        }
        public BaseElement(byte[]? image = null, string? name = null, string? description = null)
        {
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
    }
}

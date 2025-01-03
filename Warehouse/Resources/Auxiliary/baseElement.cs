namespace Warehouse.Resources.Auxiliary
{
    class baseElement
    {
        public baseElement(string imageId = "Null", string name = "Null", string description = "Null") 
        {
            this.imageId = imageId;
            this.name = name;
            this.description = description;

        }
        public string imageId {  get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}

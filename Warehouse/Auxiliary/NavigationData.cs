using Warehouse.DataBase.Models;

namespace Warehouse.Auxiliary
{
    //literally buffer for contain some data for have access between any classes
    public static class NavigationData
    {
        public static BaseElement? CurrentBaseElement { get; set; }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Auxiliary.Patterns.Interfaces;
using Warehouse.DataBase;
using Warehouse.DataBase.Models;

namespace Warehouse.Auxiliary.Patterns
{
    public class DBManagerSubscriber  : ISubscriber
    {

        public DBManagerSubscriber() { }

        //"args" can get values such as "DELETE", "ADD"... and throughes exceptions if method requires a info
        public void Update(string? args = null, object? obj = null)
        {
            if (args == null)
                return;

            var context = MauiProgram.Services.GetRequiredService<WarehouseContext>();

            if (obj == null)
                throw new ArgumentNullException("Patterns/DBManagerSubscriber.cs" + nameof(obj));

            switch (args)
            {
                
                case "DELETE":
                    context.Remove(obj);
                    break;
                case "ADD":
                    var entity = obj as BaseElement; // Replace YourEntityType with the actual type
                    if (entity != null)
                    {
                        entity.Id = 0; // Ensure the ID is set to default value
                    }
                    context.Add(obj);
                    break;
                case "UPDATE":
                    context.Update(obj);
                    break;
                default:
                    throw new ArgumentException("Invalid argument value", nameof(args));
            }

            context.SaveChanges();
        }
    }
}

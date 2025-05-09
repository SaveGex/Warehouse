using Warehouse.Auxiliary.Patterns.Interfaces;
using Warehouse.DataBase;
using Warehouse.DataBase.Models;

namespace Warehouse.Auxiliary.Patterns
{
    public class DBManagerSubscriber : ISubscriber
    {
        public DBManagerSubscriber() { }

        //"args" can get values such as "DELETE", "ADD"... and throws exceptions if method requires info
        public void Update(string? args = null, object? obj = null, Dictionary<string, object>? qargs = null)
        {
            if (args == null)
                return;

            if (MauiProgram.Services == null)
                throw new InvalidOperationException("MauiProgram.Services is not initialized.");

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

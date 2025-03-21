
namespace Warehouse.Auxiliary.Patterns.Interfaces
{
    public interface ISubscriber
    {
        public void Update(string? args = null, object? obj = null);
    }
}

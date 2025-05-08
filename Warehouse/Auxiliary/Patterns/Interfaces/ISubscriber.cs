
namespace Warehouse.Auxiliary.Patterns.Interfaces;


public interface ISubscriber
{
    public abstract void Update(string? args = null, object? obj = null, Dictionary<string, object>? qargs = null);
}
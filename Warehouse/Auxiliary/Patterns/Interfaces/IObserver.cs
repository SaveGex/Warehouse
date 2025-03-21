using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Auxiliary.Patterns.Interfaces
{
    public interface IObserver
    {

        public bool AddSubscriber(ISubscriber? subscriber);
        public bool RemoveSubscriber(ISubscriber? subscriber);
        public void NotifySubscribers(string? args, object? obj);
    }
}

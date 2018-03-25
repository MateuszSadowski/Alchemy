using System;
using System.Collections.Generic;
using System.Threading;

namespace Alchemy
{
    public class Warehouse
    {
        public Semaphore SemWarehouse { get; set; }
        public Resource[] AvailableResources { get; set; }
        public Warehouse()
        {
            SemWarehouse = new Semaphore(1, 1);
            AvailableResources = new Resource[3];
        }

        public void PrintWarehouseMessage(string message)
        {
            Console.WriteLine("Warehouse " + message);
        }
    }
}
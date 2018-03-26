using System;
using System.Collections.Generic;
using System.Threading;

namespace Alchemy
{
    public class Warehouse
    {
        public Semaphore SemWarehouse { get; set; }
        public int[] AvailableResources { get; set; }
        public Semaphore[] SemFreeSlot { get; set;}
        public Warehouse()
        {
            SemWarehouse = new Semaphore(1, 1);
            SemFreeSlot = new Semaphore[3];
            SemFreeSlot[(int)Resource.Lead] = new Semaphore(2, 2);
            SemFreeSlot[(int)Resource.Mercury] = new Semaphore(2, 2);
            SemFreeSlot[(int)Resource.Sulfur] = new Semaphore(2, 2);
            AvailableResources = new int[3];
        }

        public void PrintWarehouseMessage(string message)
        {
            Console.WriteLine("Warehouse " + message);
        }
    }
}
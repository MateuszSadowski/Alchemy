using System;
using System.Collections.Generic;
using System.Threading;

namespace Alchemy
{
    public class Distributor
    {
        public Semaphore[] SemAlchemists { get; set;}
        public Semaphore SemResourceProduced { get; set; }
        public Warehouse Warehouse { get; set; }
        public Dictionary<AlchemistType, List<Resource>> requiredResources
            = new Dictionary<AlchemistType, List<Resource>>() 
        {
            { AlchemistType.A, new List<Resource>() { Resource.Lead, Resource.Mercury } },
            { AlchemistType.B, new List<Resource>() { Resource.Mercury, Resource.Sulfur } },
            { AlchemistType.C, new List<Resource>() { Resource.Lead, Resource.Sulfur } },
            { AlchemistType.D, new List<Resource>() { Resource.Mercury, Resource.Sulfur, Resource.Lead } }
        };
        public Distributor()
        {
            SemAlchemists = new Semaphore[4];
            SemAlchemists[(int)AlchemistType.A] = new Semaphore(1, 1);
            SemAlchemists[(int)AlchemistType.B] = new Semaphore(1, 1);
            SemAlchemists[(int)AlchemistType.C] = new Semaphore(1, 1);
            SemAlchemists[(int)AlchemistType.D] = new Semaphore(1, 1);
            SemResourceProduced = new Semaphore(1, 1);
            Warehouse = new Warehouse();
        }

        public void PrintDistributorMessage(string message)
        {
            Console.WriteLine("DISTRIBUTOR " + message);
        }
    }
}
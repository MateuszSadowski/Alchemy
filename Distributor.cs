using System;
using System.Collections.Generic;
using System.Threading;

namespace Alchemy
{
    public class Distributor
    {
        public Semaphore[] SemAlchemists { get; set;}
        public SemaphoreSlim SemResourceProduced { get; set; }
        public Semaphore SemAlchemistCount { get; set; }
        public int[] AlchemistCount { get; set; }
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
            SemAlchemists[(int)AlchemistType.A] = new Semaphore(0, 1);
            SemAlchemists[(int)AlchemistType.B] = new Semaphore(0, 1);
            SemAlchemists[(int)AlchemistType.C] = new Semaphore(0, 1);
            SemAlchemists[(int)AlchemistType.D] = new Semaphore(0, 1);
            SemResourceProduced = new SemaphoreSlim(0);
            SemAlchemistCount = new Semaphore(1, 1);
            AlchemistCount = new int[4];
            Warehouse = new Warehouse();
        }

        public void Work()
        {
            while(true)
            {
                PrintDistributorMessage("waits for new resource to be produced.");
                SemResourceProduced.Wait();  //wait for new resource
                PrintDistributorMessage("tries to distribute resources.");
                TryDistribute();
                //TODO: SemResourceProduced.Release(); ???
            }
        }

        public void TryDistribute()
        {
            SemAlchemistCount.WaitOne();
            for (int i = 0; i <= (int)AlchemistType.D; i++) //TODO: Start from Alchemist D
            {
                if(AlchemistCount[i] > 0)
                {
                    Warehouse.SemWarehouse.WaitOne();
                    bool allResourcesAvailable = true;
                    foreach (var resource in requiredResources[(AlchemistType)i])
                    {
                        allResourcesAvailable &= Warehouse.AvailableResources[(int)resource] > 0;
                    }
                    if(allResourcesAvailable)
                    {
                        PrintDistributorMessage("Success for alchemist of type " + (AlchemistType)i + i + ".");
                        foreach (var resource in requiredResources[(AlchemistType)i])
                        {   //distrubte 1 resource each
                            Warehouse.AvailableResources[(int)resource]--;
                            Warehouse.SemFreeSlot[(int)resource].Release();
                        }
                        SemAlchemists[i].Release();
                        AlchemistCount[i]--;
                    }
                    Warehouse.SemWarehouse.Release();
                }
            }
            SemAlchemistCount.Release();
        }

        public void PrintDistributorMessage(string message)
        {
            Console.WriteLine("DISTRIBUTOR " + message);
        }
    }
}
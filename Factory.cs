using System;
using System.Threading;

namespace Alchemy
{
    public class Factory
    {
        //public Semaphore SemFreeSlot { get; private set; }
        public Semaphore SemCurses { get; private set; }
        public Semaphore SemGoodToGo { get; private set; }
        public SemaphoreSlim SemResourceProduced { get; set; }
        public int Curses { get; set; }
        public Resource Resource {get; private set; }
        private Random random;
        // private int resourceCount;  //mock alchemists
        // private Semaphore SemAlchemist;
        public Warehouse Warehouse;

        public Factory(Resource res, Warehouse warehouse, SemaphoreSlim semResourceProduced)
        {
            //SemFreeSlot = new Semaphore(2, 2);
            SemCurses = new Semaphore(1, 1);
            SemGoodToGo = new Semaphore(1, 1);
            Resource = res;
            random = new Random();
            Warehouse = warehouse;
            SemResourceProduced = semResourceProduced;
            //mock alchemist
            // resourceCount = 0;
            // SemAlchemist = new Semaphore(1, 1);
        }

        public void Work()
        {
            while(true)
            {
                Produce();
                // switch(random.Next(1,3))    //will cause factories too block
                //                             //if resources full and try to produce
                // {
                //     case 1:
                //         Produce();
                //         break;
                //     case 2:
                //         Consume();  //mock alchemists
                //         break;
                // }
            }
        }

        // private void Consume()
        // {
        //     SemAlchemist.WaitOne();
        //     if(resourceCount > 0)
        //     {
        //         PrintFactoryMessage("is consuming a resource.");
        //         resourceCount--;
        //         SemFreeSlot.Release();
        //         SemAlchemist.Release();
        //     }
        //     else
        //     {
        //         SemAlchemist.Release();
        //     }
        // }

        private void Produce()
        {
            //PrintFactoryMessage("is waiting for a free slot...");
            Warehouse.SemFreeSlot[(int)Resource].WaitOne();  //wait for a free slot

            PrintFactoryMessage("has a free slot. Trying to produce...");
            SemGoodToGo.WaitOne();    //wait for the curses to be purged or continue if clean

            PrintFactoryMessage("is clean. Producing...");
            Thread.Sleep(random.Next(1, 5) * 1000);   //produce over random time duration
            Warehouse.SemWarehouse.WaitOne(); 
            Warehouse.AvailableResources[(int)Resource]++;   
            //signal that there is a resource available
            SemResourceProduced.Release();  //TODO: After or before releasing semwarehouse??
            Warehouse.SemWarehouse.Release();

            PrintFactoryMessage("finished production.");
            SemGoodToGo.Release();
        }

        public void PrintFactoryMessage(string message)
        {
            Console.WriteLine("[" + Resource + "] FACTORY " + message);
        }
    }
}

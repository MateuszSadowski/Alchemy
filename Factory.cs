using System;
using System.Threading;

namespace Alchemy
{
    public class Factory
    {
        public Semaphore SemFreeSlot { get; private set; }
        public Semaphore SemCurses { get; private set; }
        public Semaphore SemPurged { get; private set; }
        public int Curses { get; set; }
        public Resource Resource {get; private set; }
        private Random random;
        private int resourceCount;  //mock alchemists
        private Semaphore SemAlchemist;

        public Factory(Resource res)
        {
            SemFreeSlot = new Semaphore(2, 2);
            SemCurses = new Semaphore(1, 1);
            SemPurged = new Semaphore(0, 1);
            Resource = res;
            Curses = 0;
            random = new Random();
            //mock alchemist
            resourceCount = 0;
            SemAlchemist = new Semaphore(1, 1);
        }

        public void Work()
        {
            while(true)
            {
                switch(random.Next(1,2))
                {
                    case 1:
                        Produce();
                        break;
                    case 2:
                        Consume();  //mock alchemists
                        break;
                }
            }
        }

        private void Consume()
        {
            SemAlchemist.WaitOne();
            if(resourceCount > 0)
            {
                PrintFactoryMessage("is consuming a resource.");
                resourceCount--;
                SemFreeSlot.Release();
                SemAlchemist.Release();
            }
            else
            {
                SemAlchemist.Release();
            }
        }

        private void Produce()
        {
            PrintFactoryMessage("is waiting for a free slot.");
            SemFreeSlot.WaitOne();  //wait for a free slot
            PrintFactoryMessage("has a free slot. Checking if cursed.");
            SemCurses.WaitOne();    //check if there is no curse
            if(0 != Curses)
            {
                PrintFactoryMessage("has " + Curses + " curses left.");
                SemCurses.Release();
                PrintFactoryMessage("is waiting to be purged.");
                SemPurged.WaitOne();    //wait for the curses to be purged
                PrintFactoryMessage("has been PURGED.");
            }
            else
            {
                SemCurses.Release();
            }
            PrintFactoryMessage("is clean. Starting production.");
            SemAlchemist.WaitOne(); //mock alchemist
            resourceCount++;    //mock alchemist
            SemAlchemist.Release(); //mock alchemist
            Thread.Sleep(random.Next(1, 5) * 1000);   //produce over random time duration
            //signal that there is Resource available 
            PrintFactoryMessage("finished production.");
        }

        public void PrintFactoryMessage(string message)
        {
            Console.WriteLine("[" + Resource + "] FACTORY " + message);
        }
    }
}

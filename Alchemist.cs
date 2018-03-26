using System;
using System.Threading;

namespace Alchemy
{
    public class Alchemist
    {
        public int Id { get; private set; }
        public AlchemistType Type { get; private set; }

        public Alchemist(int id, AlchemistType type)
        {
            Id = id;
            Type = type;
            PrintAlchemistMessage("comes for resources.");
        }

        public void WaitForResources(Semaphore semAlchemist)
        {
            semAlchemist.WaitOne();
            //consume resource and quit
            PrintAlchemistMessage("consumes resources and quits.");
        }

        public void PrintAlchemistMessage(string message)
        {
            Console.WriteLine("[" + Id + ", " + Type + "] Alchemist " + message);
        }
    }
}
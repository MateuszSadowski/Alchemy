using System;
using System.Threading;

namespace Alchemy
{
    public class Warlock
    {
        private int Id { get; set; }
        private Random random;

        public Warlock(int id)
        {
            Id = id;
            random = new Random();
        }

        public void Curse(Factory[] factories)
        {
            Thread.Sleep(random.Next(1, 5) * 1000);

            Factory factory = factories[random.Next(0, factories.Length - 1)];

            factory.SemCurses.WaitOne();
            PrintWarlockMessage("is cursing " + factory.Resource + " factory.");
            if(0 == factory.Curses)
            {   //factory.SemPurged == 1
                factory.SemPurged.WaitOne();    //stop the factory from producing
                factory.Curses++;
                factory.SemCurses.Release();
            }
            else
            {
                factory.Curses++;
                factory.SemCurses.Release();
            }
        }

        public void PrintWarlockMessage(string message)
        {
            Console.WriteLine("[" + Id + "] WARLOCK " + message);
        }
    }
}

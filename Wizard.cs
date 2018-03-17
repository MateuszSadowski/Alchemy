using System;
using System.Threading;

namespace Alchemy
{
    public class Wizard
    {
        private int Id { get; set; }
        private Random random;

        public Wizard(int id)
        {
            Id = id;
            random = new Random();
        }

        public void Purge(Factory[] factories)
        {
            while(true)
            {
                Thread.Sleep(random.Next(1, 5) * 1000);
                PrintWizardMessage("is trying to purge curses.");

                foreach (var factory in factories)
                {
                    factory.SemCurses.WaitOne();
                    if(factory.Curses > 1)
                    {
                        PrintWizardMessage("is purging " + factory.Resource + " factory.");
                        factory.Curses--;
                        factory.SemCurses.Release();
                    }
                    else if(1 == factory.Curses)
                    {
                        PrintWizardMessage("is purging " + factory.Resource + " factory.");
                        factory.Curses--;
                        factory.SemPurged.Release();
                        factory.SemCurses.Release();
                    }
                    else
                    {
                        //no curses, release and move on
                        PrintWizardMessage("no curses to purge for " + factory.Resource + " factory.");
                        factory.SemCurses.Release();
                    }
                }
            }
        }

        public void PrintWizardMessage(string message)
        {
            Console.WriteLine("[" + Id + "] WIZARD " + message);
        }
    }
}

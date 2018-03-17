using System;
using System.Threading;

namespace Alchemy
{
    public enum Resource {Lead, Sulfur, Mercury};
    class Program
    {
        static Factory[] factories;
        static Wizard[] wizards;
        static Warlock[] warlocks;

        static void Main(string[] args)
        {
            InitializeFactories();
            InitializeWizards(3);
            InitializeWarlocs(3);
            
            RunFactories();
            RunWizards();
            RunWarlocks();
        }

        static void InitializeFactories()
        {
            factories = new Factory[3];
            factories[(int)Resource.Lead] = new Factory(Resource.Lead);
            factories[(int)Resource.Sulfur] = new Factory(Resource.Sulfur);
            factories[(int)Resource.Mercury] = new Factory(Resource.Mercury);
        }
        static void InitializeWizards(int n)
        {
            wizards = new Wizard[n];
            for (int i = 0; i < n; i++)
            {
                wizards[i] = new Wizard(i + 1);
            }
        }
        static void InitializeWarlocs(int n)
        {
            warlocks = new Warlock[n];
            for (int i = 0; i < n; i++)
            {
                warlocks[i] = new Warlock(i + 1);
            }
        }

        static void RunFactories()
        {
            foreach (var factory in factories)
            {
                new Thread(() => factories[(int)factory.Resource].Work()).Start();
            }
        }
        static void RunWizards()
        {
            foreach (var wizard in wizards)
            {
                new Thread(() => wizard.Purge(factories)).Start();
            }
        }
        static void RunWarlocks()
        {
            foreach (var warlock in warlocks)
            {
                new Thread(() => warlock.Curse(factories)).Start();
            }
        }
    }
}

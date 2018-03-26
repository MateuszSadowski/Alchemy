using System;
using System.Threading;
using System.Collections.Generic;

namespace Alchemy
{
    public enum Resource {Lead, Sulfur, Mercury};
    public enum AlchemistType {A, B, C, D};
    class Program
    {
        static Distributor distributor;
        static Factory[] factories;
        static Wizard[] wizards;
        static Warlock[] warlocks;
        static List<Alchemist> alchemists = new List<Alchemist>();
        static int nextAlchemistId { get { return alchemists.Count + 1; } set { } }
        static Random random = new Random();

        static void Main(string[] args)
        {
            distributor = new Distributor();
            InitializeFactories(distributor.Warehouse, distributor.SemResourceProduced);
            InitializeWizards(3);
            InitializeWarlocs(3);
            
            RunDistributor();
            RunFactories();
            RunWizards();
            RunWarlocks();
            GenerateAlchemists();

            System.Console.WriteLine("Initialized everything.");
        }

        static void InitializeFactories(Warehouse warehouse, SemaphoreSlim semResourceProduced)
        {
            factories = new Factory[3];
            factories[(int)Resource.Lead] = new Factory(Resource.Lead, warehouse, semResourceProduced);
            factories[(int)Resource.Sulfur] = new Factory(Resource.Sulfur, warehouse, semResourceProduced);
            factories[(int)Resource.Mercury] = new Factory(Resource.Mercury, warehouse, semResourceProduced);
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

        static void RunDistributor()
        {
            new Thread(() => distributor.Work()).Start();
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

        static void RunAlchemists()
        {
            while(true)
            {
                int alchemistType = random.Next(0, 4);
                var alchemist = new Alchemist(nextAlchemistId, (AlchemistType)alchemistType);
                alchemists.Add(alchemist);

                new Thread(() => alchemist.WaitForResources(distributor.SemAlchemists[(int)alchemist.Type]));   //wait for appropriate resources
                
                distributor.SemAlchemistCount.WaitOne();
                distributor.AlchemistCount[(int)alchemist.Type]++;
                distributor.SemAlchemistCount.Release();

                Thread.Sleep(random.Next(5, 31) * 1000);
            }
        }

        static void GenerateAlchemists()
        {
            new Thread(() => RunAlchemists()).Start();
        }
    }
}
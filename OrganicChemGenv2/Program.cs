using System;
using System.ComponentModel;
using System.Configuration;

namespace OrganicChemGen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Press enter to generate:\n");
                Console.ReadKey();
                Builder();
            }
        }
        
        static void Builder()
        {
            Random rnd = new Random();


            int max_carbon_count = Convert.ToInt32(ConfigurationManager.AppSettings.Get("max_carbon_count"));
            bool more_triple_bonds = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("more_triple_bonds"));

            string[] components_l = { "NH2", "NO2", " OH", " Br", "  F", "  I", " Cl", "   " };
            string[] components_r = { "NH2", "NO2", "OH", "Br", "F", "I", "Cl", "" };

            int c = rnd.Next(1, max_carbon_count+1);
            Console.WriteLine("Carbon count: " + c);

            if (c == 1)
            {
                Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}\n      |\n{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |\n      {components_r[rnd.Next(components_r.Length)]}");
            }
            else
            {
                Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}\n      |\n{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                int c2;
                int lim3 = 0;
                if ((c & 1) == 1) {c2 = c + 1;}
                else { c2 = c;}
                for (int i = 1; i < c2/2; i++)
                {
                    int bonds = rnd.Next(1, 4);
                    if (bonds == 1)
                    {
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                    }
                    else if (bonds == 2)
                    {
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      ||");
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      |");
                    }
                    else if (more_triple_bonds == true || (bonds == 3 && lim3 == 0))
                    {
                        lim3 = 1;
                        Console.WriteLine("      C\n     |||");
                        Console.WriteLine("      C\n      |");
                    }
                    else if (bonds == 3 && lim3 > 0 && more_triple_bonds == false)
                    {
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      ||");
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      |");
                    } 
                }
                if ((c & 1) == 0)
                {
                    Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |\n      {components_r[rnd.Next(components_r.Length)]}");
                }
                else
                {
                    Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}");
                }
            }
        }
    }
}
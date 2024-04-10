using System;
using System.ComponentModel;
using System.Configuration;

namespace OrganicChemGen
{
    internal class Program
    {
        static int max_carbon_count = Convert.ToInt32(ConfigurationManager.AppSettings.Get("max_carbon_count"));
        static int min_carbon_count = Convert.ToInt32(ConfigurationManager.AppSettings.Get("min_carbon_count"));
        static bool multiple_bonds = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("multiple_bonds"));
        static bool double_bonds = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("double_bonds"));
        static int max_double_bond_count = Convert.ToInt32(ConfigurationManager.AppSettings.Get("max_double_bond_count"));
        static bool triple_bonds = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("triple_bonds"));
        static int max_triple_bond_count = Convert.ToInt32(ConfigurationManager.AppSettings.Get("max_triple_bond_count"));
        static bool realistic_triple_bonds = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("realistic_triple_bonds"));

        static string[] components_l = { "NH2", "NO2", " OH", " Br", "  F", "  I", " Cl", "   " };
        static string[] components_r = { "NH2", "NO2", "OH", "Br", "F", "I", "Cl", "" };

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Press enter to generate:\n");
                Console.ReadKey();
                Builder(components_r, components_l, realistic_triple_bonds, max_triple_bond_count, triple_bonds, max_double_bond_count, double_bonds, multiple_bonds, max_carbon_count);
            }
        }
        
        static void Builder(string[]components_r, string[]components_l, bool realistic_triple_bonds, int max_triple_bond_count, bool triple_bonds, int max_double_bond_count, bool double_bonds, bool multiple_bonds, int max_carbon_count)
        {
            Random rnd = new Random();

            int c = rnd.Next(min_carbon_count, max_carbon_count + 1);
            Console.WriteLine("Carbon count: " + c);

            if (c == 1)
            {
                Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}\n      |\n{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |\n      {components_r[rnd.Next(components_r.Length)]}");
            }
            else
            {
                Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}\n      |\n{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                int c2;
                int blim2 = 0;
                int blim3 = 0;
                int lim3 = 0;
                if ((c & 1) == 1) {c2 = c + 1;}
                else { c2 = c;}
                int cc = c2 / 2;
                for (int i = 1; i < cc; i++)
                {
                    int bonds = 1;
                    if (multiple_bonds==true) {bonds = rnd.Next(1, 4); }
                    
                    if (bonds == 1)
                    {
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                        lim3 = 0;
                    }
                    else if (bonds == 2 && double_bonds == true)
                    {
                        if (max_double_bond_count == 0) 
                        {
                            Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      ||");
                            Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      |");
                            lim3 = 0;
                        }
                        else if (blim2 < max_double_bond_count) 
                        {
                            Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      ||");
                            Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      |");
                            lim3 = 0;
                            blim2++;
                        }

                    }
                    else if (bonds == 3 && triple_bonds == true)
                    {
                        if (realistic_triple_bonds == true)
                        {
                            if (max_triple_bond_count == 0) 
                            { 
                                if (lim3 == 0)
                                {
                                    lim3++;
                                    Console.WriteLine("      C\n     |||");
                                    Console.WriteLine("      C\n      |");
                                }
                                else if (lim3 > 0)
                                {
                                    cc++;
                                }
                            }
                            else if (blim3 < max_triple_bond_count)
                            {
                                if (lim3 == 0)
                                {
                                    lim3++;
                                    Console.WriteLine("      C\n     |||");
                                    Console.WriteLine("      C\n      |");
                                    blim3 = blim3+1;
                                }
                                else if (lim3 > 0)
                                {
                                    cc++;
                                }
                            }

                        }
                        else if (realistic_triple_bonds == false)
                        {
                            if (max_triple_bond_count == 0)
                            {
                                Console.WriteLine("      C\n     |||");
                                Console.WriteLine("      C\n      |");
                            }
                            else if (blim3 < max_triple_bond_count) 
                            {
                                Console.WriteLine("      C\n     |||");
                                Console.WriteLine("      C\n      |");
                                blim3++;
                            }
                        }

                    }
                    else if (((double_bonds == false || max_double_bond_count > blim2) && bonds == 2) || ((triple_bonds == false || max_triple_bond_count > blim3) && bonds == 3))
                    {
                        cc++;
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
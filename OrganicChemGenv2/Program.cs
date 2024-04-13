using System;
using System.ComponentModel;
using System.Configuration;
using System.Threading.Channels;

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
        static string[] components_l = ConfigurationManager.AppSettings["components"].Split(',');
        static string[] components_r = ConfigurationManager.AppSettings["components"].Split(',');
        static bool aldehydes = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("aldehydes_enable"));
        static bool ketones = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ketones_enable"));

        static Random rnd = new Random();

        static void Organizer(string[] components_l)
        {
            for (int i = 0; i < components_l.Length; i++)
            {
                if (components_l[i].Length == 1) { components_l[i] = "  " + components_l[i]; }
                else if (components_l[i].Length == 2) { components_l[i] = " " + components_l[i]; }
            }
        }

        static void Main(string[] args)
        {
            Organizer(components_l);
            while (true)
            {
                Console.WriteLine("Press enter to generate:\n");
                Console.ReadKey();
                Builder(realistic_triple_bonds, max_triple_bond_count, triple_bonds, max_double_bond_count, double_bonds, multiple_bonds, max_carbon_count);
            }
        }

        static void Builder(bool realistic_triple_bonds, int max_triple_bond_count, bool triple_bonds, int max_double_bond_count, bool double_bonds, bool multiple_bonds, int max_carbon_count)
        {


            int c = rnd.Next(min_carbon_count, max_carbon_count + 1);
            Console.WriteLine("Carbon count: " + c);

            if (c == 1)
            {
                Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}\n      |\n{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |\n      {components_r[rnd.Next(components_r.Length)]}");
            }
            else
            {
                PrintSingle(components_r, components_l, ketones);
                int c2;
                int blim2 = 0;
                int blim3 = 0;
                int lim3 = 0;
                if ((c & 1) == 1) { c2 = c + 1; }
                else { c2 = c; }
                int cc = c2 / 2;
                for (int i = 1; i < cc; i++)
                {
                    int bonds = 1;
                    if (multiple_bonds == true) { bonds = rnd.Next(1, 4); }

                    if (bonds == 1)
                    {
                        PrintSingle(components_r, components_l, ketones);
                        lim3 = 0;
                    }
                    else if (bonds == 2 && double_bonds == true)
                    {
                        if (max_double_bond_count == 0)
                        {
                            PrintDouble(components_r, components_l);
                            lim3 = 0;
                        }
                        else if (blim2 < max_double_bond_count)
                        {
                            PrintDouble(components_r, components_l);
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
                                    PrintTriple();
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
                                    PrintTriple();
                                    blim3 = blim3 + 1;
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
                                PrintTriple();
                            }
                            else if (blim3 < max_triple_bond_count)
                            {
                                PrintTriple();
                                blim3++;
                            }
                        }

                    }
                    else if (((double_bonds == false || max_double_bond_count > blim2) && bonds == 2) || ((triple_bonds == false || max_triple_bond_count > blim3) && bonds == 3))
                    {
                        cc++;
                    }
                }
                End(c, components_r, components_l, aldehydes);
            }
        }

        static void PrintSingle(string[] components_r, string[] components_l, bool ketones)
        {
            if (ketones ==  false) 
            { 
                Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
            }
            else if (ketones == true) 
            {
                int CO_gen = rnd.Next(0, 2);

                if (CO_gen == 0) 
                {
                    Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                    Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                }
                else if ( CO_gen == 1)
                {
                Console.WriteLine($"  O = C\n      |");
                Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |");
                }
            }
        }

        static void PrintDouble(string[] components_r, string[] components_l)
        {
            Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      ||");
            Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C\n      |");
        }

        static void PrintTriple()
        {
            Console.WriteLine("      C\n     |||");
            Console.WriteLine("      C\n      |");
        }

        static void End(int c, string[] components_r, string[] components_l, bool aldehydes)
        {
            if (aldehydes == true)
            {
                int CHO_gen = rnd.Next(0, 2);

                if (CHO_gen == 0)
                {
                    if ((c & 1) == 0)
                    {
                        Console.WriteLine($"{components_l[rnd.Next(components_l.Length)]} — C — {components_r[rnd.Next(components_r.Length)]}\n      |\n      {components_r[rnd.Next(components_r.Length)]}");
                    }
                    else
                    {
                        Console.WriteLine($"      {components_r[rnd.Next(components_r.Length)]}");
                    }
                }
                else if (CHO_gen == 1)
                {
                    if ((c & 1) == 0)
                    {
                        Console.WriteLine($"  H — C\n      ||\n      O");
                    }
                }

            }
            else
            {
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Vernyomas
{
    internal class Program
    {
        static void Main()
        {
            Menü();
        }
        static string Regisztracio()
        {
            Console.Clear();
            Console.WriteLine("Név: ");
            string reg_nev = Console.ReadLine();
            Console.WriteLine("Születési dátum (ÉÉÉÉ-HH-NN): ");
            string datum = Console.ReadLine();
            File.WriteAllText($"{reg_nev}.txt", $"Név: {reg_nev}\nDátum: {datum}");
            Console.Clear();
            return "Sikeres regisztráció!";
        }
        static string Bejelentkezes()
        {
            Console.Clear();
            Console.WriteLine("Milyen néven regisztrált?");
            string bej_nev = Console.ReadLine();
            string fajlnev = $"{bej_nev}.txt";
            if (File.Exists(fajlnev))
            {
                Console.Clear();
                string tartalom = File.ReadAllText(fajlnev);
                return $"Felhasználói adatok:\n{tartalom}";
            }
            else
            {
                return "Nincs ilyen felhasználó!";
            }
        }
        static void Menü()
        {
            string[] menuReszek = { "Regisztáció", "Bejelentkezés", "Kilépés" };
            int kivalasztott = 0;
            ConsoleKey keyPress;

            Console.CursorVisible = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("----- Főmenü -----\n");

                for (int i = 0; i < menuReszek.Length; i++)
                {
                    if (i == kivalasztott)
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"> {menuReszek[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {menuReszek[i]}");
                    }
                }


                keyPress = Console.ReadKey(true).Key;


                if (keyPress == ConsoleKey.Tab)
                {
                    kivalasztott = (kivalasztott + 1) % menuReszek.Length;
                }

                else if (keyPress == ConsoleKey.Enter)
                {
                    Console.Clear();
                    break;
                }
            }

            if (kivalasztott == 0)
            {
                Regisztracio();
            }
            else if (kivalasztott == 1)
            {
                Bejelentkezes();
            }
            else if (kivalasztott == 2)
            {
                string[] menuReszek2 = { "Igen", "Nem" };
                int kivalasztott2 = 0;
                ConsoleKey keyPress2;
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("Biztos ki szeretne lépni?\n");

                    for (int i = 0; i < menuReszek2.Length; i++)
                    {
                        if (i == kivalasztott2)
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine($"> {menuReszek2[i]}");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"  {menuReszek2[i]}");
                        }
                    }


                    keyPress2 = Console.ReadKey(true).Key;


                    if (keyPress2 == ConsoleKey.Tab)
                    {
                        kivalasztott2 = (kivalasztott2 + 1) % menuReszek2.Length;
                    }

                    else if (keyPress2 == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        break;
                    }
                }
                if (kivalasztott2 == 0)
                {
                    Environment.Exit(0);
                }
                else if (kivalasztott2 == 1)
                {
                    Main();
                }
            }

            Console.ResetColor();
            Console.CursorVisible = true;

            Console.ReadLine();
        }
    }
}

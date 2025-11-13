using System;
using System.IO;

namespace Vernyomas
{
    internal class Program
    {
        static void Main()
        {
            Menü();
        }
        /// <summary>
        /// Menü kezelő funkció, amely lehetővé teszi a felhasználó számára a regisztrációt, bejelentkezést vagy kilépést.
        /// </summary>
        
        
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

            Console.CursorVisible = true;

            if (kivalasztott == 0)
            {
                string eredmeny = Regisztracio();
                Console.WriteLine(eredmeny);
                Console.WriteLine("\nNyomj meg egy gombot a visszalépéshez...");
                Console.ReadKey();
                Menü();
                Regisztracio();
            }
            else if (kivalasztott == 1)
            {
                string eredmeny = Bejelentkezes();
                Console.WriteLine(eredmeny);
                Console.WriteLine("\nNyomj meg egy gombot a visszalépéshez...");
                Console.ReadKey();
                Menü();
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
                    Menü();
                }
            }
        }
        /// <summary>
        /// Regisztrációs funkció, amely létrehoz egy új felhasználói fájlt a megadott névvel és születési dátummal.
        /// </summary>
        /// <returns></returns>
        // --- Regisztráció ---
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
        /// <summary>
        /// Bejelentkezés funkció, amely ellenőrzi a felhasználó létezését, majd lehetővé teszi vérnyomásmérések rögzítését és értékelését.
        /// </summary>
        /// <returns></returns>
        // --- Bejelentkezés ---
        static string Bejelentkezes()
        {
            Console.Clear();
            Console.WriteLine("Milyen néven regisztrált?");
            string bej_nev = Console.ReadLine();
            string fajlnev = $"{bej_nev}.txt";
            return Meres(fajlnev);
        }

        private static string Meres(string fajlnev)
        {
            if (File.Exists(fajlnev))
            {
                Console.Clear();
                string tartalom = File.ReadAllText(fajlnev);
                Console.WriteLine($"Felhasználói adatok:\n{tartalom}");
                Console.WriteLine("\nKérem, adja meg az életkorát: ");
                int eletkor = int.Parse(Console.ReadLine());

                double osszSziszt = 0;
                double osszDiaszt = 0;

                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine($"\n--- {i}. mérés ---");
                    Console.Write("Szisztolés érték (felső): ");
                    int sziszt = int.Parse(Console.ReadLine());
                    Console.Write("Diasztolés érték (alsó): ");
                    int diaszt = int.Parse(Console.ReadLine());

                    VerNyomas vernyomas = new VerNyomas(sziszt, diaszt, eletkor);
                    string ertekeles = vernyomas.Ertekeles();

                    // Szín a kockázati szinthez
                    if (ertekeles.Contains("Magas") || ertekeles.Contains("Válságos"))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine($"Vérnyomás: {sziszt}/{diaszt} mmHg — {ertekeles}");
                    Console.ResetColor();

                    string datum = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    File.AppendAllText(fajlnev, $"\n{datum} — {sziszt}/{diaszt}, {ertekeles}");

                    osszSziszt += sziszt;
                    osszDiaszt += diaszt;
                }

                double atlagSziszt = osszSziszt / 5;
                double atlagDiaszt = osszDiaszt / 5;

                VerNyomas atlagVernyomas = new VerNyomas((int)atlagSziszt, (int)atlagDiaszt, eletkor);
                string atlagErtekeles = atlagVernyomas.Ertekeles();

                Console.WriteLine("\n--- Átlagolt érték ---");
                if (atlagErtekeles.Contains("Magas") || atlagErtekeles.Contains("Válságos"))
                    Console.ForegroundColor = ConsoleColor.Red;
                else
                    Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine($"Átlagos vérnyomás: {atlagSziszt:F0}/{atlagDiaszt:F0} mmHg — {atlagErtekeles}");
                Console.ResetColor();

                File.AppendAllText(fajlnev, $"\nÁtlag: {atlagSziszt:F0}/{atlagDiaszt:F0}, {atlagErtekeles}\n");

                return "\nAz adatok sikeresen rögzítve!";
            }
            else
            {
                return "Nincs ilyen felhasználó!";
            }
        }
    }

    // --- Vérnyomás osztály ---
    /// <summary>
    /// A vérnyomás értékeléséért felelős osztály, amely a szisztolés és diasztolés értékek alapján meghatározza a vérnyomás kategóriáját.
    /// </summary>
    class VerNyomas
    {
        public int Szisztoles { get; set; }
        public int Diasztoles { get; set; }
        public int Eletkor { get; set; }

        // Konstruktor
        public VerNyomas(int sziszt, int diaszt, int eletkor)
        {
            Szisztoles = sziszt;
            Diasztoles = diaszt;
            Eletkor = eletkor;
        }

        public string Ertekeles()
        {
            if(Eletkor==25){ 
                if (Szisztoles < 120 && Diasztoles < 80)
                    return "Normális";
                else if (Szisztoles >= 120 && Szisztoles <= 129 && Diasztoles < 80)
                    return "Emelkedett";
                else if ((Szisztoles >= 130 && Szisztoles <= 139) || (Diasztoles >= 80 && Diasztoles <= 89))
                    return "Magas vérnyomás (1. fok)";
                else if ((Szisztoles >= 140 && Szisztoles <= 180) || (Diasztoles >= 90 && Diasztoles <= 120))
                    return "Magas vérnyomás (2. fok)";
                else if (Szisztoles > 180 || Diasztoles > 120)
                    return "Válságos (keressen orvost)";
                else
                    return "Ismeretlen érték";
            }
            return "teszt";
        }
    }
}

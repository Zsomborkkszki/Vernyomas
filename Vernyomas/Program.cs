using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

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
            //ide kell egy while ciklus
            DateTime dt;
            if (DateTime.TryParseExact(datum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                File.WriteAllText($"{reg_nev}.txt", $"Név: {reg_nev}\nDátum: {datum}");
                Console.Clear();
                return "Sikeres regisztráció!";
            }
            else
            {
                return("Helytelen formátum! Próbálja újra!");
            }

            
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
                //Ezt ki kell szedni majd
                Console.WriteLine("\nKérem, adja meg az életkorát: ");
                int eletkor = int.Parse(Console.ReadLine());
                //eddig
                double osszSziszt = 0;
                double osszDiaszt = 0;

                
                var sisztList = new List<int>();
                var diasztList = new List<int>();

                for (int i = 1; i <= 5; i++)
                {
                    Console.WriteLine($"\n--- {i}. mérés ---");
                    Console.Write("Szisztolés érték (felső): ");
                    int sziszt = int.Parse(Console.ReadLine());
                    Console.Write("Diasztolés érték (alsó): ");
                    int diaszt = int.Parse(Console.ReadLine());

                    
                    sisztList.Add(sziszt);
                    diasztList.Add(diaszt);

                    VerNyomas vernyomas = new VerNyomas(sziszt, diaszt, eletkor);
                    string ertekeles = vernyomas.Ertekeles();

                    // Szín a kockázati szinthez
                    if (ertekeles.Contains("Magas") || ertekeles.Contains("Válságos") || ertekeles.Contains("Ismeretlen érték"))
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

                
                string maxMinResult = VerNyomas.MaxMin(sisztList, diasztList);
                Console.WriteLine("\n--- Maximum és minimum ---");
                Console.WriteLine(maxMinResult);
                File.AppendAllText(fajlnev, $"\n{maxMinResult}\n");

                Console.WriteLine("\n--- Magas vérnyomások előfordulása---");
                int hanyszorMagas = VerNyomas.HanyszorMagas(sisztList, diasztList, eletkor);
                Console.WriteLine($"\nMagas vérnyomás előfordulása: {hanyszorMagas} alkalom");
                File.AppendAllText(fajlnev, $"\nMagas vérnyomás előfordulása: {hanyszorMagas} alkalom\n");

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
        public int Szisztoles;
        public int Diasztoles;
        public int Eletkor;

        // Konstruktor
        public VerNyomas(int sziszt, int diaszt, int eletkor)
        {
            Szisztoles = sziszt;
            Diasztoles = diaszt;
            Eletkor = eletkor;
        }

        public string Ertekeles()
        {
            // ===== 1–12 év =====
            if (Eletkor >= 1 && Eletkor <= 12)
            {
                if (Szisztoles >= 90 && Szisztoles <= 110 && Diasztoles >= 55 && Diasztoles <= 75)
                    return "Normális";
                else if (Szisztoles >= 111 && Szisztoles <= 119 && Diasztoles < 80)
                    return "Emelkedett";
                else if ((Szisztoles >= 120 && Szisztoles <= 129) || (Diasztoles >= 76 && Diasztoles <= 79))
                    return "Magas vérnyomás (1. fok)";
                else if ((Szisztoles >= 130 && Szisztoles <= 160) || (Diasztoles >= 80 && Diasztoles <= 100))
                    return "Magas vérnyomás (2. fok)";
                else if (Szisztoles > 160 || Diasztoles > 100)
                    return "Válságos (keressen orvost)";
                else
                    return "Ismeretlen érték";
            }

            // ===== 12–18 év =====
            else if (Eletkor > 12 && Eletkor <= 18)
            {
                if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
                    return "Normális";
                else if (Szisztoles >= 120 && Szisztoles <= 129 && Diasztoles >= 70 && Diasztoles <= 79)
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

            // ===== 18–25 év =====
            else if (Eletkor > 18 && Eletkor <= 25)
            {
                if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
                    return "Normális";
                else if (Szisztoles >= 120 && Szisztoles <= 129 && Diasztoles >= 70 && Diasztoles <= 79)
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

            // ===== 25–40 év =====
            else if (Eletkor > 25 && Eletkor <= 40)
            {
                if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
                    return "Normális";
                else if (Szisztoles >= 120 && Szisztoles <= 129 && Diasztoles >= 70 && Diasztoles <= 79)
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

            // ===== 40–60 év =====
            else if (Eletkor > 40 && Eletkor <= 60)
            {
                if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
                    return "Normális";
                else if (Szisztoles >= 120 && Szisztoles <= 129 && Diasztoles >= 70 && Diasztoles <= 79)
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

            // ===== 60+ év =====
            else if (Eletkor > 60)
            {
                if (Szisztoles >= 110 && Szisztoles <= 129 && Diasztoles >= 60 && Diasztoles <= 69)
                    return "Normális";
                else if (Szisztoles >= 130 && Szisztoles <= 139 && Diasztoles >= 70 && Diasztoles <= 79)
                    return "Emelkedett";
                else if ((Szisztoles >= 140 && Szisztoles <= 149) || (Diasztoles >= 80 && Diasztoles <= 89))
                    return "Magas vérnyomás (1. fok)";
                else if ((Szisztoles >= 150 && Szisztoles <= 180) || (Diasztoles >= 90 && Diasztoles <= 120))
                    return "Magas vérnyomás (2. fok)";
                else if (Szisztoles > 180 || Diasztoles > 120)
                    return "Válságos (keressen orvost)";
                else
                    return "Ismeretlen érték";
            }
            return "teszt";
        }

        /// <summary>
        /// Meghatározza a mért értékek maximumát és minimumát.
        /// Bemenet: listák az 5 mérés szisztolés és diasztolés értékeivel.
        /// Visszatérés: rövid, emberi olvasható szöveg a max/min eredményekkel.
        /// </summary>
        public static string MaxMin(List<int> sisztList, List<int> diasztList)
        {
            if (sisztList == null || diasztList == null)
                return "Nincsenek mérési adatok.";

            if (sisztList.Count == 0 || diasztList.Count == 0)
                return "Nincsenek mérési adatok.";

            int maxSziszt = sisztList.Max();
            int minSziszt = sisztList.Min();
            int maxDiaszt = diasztList.Max();
            int minDiaszt = diasztList.Min();

            return $"Szisztolés — Max: {maxSziszt} mmHg, Min: {minSziszt} mmHg\nDiasztolés — Max: {maxDiaszt} mmHg, Min: {minDiaszt} mmHg";
        }

        /// <summary>
        /// Meghatározza a mért értékek alapján, hogy hányszor mért magas vérnyomást.
        /// Bemenet: listák az 5 mérés szisztolés és diasztolés értékeivel és a megadott életkor.
        /// Visszatérés: Azoknak a méréseknek a száma, amelyek magas vagy válságos vérnyomást jeleznek.
        /// </summary>
        public static int HanyszorMagas(List<int> sisztList, List<int> diasztList, int eletkor)
        {
            int hanyszor = 0;
            for (int i = 0; i < sisztList.Count; i++)
            {
                int sziszt = sisztList[i];
                int diaszt = diasztList[i];

                VerNyomas vernyomas = new VerNyomas(sziszt, diaszt, eletkor);

                string ertekeles = vernyomas.Ertekeles();
                if (ertekeles.Contains("Magas") || ertekeles.Contains("Válságos"))
                {
                    hanyszor++;
                }
            }
            return hanyszor;
        }
        public static int Kor(string datum)
        {
            var darabolas = datum.Split('-');
            int ev = int.Parse(darabolas[0]);
            int honap = int.Parse(darabolas[1]);
            int nap = int.Parse(darabolas[2]);

            int age = DateTime.Now.Year - ev;

            if (DateTime.Now.Month < honap || (DateTime.Now.Month == honap && DateTime.Now.Day < nap))
            {
                age--;
            }

            return age;
        }
        
    }
}
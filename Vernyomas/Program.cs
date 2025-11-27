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
            }
            else if (kivalasztott == 1)
            {
                string eredmeny = Bejelentkezes();
                Console.WriteLine(eredmeny);
                Console.WriteLine("\nNyomj meg egy gombot a visszalépéshez...");
                Console.ReadKey();
                Menü();
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

        static string Regisztracio()
        {
            string datum = "";
            string reg_nev = "";
            DateTime dt;
            Console.Clear();
            Console.WriteLine("Név: ");
            reg_nev = Console.ReadLine();

            do
            {
                Console.WriteLine("Születési dátum (ÉÉÉÉ-HH-NN): ");
                datum = Console.ReadLine();
            }
            while (!DateTime.TryParseExact(datum, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt));
            File.AppendAllText("Users.txt", $"{reg_nev}");
            
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
            return Meres(fajlnev);
        }

        private static string Meres(string fajlnev)
        {
            if (File.Exists(fajlnev))
            {
                Console.Clear();
                string tartalom = File.ReadAllText(fajlnev);
                Console.WriteLine($"Felhasználói adatok:\n{tartalom}");
                double osszSziszt = 0;
                double osszDiaszt = 0;

                var sisztList = new List<int>();
                var diasztList = new List<int>();

                string datumSzul = null;
                var sorok = tartalom.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (sorok.Length >= 2)
                {
                    var parts = sorok[1].Split(':');
                    if (parts.Length >= 2)
                        datumSzul = parts[1].Trim();
                }

                if (string.IsNullOrEmpty(datumSzul))
                {
                    return "A felhasználói fájl nem tartalmaz érvényes születési dátumot (yyyy-MM-dd).";
                }
                Console.Write("Mennyi adatot szeretne megadni:");
                int adatSzam = int.Parse(Console.ReadLine());
                for (int i = 1; i <= adatSzam; i++)
                {
                    Console.WriteLine($"\n--- {i}. mérés ---");
                    Console.Write("Szisztolés érték (felső): ");
                    int sziszt = int.Parse(Console.ReadLine());
                    Console.Write("Diasztolés érték (alsó): ");
                    int diaszt = int.Parse(Console.ReadLine());

                    sisztList.Add(sziszt);
                    diasztList.Add(diaszt);

                    string meresDatum = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    VerNyomas vernyomas = new VerNyomas(sziszt, diaszt, datumSzul);
                    string ertekeles = vernyomas.Ertekeles();

                    if (ertekeles.Contains("Magas") || ertekeles.Contains("Válságos") || ertekeles.Contains("Ismeretlen érték"))
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine($"Vérnyomás: {sziszt}/{diaszt} mmHg — {ertekeles}");
                    Console.ResetColor();

                    File.AppendAllText(fajlnev, $"\n{meresDatum} — {sziszt}/{diaszt}, {ertekeles}");

                    osszSziszt += sziszt;
                    osszDiaszt += diaszt;
                }

                double atlagSziszt = osszSziszt / 5;
                double atlagDiaszt = osszDiaszt / 5;

                VerNyomas atlagVernyomas = new VerNyomas((int)atlagSziszt, (int)atlagDiaszt, datumSzul);
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
                int hanyszorMagas = VerNyomas.HanyszorMagas(sisztList, diasztList, datumSzul);
                Console.WriteLine($"\nMagas vérnyomás előfordulása: {hanyszorMagas} alkalom");
                File.AppendAllText(fajlnev, $"\nMagas vérnyomás előfordulása: {hanyszorMagas} alkalom\n");

                Console.WriteLine($"\nMagas vérnyomás előfordulása: {hanyszorMagas} alkalom");

                string osszesitett = OsszesitettAtlag(atlagSziszt, atlagDiaszt);
                Console.WriteLine(osszesitett);
                File.AppendAllText(fajlnev, $"\n{osszesitett}\n");


                return "\nAz adatok sikeresen rögzítve!";
            }
            else
            {
                return "Nincs ilyen felhasználó!";
            }
        }

        static string OsszesitettAtlag(double felhAtlagSziszt, double felhAtlagDiaszt)
        {
            if (!File.Exists("Users.txt"))
                return "Nincs User.txt, nem lehet összesített átlagot számolni!";

            string[] userNevek = File.ReadAllLines("Users.txt");

            double osszSziszt = 0;
            double osszDiaszt = 0;
            int felhasznaloSzam = 0;

            foreach (var nev in userNevek)
            {
                string fajl = $"{nev}.txt";
                if (!File.Exists(fajl)) continue;

                var sorok = File.ReadAllLines(fajl);

                // Megkeressük az "Átlag:" sor utolsó előfordulását
                string atlagSor = sorok.LastOrDefault(s => s.StartsWith("Átlag:"));
                if (atlagSor == null) continue;

                // Példa sor: "Átlag: 120/80, Normális"
                var darabolt = atlagSor.Split(' ', ',', '/', ':');

                // darabolt = ["Átlag", "", "120", "80", ...]
                if (darabolt.Length < 4) continue;

                if (int.TryParse(darabolt[2], out int sziszt) &&
                    int.TryParse(darabolt[3], out int diaszt))
                {
                    osszSziszt += sziszt;
                    osszDiaszt += diaszt;
                    felhasznaloSzam++;
                }
            }

            if (felhasznaloSzam == 0)
                return "Nincs elég adat más felhasználóktól!";

            double osszesitettSziszt = osszSziszt / felhasznaloSzam;
            double osszesitettDiaszt = osszDiaszt / felhasznaloSzam;

            // Százalékos eltérés a bejelentkezett felhasználó átlagától
            double szisztElteres = ((osszesitettSziszt - felhAtlagSziszt) / felhAtlagSziszt) * 100.0;
            double diasztElteres = ((osszesitettDiaszt - felhAtlagDiaszt) / felhAtlagDiaszt) * 100.0;

            return $"\n--- Összesített átlag más felhasználókból ---\n" +
                   $"Szisztolés: {osszesitettSziszt:F1} mmHg ({szisztElteres:F1}% eltérés)\n" +
                   $"Diasztolés: {osszesitettDiaszt:F1} mmHg ({diasztElteres:F1}% eltérés)";
        }

        class VerNyomas
        {
            public int Szisztoles;
            public int Diasztoles;
            public string Datum;

            public VerNyomas(int sziszt, int diaszt, string datum)
            {
                Szisztoles = sziszt;
                Diasztoles = diaszt;
                Datum = datum;
            }

            public string Ertekeles()
            {
                int kor = Kor(Datum);

                if (kor >= 1 && kor <= 12)
                {
                    if (Szisztoles < 90 && Diasztoles < 55)
                        return "Alacsony";
                    else if (Szisztoles >= 90 && Szisztoles <= 110 && Diasztoles >= 55 && Diasztoles <= 75)
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
                else if (kor > 12 && kor <= 18)
                {
                    if (Szisztoles < 100 && Diasztoles < 60)
                        return "Alacsony";
                    else if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
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
                else if (kor > 18 && kor <= 25)
                {
                    if (Szisztoles < 100 && Diasztoles < 60)
                        return "Alacsony";
                    else if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
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
                else if (kor > 25 && kor <= 40)
                {
                    if (Szisztoles < 100 && Diasztoles < 60)
                        return "Alacsony";
                    else if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
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
                else if (kor > 40 && kor <= 60)
                {
                    if (Szisztoles < 100 && Diasztoles < 60)
                        return "Alacsony";
                    else if (Szisztoles >= 100 && Szisztoles <= 119 && Diasztoles >= 60 && Diasztoles <= 69)
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
                else if (kor > 60)
                {
                    if (Szisztoles < 110 && Diasztoles < 60)
                        return "Alacsony";
                    else if (Szisztoles >= 110 && Szisztoles <= 129 && Diasztoles >= 60 && Diasztoles <= 69)
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

            public static int HanyszorMagas(List<int> sisztList, List<int> diasztList, string szuletesiDatum)
            {
                int hanyszor = 0;
                for (int i = 0; i < sisztList.Count; i++)
                {
                    int sziszt = sisztList[i];
                    int diaszt = diasztList[i];

                    VerNyomas vernyomas = new VerNyomas(sziszt, diaszt, szuletesiDatum);

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

                int kor = DateTime.Now.Year - ev;

                if (DateTime.Now.Month < honap || (DateTime.Now.Month == honap && DateTime.Now.Day < nap))
                {
                    kor--;
                }

                return kor;
            }
        }
    }
}

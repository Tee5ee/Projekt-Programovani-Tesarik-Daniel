using System.IO;

namespace tesarik_projekt
{
    internal class Program
    {
        static int NacteniCeny()
        {
            using (StreamReader sr = new StreamReader(@"AktualniObjednavka.txt")) //nacteni souboru, ktery obsahuje aktualni objednavku
            {
                string Radek;
                string[] PoleRadku = new string[2];
                int Cena = 0;
                while (!(sr.EndOfStream)) //prochazeni souboru dokud nedojdeme na konec
                {
                    Radek = sr.ReadLine();
                    PoleRadku = Radek.Split(';'); //rozdeleni radku na pole, oddelovac mezera
                    try
                    {
                        Cena = Cena + int.Parse(PoleRadku[1]); //pridani ceny z pole do celkove ceny objednavky
                    }
                    catch
                    {
                        Console.WriteLine("\nSoubor byl zkorumpován!");
                        return -1;
                    }
                }
                return Cena;
            }
        }

        static void ZapisAktualniObjednavky(string[] a, int[] b, int c) // a = seznam jidel, b = cenik, c = index jídla
        {
            string ZapisovaneJidlo = a[c];
            int[] Cenik = b;
            using (StreamWriter sw = new StreamWriter(@"AktualniObjednavka.txt", true)) // true - pripsani nove hodnoty
            {
                sw.WriteLine($"{ZapisovaneJidlo};{Cenik[c]}"); //zapis do souboru ve formatu "nazev jídla;cena"
            }
        }

        static void ExportAktualniObjednavky()
        {
            if (File.Exists("ExistujiciObjednavka.txt")) //kontrola jestli soubor existujiic objednavky jiz existuje
            {
                File.Delete("ExistujiciObjednavka.txt"); //pokud ano - smaze ho
            }

            Console.Clear();
            File.Copy(@"AktualniObjednavka.txt", @"ExistujiciObjednavka.txt", true); //zkopiruje soubor aktualni objednavky ve formatu: zdrojovy soubor, cilovy soubor
            Console.WriteLine("Objednávka byla úspěšně exportována do souboru ExistujiciObjednavka.txt");
        }

        static int NacteniExistujiciObjednavky()
        {
            if (File.Exists("ExistujiciObjednavka.txt")) //kontrola jestli soubor existujiic objednavky jiz existuje
            {
                File.Copy(@"ExistujiciObjednavka.txt", @"AktualniObjednavka.txt", true); //pokud ano - zkopiruje soubor exportovane objednavky a pojmenuje ho na format souboru aktualni objednavky
                return 1;
            }
            else
            {
                Console.WriteLine("Soubor existující objednávky nebyl nalezen!\nUjistěte se, zda má soubor správný formát a je ve stejném adresáři jako soubor aplikace!");
                return 0;
            }
        }

        static void SmazaniPolozky(int a)
        {
            int indexRadku = a - 1; //poradova cisla se vypisuji od 1 - musi se posunout index o 1 dolu
            List<string> radky = File.ReadAllLines("AktualniObjednavka.txt").ToList(); //nacteni vsech radku do listu

            if (indexRadku >= 0 && indexRadku < radky.Count) //pokud neni vstupni index 0 a zaroven neni vetsi nez pocet radku
            {
                Console.Clear();
                radky.RemoveAt(indexRadku); //vymazani polozky z listu
                File.WriteAllLines("AktualniObjednavka.txt", radky); //vypsani listu zpet do souboru
                Console.WriteLine("Položka byla úspěšně smazána.\n");
            }
        }

        static void MenuObjednavky()
        {
            string Odpoved;
            while (true)
            {
                Console.WriteLine("Objednávka:\n-----");
                using (StreamReader sr = new StreamReader(@"AktualniObjednavka.txt"))
                {
                    string radek;
                    string[] PoleRadek = new string[2];
                    int PoradoveCislo = 1;
                    while ((radek = sr.ReadLine()) != null) //vypisovaci logika jidel
                    {
                        PoleRadek = radek.Split(';'); //jako oddelovac je vybrany charakter ;
                        Console.WriteLine($"{PoradoveCislo} - {PoleRadek[0]} {PoleRadek[1]}");
                        PoradoveCislo++;
                    }
                }
                int CenaObjednavky = NacteniCeny();
                if (CenaObjednavky == 0)
                {
                    Console.WriteLine("\nPrázdná objednávka!\n");
                    Console.WriteLine("-----");
                }
                else
                {
                    if (CenaObjednavky == -1)
                    {
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.WriteLine($"\nCena: {CenaObjednavky} Kč\n");
                    }
                    Console.WriteLine("-----");
                    Console.WriteLine("\n[0] - Smazat položku (0 <mezera> pořadové číslo)");
                }

                Console.WriteLine("[1] - Zpět do menu\n");

                Console.Write("> ");
                Odpoved = Console.ReadLine();

                try
                {
                    switch (int.Parse(Odpoved[0].ToString())) //prevedeni prvni hodnoty stringu na integer
                    {
                        case 0:
                            try
                            {
                                int Polozka1 = int.Parse(Odpoved[2].ToString());
                                int Polozka2 = int.Parse(Odpoved[3].ToString());
                                int Polozka = Polozka1 * 10 + Polozka2; //nacteme stringy cisel pro dvojciferna poradova cisla
                                SmazaniPolozky(Polozka);
                            }
                            catch
                            {
                                Console.Clear();
                                Console.WriteLine("Neplatné pořadové číslo!");
                                continue;
                            }
                            break;

                        case 1:
                            Console.Clear();
                            return;
                        default:
                            Console.Clear();
                            Console.WriteLine("Neplatná volba!\n");
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Neplatná volba!\n");
                    continue;
                }
            }

        }

        static int PotvrzeniKonce()
        {
            while (true)
            {
                Console.WriteLine("Opravdu chcete ukončit objednávku? (Y/N)\n");

                Console.Write("> ");
                string Odpoved = Console.ReadLine();
                if (Odpoved == "y" || Odpoved == "Y")
                {
                    return 1; //pokud ano - funkce vrati hodnotu 1
                }
                else
                {
                    if (Odpoved == "n" || Odpoved == "N")
                    {
                        Console.Clear();
                        Console.WriteLine("Úspěšně vráceno do menu\n");
                        return 0;
                    }
                    Console.Clear();
                    Console.WriteLine("Neplatná volba!");
                    return 0; //pokud ano - funkce vrati hodnotu 0
                }
            }
        }

        static int Konec(string[] a)
        {
            while (true)
            {
                string[] SeznamJidel = a;
                int OpravduKonec = 0;

                string[] NacteneRadky = File.ReadAllLines("AktualniObjednavka.txt"); //nacteni vsech radku do pole
                Console.WriteLine("Vaše objednávka:\n-----");

                using (StreamReader sr = new StreamReader(@"AktualniObjednavka.txt"))
                {
                    string radek;
                    string[] PoleRadek = new string[2];
                    while ((radek = sr.ReadLine()) != null) //vypisovaci logika objednavky
                    {
                        PoleRadek = radek.Split(';');
                        Console.WriteLine($"{PoleRadek[0]} {PoleRadek[1]}");
                    }
                }

                int CelkovaCena = NacteniCeny();
                if (CelkovaCena == 0)
                {
                    Console.WriteLine("\nPrázdná objednávka!\n");
                }
                else
                {
                    if (CelkovaCena == -1)
                    {
                        Console.Write("\n");
                    }
                    else
                    {
                        Console.WriteLine($"\nCena: {CelkovaCena} Kč");
                    }
                }
                Console.WriteLine("-----");
                Console.WriteLine("Děkujeme za Vaši objednávku! Přejete si jí exportovat? (Y/N)\n");

                Console.Write("> ");
                string ExportOdpoved = Console.ReadLine();

                if (ExportOdpoved == "y" || ExportOdpoved == "Y")
                {
                    ExportAktualniObjednavky();
                    OpravduKonec = PotvrzeniKonce();
                    if (OpravduKonec == 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    if (ExportOdpoved == "n" || ExportOdpoved == "N")
                    {
                        Console.Clear();
                        OpravduKonec = PotvrzeniKonce();
                        if (OpravduKonec == 1)
                        {
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Neplatná volba!");
                        return 0;
                    }
                }
            }
        }

        static void Main() 
        {
            int Odpoved = 0; //deklarace a inicializace promennych
            string[] SeznamJidel = { "Šunkofleky", "Koprovka", "Svíčková", "Guláš", "Rajská", "Kuřecí řízek" };
            int[] Cenik = { 115, 666, 130, 135, 130, 105 };
            bool PrvniIterace = true;
            int PotvrzeniKonce = 0;

            if (File.Exists("AktualniObjednavka.txt")) //pokud existuje soubor aktualni objednavky - smaze ho a vytvori novy, cisty
            {
                File.Delete("AktualniObjednavka.txt");
                File.Create("AktualniObjednavka.txt").Close();
            }
            else
            {
                File.Create("AktualniObjednavka.txt").Close();
            }

            do //hlavni cyklus programu
            {
                if (PrvniIterace == true) //import pri prvni iteraci cyklu
                {
                    bool Import;

                    Console.WriteLine("Vítejte v naší restauraci!");
                    Console.WriteLine("Přejete si vytvořit novou objednávku [1], nebo importovat již existující [2] ?\n");


                    Console.Write("> ");
                    try
                    {
                        bool tempImport = int.Parse(Console.ReadLine()) == 2 ? true : false; //parsenuti vstupu -> porovnani hodnoty -> nastaveni bool hodnoty
                        Import = tempImport;
                    }
                    catch
                    {
                        Console.WriteLine("Nepltatná volba!");

                        Console.Clear();
                        continue;
                    }

                    if (Import == true) //nacitani existujici objednavky
                    {

                        Console.Clear();
                        int UspesneNacteni = NacteniExistujiciObjednavky();
                        if (UspesneNacteni == 1) //kontrola zda se soubor spravne nacetl
                        {
                            Console.WriteLine("Objednávka byla úspěšně importována.\n");
                            Console.WriteLine("Přejete si upravit objednávku? (Y/N)\n");

                            Console.Write("> ");
                            string PokracovaniPoImportu = Console.ReadLine();

                            if (PokracovaniPoImportu == "y" || PokracovaniPoImportu == "Y")
                            {
                                Console.WriteLine("Úspěšně vráceno do menu\n");//jdeme do menu
                            }
                            else if (PokracovaniPoImportu == "n" || PokracovaniPoImportu == "N")
                            {
                                Console.Clear();

                                PotvrzeniKonce = Konec(SeznamJidel);
                                if (PotvrzeniKonce == 1)
                                {
                                    return;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Neplatná volba!");
                            }
                        }
                        Console.Clear();
                    }
                    else
                    {
                        Console.Clear();
                    }
                }

                Console.WriteLine("Co si přejete objednat?");
                Console.WriteLine("-----");

                Console.WriteLine($"[0] - Šunkofleky   ( {Cenik[0]},- )");
                Console.WriteLine($"[1] - Koprovka     ( {Cenik[1]},- )");
                Console.WriteLine($"[2] - Svíčková     ( {Cenik[2]},- )");
                Console.WriteLine($"[3] - Guláš        ( {Cenik[3]},- )");
                Console.WriteLine($"[4] - Rajská       ( {Cenik[4]},- )");
                Console.WriteLine($"[5] - Kuřecí řízek ( {Cenik[5]},- )");
                Console.WriteLine("");
                Console.WriteLine("[7] - Importovat objednávku");
                Console.WriteLine("[8] - Zobrazit objednávku");
                Console.WriteLine("[9] - Konec objednávky");

                int CelkovaCena = NacteniCeny(); //zavolani funkce NacteniCeny() - vypise cenu podle prvku v souboru aktualniobjednavka
                if (CelkovaCena == 0)
                {
                    Console.WriteLine("\nPrázdná objednávka!\n");
                    Console.WriteLine("-----\n");
                }
                else
                {
                    if (CelkovaCena == -1)
                    {
                        Console.Write("\n-----\n\n");
                    }
                    else
                    {
                        Console.WriteLine($"\nCena: {CelkovaCena} Kč");
                        Console.WriteLine("-----\n");
                    }
                }

                PrvniIterace = false; //zruseni inicialni nabidky

                try
                {
                    Console.Write("> ");
                    Odpoved = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Neplatná volba!\n");
                    continue;
                }

                switch (Odpoved)
                {
                    //jidla
                    case 0:
                        Console.Clear();
                        Console.WriteLine("Bylo přidáno do objednávky: " + SeznamJidel[0] + "\n");
                        ZapisAktualniObjednavky(SeznamJidel, Cenik, Odpoved);
                        break;
                    case 1:
                        Console.Clear();
                        Console.WriteLine("Bylo přidáno do objednávky: " + SeznamJidel[1] + "\n");
                        ZapisAktualniObjednavky(SeznamJidel, Cenik, Odpoved);
                        break;
                    case 2:
                        Console.Clear();
                        Console.WriteLine("Bylo přidáno do objednávky: " + SeznamJidel[2] + "\n");
                        ZapisAktualniObjednavky(SeznamJidel, Cenik, Odpoved);
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Bylo přidáno do objednávky: " + SeznamJidel[3] + "\n");
                        ZapisAktualniObjednavky(SeznamJidel, Cenik, Odpoved);
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Bylo přidáno do objednávky: " + SeznamJidel[4] + "\n");
                        ZapisAktualniObjednavky(SeznamJidel, Cenik, Odpoved);
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Bylo přidáno do objednávky: " + SeznamJidel[5] + "\n");
                        ZapisAktualniObjednavky(SeznamJidel, Cenik, Odpoved);
                        break;

                    //ostatni volby
                    case 7: //import po prvni iteraci cyklu
                        Console.Clear();
                        int UspesneNacteni = NacteniExistujiciObjednavky(); //kontrola zda se soubor spravne nacetl
                        if (UspesneNacteni == 1)
                        {
                            Console.WriteLine("Objednávka byla úspěšně importována.\n");
                            Console.WriteLine("Přejete si upravit objednávku? (Y/N)\n");

                            Console.Write("> ");
                            string PokracovaniPoImportu = Console.ReadLine();
                            if (PokracovaniPoImportu == "y" || PokracovaniPoImportu == "Y")
                            {
                                Console.Clear();
                                break;
                            }
                            else
                            {
                                Console.Clear();
                                if (PokracovaniPoImportu == "n" || PokracovaniPoImportu == "N")
                                {
                                    PotvrzeniKonce = Konec(SeznamJidel);
                                    if (PotvrzeniKonce == 1)
                                    {
                                        return;
                                    }
                                    break;
                                }
                                Console.Clear();
                                Console.WriteLine("Neplatná volba!");
                            }
                        }
                        
                        break;

                    case 8:
                        Console.Clear();
                        MenuObjednavky();
                        break;

                    case 9: //konec
                        Console.Clear();
                        PotvrzeniKonce = Konec(SeznamJidel);
                        if (PotvrzeniKonce == 1)
                        {
                            return;
                        }
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Neplatná volba!\n");
                        break;
                }
            } while (true);
        }
    }
}
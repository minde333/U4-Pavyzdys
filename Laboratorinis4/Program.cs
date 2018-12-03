//Arminas Marozas
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorinis4
{
    class Program
    {
        public const int VisuZodziuDydis = 100000;  //Masyvo, kuriame talpinami visi teksto žodžiai, dydis
        public const int IlgiausiuZodziuDydis = 10; //Masyvo, kuriame talpinami ilgiausi žodžiai, dydis
        static void Main(string[] args)
        {
            const string CFd = "..\\..\\Knyga1.txt";  //Pirmas duomenų failas
            const string CFd2 = "..\\..\\Knyga2.txt"; //Antras duomenų failas
            const string CFr = "..\\..\\Rodikliai.txt"; //Pirmas rezultatų failas
            const string CFr2 = "..\\..\\ManoKnyga.txt"; //Antras rezultatų faials

            //Patikrina, ar toks rezultatų failas jau yra, jeigu taip, tada jį ištrina
            if (File.Exists(CFr))
            {
                File.Delete(CFr);
            }

            Console.OutputEncoding = Encoding.UTF8; //Konsolėje rašomos lietuviškos raidės
            Program p = new Program(); //Sukuriamas Program klasės objektas
            char[] skyrikliai = { ' ', '.', ',', '!', '?', ':', ';', '(', ')', '\t', '"' };  //Skyriklių aibė

            //Sukuriami žodžių konteinerio obejktai, kuriuose talpinami žodžiai be skyriklių, ir apskaičiuojami pasikartojimai
            ZodziuKonteineris PirmosKnygosZodziai = new ZodziuKonteineris(VisuZodziuDydis);
            ZodziuKonteineris AntrosKnygosZodziai = new ZodziuKonteineris(VisuZodziuDydis);

            //Sukuriami žodžių konteinerio objektai, kuriuose talpinami žodžiai su skyrikliais tokia eilės tvarka, kokia yra tekste
            ZodziuKonteineris ZodziaiSuSkyrikliaisPirmas = new ZodziuKonteineris(VisuZodziuDydis);
            ZodziuKonteineris ZodziaiSuSkyrikliaisAntras = new ZodziuKonteineris(VisuZodziuDydis);

            //Apdoroja failus, ir sudeda žodžius į atitinkamus konteinerius
            p.Apdorojimas(CFd, ref PirmosKnygosZodziai, skyrikliai, ref ZodziaiSuSkyrikliaisPirmas);
            p.Apdorojimas(CFd2, ref AntrosKnygosZodziai, skyrikliai, ref ZodziaiSuSkyrikliaisAntras);


            ZodziuKonteineris IlgiausiZodziai = new ZodziuKonteineris(IlgiausiuZodziuDydis); //Žodžių konteineris, talpinantis ilgiausius žodžius
            IlgiausiZodziai = p.IlgiausiZodziai(PirmosKnygosZodziai, AntrosKnygosZodziai); //Surandami ilgiausi žodžiai

            //Patikrina, ar yra ilgiausių žodžių, jeigu taip, tada juos atspausdina
            if (IlgiausiZodziai.Kiekis == 0)
            {
                Console.WriteLine("Duomenų failai tušti arba antrame tekste yra visi pirmo failo ilgiausi žodžiai");
                p.IlgiausiuZodziuSpausdinimasFaile(IlgiausiZodziai, CFr, PirmosKnygosZodziai);
            }
            else
            {
                p.IlgiausiuZodziuSpausdinimasFaile(IlgiausiZodziai, CFr, PirmosKnygosZodziai);
            }


            FragmentuKonteineris Fragmentai = new FragmentuKonteineris(); //Fragmentų konteinerio objektas
            Fragmentai = p.IlgiausiFragmentai(ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras); //Surandami ilgiausi fragmentai

            //Patikrina ar yra ilgiausių fragmentų, jeigu taip, tada juos atspausdina kartu su eilučių numeriais
            if (Fragmentai.Count == 0)
            {
                Console.WriteLine("Duomenų failai tušti arba tekstuose nėra vienodų fragmentų");
                p.IlgiausiuFragmentuSpausdinimasFaile(Fragmentai, CFr, ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras);
            }
            else
            {
                p.IlgiausiuFragmentuSpausdinimasFaile(Fragmentai, CFr, ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras);
            }


            StringBuilder Tekstas = new StringBuilder();  //Bendro teksto objektas
            Tekstas = p.TekstoPertvarkymas(ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras); //Pagal taisykles sudaro vieną tekstą


            //Patikrina ar tekstas yra, jeigu taip, tada jį atspausdina
            if (Tekstas.Length == 0)
            {
                Console.WriteLine("Duomenų failuose nėra");
                p.PertvarkytoTekstoSpausdinimasFaile(CFr2, ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras);
            }
            else
            {
                p.PertvarkytoTekstoSpausdinimasFaile(CFr2, ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras);
            }


        }

        /// <summary>
        /// Pagal taisykles iš dviejų tekstų padaro vieną
        /// </summary>
        /// <param name="ZodziaiSuSkyrikliaisPirmas">Pirmo failo žodžiai su skyrikliais</param>
        /// <param name="ZodziaiSuSkyrikliaisAntras">Antro failo žodžiai su skyrikliais</param>
        /// <returns>Sudarytą bendrą tekstą</returns>
        StringBuilder TekstoPertvarkymas(ZodziuKonteineris ZodziaiSuSkyrikliaisPirmas, ZodziuKonteineris ZodziaiSuSkyrikliaisAntras)
        {
            StringBuilder VisasTekstas = new StringBuilder();
            int k = 0;
            int AntroZodzioPradzia = 0;
            for (int i = 0; i < ZodziaiSuSkyrikliaisPirmas.Kiekis; i++)
            {
                string PirmasZodis = Regex.Replace(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Pavadinimas, "[.,;()\"]", String.Empty);
                string AntrasZodis = Regex.Replace(ZodziaiSuSkyrikliaisAntras.PaimtiZodi(AntroZodzioPradzia).Pavadinimas, "[.,;()\"]", String.Empty);
                if (PirmasZodis != AntrasZodis)
                {
                    VisasTekstas.Append(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Pavadinimas);
                    VisasTekstas.Append(" ");
                }
                else
                {
                    if (i + 1 < ZodziaiSuSkyrikliaisPirmas.Kiekis)
                    {
                        PirmasZodis = Regex.Replace(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i + 1).Pavadinimas, "[.,;()\"]", String.Empty);
                    }
                    else
                    {
                       
                        break;
                    }
                    for (int g = AntroZodzioPradzia; g < ZodziaiSuSkyrikliaisAntras.Kiekis; g++)
                    {
                        AntrasZodis = Regex.Replace(ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g).Pavadinimas, "[.,;()\"]", String.Empty);
                        if (PirmasZodis != AntrasZodis)
                        {
                            VisasTekstas.Append(ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g).Pavadinimas);
                            VisasTekstas.Append(" ");
                            if (g == ZodziaiSuSkyrikliaisAntras.Kiekis - 1)
                            {
                                AntroZodzioPradzia = ZodziaiSuSkyrikliaisAntras.Kiekis - 1;
                            }
                        }
                        else
                        {
                            
                            if (g + 1 < ZodziaiSuSkyrikliaisAntras.Kiekis - 1)
                            {
                                AntroZodzioPradzia = g + 1;
                            }
                            else
                            {
                                AntroZodzioPradzia = ZodziaiSuSkyrikliaisAntras.Kiekis;

                            }
                            break;
                        }
                    }
                }


            }

            if (AntroZodzioPradzia < ZodziaiSuSkyrikliaisAntras.Kiekis - 1)
            {
                for (int i = AntroZodzioPradzia; i < ZodziaiSuSkyrikliaisAntras.Kiekis; i++)
                {
                    VisasTekstas.Append(ZodziaiSuSkyrikliaisAntras.PaimtiZodi(i).Pavadinimas);
                    VisasTekstas.Append(" ");
                }
            }
          

            return VisasTekstas;

        }


        /// <summary>
        /// Suranda ilgiausio fragmento simbolių skaičių
        /// </summary>
        /// <param name="ZodziaiSuSkyrikliaisPirmas">Pirmo failo žodžiai su skyrikliais</param>
        /// <param name="ZodziaiSuSkyrikliaisAntras">Antro failo žodžiai su skyrikliais</param>
        /// <returns>Ilgiausio fragmento elementų skaičių</returns>
        int IlgiausioFragmentoElementuSkaicius(ZodziuKonteineris ZodziaiSuSkyrikliaisPirmas, ZodziuKonteineris ZodziaiSuSkyrikliaisAntras)
        {
            StringBuilder Didziausias = new StringBuilder();

            for (int i = 0; i < ZodziaiSuSkyrikliaisPirmas.Kiekis; i++)
            {
                for (int g = 0; g < ZodziaiSuSkyrikliaisAntras.Kiekis; g++)
                {
                    if (ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Pavadinimas.ToLower() == ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g).Pavadinimas.ToLower())
                    {
                        StringBuilder nauja = new StringBuilder();

                        int o = 1;
                        nauja.Append(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Pavadinimas.ToLower());
                        nauja.Append(" ");

                        while (o > 0)
                        {
                            if (i + o >= ZodziaiSuSkyrikliaisPirmas.Kiekis || g + o >= ZodziaiSuSkyrikliaisAntras.Kiekis)
                            {
                                break;
                            }
                            if (ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i + o).Pavadinimas.ToLower() == ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g + o).Pavadinimas.ToLower())
                            {
                                nauja.Append(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i + o).Pavadinimas.ToLower());
                                nauja.Append(" ");
                                o++;
                            }
                            else
                            {
                                o = 0;
                            }

                        }


                        if (nauja.Length >= Didziausias.Length)
                        {
                            Didziausias = nauja;

                        }

                    }
                }
            }

            return Didziausias.Length;
        }

        /// <summary>
        /// Sudaro fragmentus ir juos įdeda į ilgiausių fragmentų konteinerį
        /// </summary>
        /// <param name="ZodziaiSuSkyrikliaisPirmas">Pirmo failo žodžiai su skyrikliais</param>
        /// <param name="ZodziaiSuSkyrikliaisAntras">Antro failo žodžiai su skyrikliais</param>
        /// <returns>Ilgiausių fragmentų konteinerį</returns>
        FragmentuKonteineris IlgiausiFragmentai(ZodziuKonteineris ZodziaiSuSkyrikliaisPirmas, ZodziuKonteineris ZodziaiSuSkyrikliaisAntras)
        {
            FragmentuKonteineris Fragmentai = new FragmentuKonteineris();
            for (int i = 0; i < ZodziaiSuSkyrikliaisPirmas.Kiekis; i++)
            {
                for (int g = 0; g < ZodziaiSuSkyrikliaisAntras.Kiekis; g++)
                {
                    EilutesNumeriuKonteineris Numeriai = new EilutesNumeriuKonteineris();
                    if (ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Pavadinimas.ToLower() == ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g).Pavadinimas.ToLower())
                    {
                        StringBuilder nauja = new StringBuilder();

                        int o = 1;
                        nauja.Append(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Pavadinimas.ToLower());
                        nauja.Append(" ");

                        Numeriai.IdetiNumeri(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i).Eilute);

                        Numeriai.IdetiNumeri(ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g).Eilute);

                        while (o > 0)
                        {
                            if (i + o >= ZodziaiSuSkyrikliaisPirmas.Kiekis || g + o >= ZodziaiSuSkyrikliaisAntras.Kiekis)
                            {
                                break;
                            }
                            if (ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i + o).Pavadinimas.ToLower() == ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g + o).Pavadinimas.ToLower())
                            {

                                nauja.Append(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i + o).Pavadinimas.ToLower());

                                Numeriai.IdetiNumeri(ZodziaiSuSkyrikliaisPirmas.PaimtiZodi(i + o).Eilute);
                                Numeriai.IdetiNumeri(ZodziaiSuSkyrikliaisAntras.PaimtiZodi(g + o).Eilute);
                                nauja.Append(" ");
                                o++;
                            }
                            else
                            {
                                o = 0;
                            }

                        }


                        if (nauja.Length == IlgiausioFragmentoElementuSkaicius(ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras))
                        {
                            Fragmentai.IdetiFragmenta(nauja.ToString());
                            Fragmentai.IdetiNumerius(Numeriai);
                        }



                    }
                }
            }
            return Fragmentai;

        }



        /// <summary>
        /// Apdoroja duomenis
        /// </summary>
        /// <param name="fv">Duomenų failo pavadinimas</param>
        /// <param name="VisiZodziai">Žodžiai be skyriklių</param>
        /// <param name="skyrikliai">Skyriklių aibė</param>
        /// <param name="ZodziaiSuSkyrikliais">Žodžiai su skyrikliais</param>
        void Apdorojimas(string fv, ref ZodziuKonteineris VisiZodziai, char[] skyrikliai, ref ZodziuKonteineris ZodziaiSuSkyrikliais)
        {
            int eilute = 0;
            string[] lines = File.ReadAllLines(fv, Encoding.UTF8);
            foreach (string line in lines)
            {
                eilute++;
                if (line.Length > 0)
                {
                    ZodziuApdorojimas(line, skyrikliai, ref VisiZodziai, ref ZodziaiSuSkyrikliais, eilute);
                }
            }

        }

        /// <summary>
        /// Apdoroja žodžius
        /// </summary>
        /// <param name="line">Eilutė</param>
        /// <param name="skyrikliai">Skyriklių aibė</param>
        /// <param name="VisiZodziai">Žodžiai be skyriklių</param>
        /// <param name="ZodziaiSuSKyrikliais">Žodžiai su skyrikliais</param>
        /// <param name="eilute">Eilutės numeris</param>
        void ZodziuApdorojimas(string line, char[] skyrikliai, ref ZodziuKonteineris VisiZodziai, ref ZodziuKonteineris ZodziaiSuSKyrikliais, int eilute)
        {

            string[] parts = line.Split(skyrikliai, StringSplitOptions.RemoveEmptyEntries);
            string[] dalys = line.Split(' ');
            foreach (string dalis in dalys)
            {
                if (dalis.Length > 0)
                {
                    Zodis Zodziukas = new Zodis(dalis, 0, eilute);
                    ZodziaiSuSKyrikliais.PridetiZodi(Zodziukas);
                }
            }
            foreach (string zodis in parts)
            {
                if (zodis.Length > 1 || zodis.Length == 1 && ArSimbolisYraRaide(zodis))
                {

                    string zodelis = zodis.ToLower();
                    if (VisiZodziai.ArYraToksPavadinimas(zodelis))
                    {
                        for (int i = 0; i < VisiZodziai.Kiekis; i++)
                        {
                            if (VisiZodziai.PaimtiZodi(i).Pavadinimas == zodelis)
                            {
                                VisiZodziai.PaimtiZodi(i).Pasikartojimai++;
                                break;
                            }
                        }
                    }
                    else
                    {
                        Zodis Zodziukas = new Zodis(zodelis, 0, eilute);
                        VisiZodziai.PridetiZodi(Zodziukas);
                    }
                }
            }

        }

        /// <summary>
        /// Sudeda ilgiausius žodžius, kurių nėra antrame faile, į konteinerį
        /// </summary>
        /// <param name="PirmosKnygosZodziai">Pirmo failo žodžiai be skyriklių</param>
        /// <param name="AntrosKnygosZodziai">Antro failo žodžiai be skyriklių</param>
        /// <returns>Ilgiausių žodžių konteinerį</returns>
        ZodziuKonteineris IlgiausiZodziai(ZodziuKonteineris PirmosKnygosZodziai, ZodziuKonteineris AntrosKnygosZodziai)
        {
            ZodziuKonteineris Ilgiausi = new ZodziuKonteineris(IlgiausiuZodziuDydis);
            bool found = false;
            for (int i = 0; i < PirmosKnygosZodziai.Kiekis; i++)
            {
                if (PirmosKnygosZodziai.PaimtiZodi(i).Pavadinimas.Length == IlgiausioZodzioRaidziuSkaicius(PirmosKnygosZodziai))
                {

                    if (AntrosKnygosZodziai.ArYraToksPavadinimas(PirmosKnygosZodziai.PaimtiZodi(i).Pavadinimas))
                    {
                        found = true;
                    }
                    if (found == false && Ilgiausi.Kiekis != 10)
                    {
                        Ilgiausi.PridetiZodi(PirmosKnygosZodziai.PaimtiZodi(i));

                    }
                    found = false;
                }
            }

            return Ilgiausi;
        }

        int IlgiausioZodzioRaidziuSkaicius(ZodziuKonteineris VisiPirmosKnygosZodziai)
        {
            int ilgis = 0;
            for (int i = 0; i < VisiPirmosKnygosZodziai.Kiekis; i++)
            {
                if (VisiPirmosKnygosZodziai.PaimtiZodi(i).Pavadinimas.Length >= ilgis)
                {
                    ilgis = VisiPirmosKnygosZodziai.PaimtiZodi(i).Pavadinimas.Length;
                }
            }
            return ilgis;

        }

        /// <summary>
        /// Patikrina ar duoto string elemento pirma raidė yra raidė. Naudojama, kai string sudarytas iš vieno simbolio
        /// </summary>
        /// <param name="simb">String elementas</param>
        /// <returns>Ar pirmoji radiė yra raidė</returns>        bool ArSimbolisYraRaide(string simb)
        {
            if (char.IsLetter(simb[0]))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Atspausdina ilgiausius žodžius faile
        /// </summary>
        /// <param name="IlgiausiZodziai">Ilgiausių žodžių konteineris</param>
        /// <param name="fv">Failas, į kurį rašys</param>
        /// <param name="PirmosKnygosZodziai">Pirmo failo žodžiai be skyriklių</param>
        void IlgiausiuZodziuSpausdinimasFaile(ZodziuKonteineris IlgiausiZodziai, string fv, ZodziuKonteineris PirmosKnygosZodziai)
        {
            using (StreamWriter writer = new StreamWriter(fv, true, Encoding.UTF8))
            {
                if (IlgiausiZodziai.Kiekis == 0)
                {
                    writer.WriteLine("Duomenų failai tušti arba antrame tekste yra visi pirmo failo ilgiausi žodžiai");
                }
                else
                {
                    writer.WriteLine("Ilgiausi ({0} raidžių) žodžiai:", IlgiausioZodzioRaidziuSkaicius(PirmosKnygosZodziai));
                    for (int i = 0; i < IlgiausiZodziai.Kiekis; i++)
                    {
                        writer.WriteLine(IlgiausiZodziai.PaimtiZodi(i));
                    }
                }
            }
        }

        /// <summary>
        /// Spasudina ilgiausius fragmentus faile
        /// </summary>
        /// <param name="IlgiausiFragmentai">Ilgiausių fragmentų konteineris</param>
        /// <param name="fv">Failas, į kurį rašys</param>
        /// <param name="ZodziaiSuSkyrikliaisPirmas">Pirmo failo žodžiai su skyrikliais</param>
        /// <param name="ZodziaiSuSkyrikliaisAntras">Antro failo žodžiai su skyrikliais</param>
        void IlgiausiuFragmentuSpausdinimasFaile(FragmentuKonteineris IlgiausiFragmentai, string fv, ZodziuKonteineris ZodziaiSuSkyrikliaisPirmas, ZodziuKonteineris ZodziaiSuSkyrikliaisAntras)
        {
            using (StreamWriter writer = new StreamWriter(fv, true, Encoding.UTF8))
            {
                if (IlgiausiFragmentai.Count == 0)
                {
                    writer.WriteLine("Duomenų failai tušti arba tekstuose nėra vienodų fragmentų");
                }
                else
                {

                    writer.WriteLine("Ilgiausi ({0} simbolių), sudaryti iš žodžių fragentai:", IlgiausioFragmentoElementuSkaicius(ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras));
                    for (int i = 0; i < IlgiausiFragmentai.Count; i++)
                    {
                        writer.WriteLine(IlgiausiFragmentai.PaimtiFragmenta(i));
                        writer.WriteLine("Šis fragmentas pirmame duomenų faile yra tokiose eilutėse:");
                        for (int g = 0; g < IlgiausiFragmentai.Numeriai[i].Count; g = g + 2)
                        {
                            int yra = 0;
                            for (int h = g + 2; h < IlgiausiFragmentai.Numeriai[i].Count; h = h + 2)
                            {
                                if (IlgiausiFragmentai.Numeriai[i].PaimtiNumeri(g) == IlgiausiFragmentai.Numeriai[i].PaimtiNumeri(h))
                                {
                                    yra = 1;
                                }
                            }
                            if (yra == 0)
                            {
                                writer.WriteLine(IlgiausiFragmentai.Numeriai[i].PaimtiNumeri(g));
                            }
                        }
                        writer.WriteLine("Šis fragmentas antrame duomenų faile yra tokiose eilutėse:");
                        for (int g = 1; g < IlgiausiFragmentai.Numeriai[i].Count; g = g + 2)
                        {
                            int yra = 0;
                            for (int h = g + 2; h < IlgiausiFragmentai.Numeriai[i].Count; h = h + 2)
                            {
                                if (IlgiausiFragmentai.Numeriai[i].PaimtiNumeri(g) == IlgiausiFragmentai.Numeriai[i].PaimtiNumeri(h))
                                {
                                    yra = 1;
                                }
                            }
                            if (yra == 0)
                            {
                                writer.WriteLine(IlgiausiFragmentai.Numeriai[i].PaimtiNumeri(g));
                            }
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Spausdina į failą pertvarkytą tekstą
        /// </summary>
        /// <param name="fv">Failas, į kurį rašys</param>
        /// <param name="ZodziaiSuSkyrikliaisPirmas">Pirmo failo žodžiai su skyrikliais</param>
        /// <param name="ZodziaiSuSkyrikliaisAntras">Antro failo žodžiai su skyrikliais</param>
        void PertvarkytoTekstoSpausdinimasFaile(string fv, ZodziuKonteineris ZodziaiSuSkyrikliaisPirmas, ZodziuKonteineris ZodziaiSuSkyrikliaisAntras)
        {
            using (StreamWriter writer = new StreamWriter(fv, false, Encoding.UTF8))
            {
                if (TekstoPertvarkymas(ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras).Length == 0)
                {
                    writer.WriteLine("Duomenų failuose nėra");
                }
                else
                {
                    writer.WriteLine(TekstoPertvarkymas(ZodziaiSuSkyrikliaisPirmas, ZodziaiSuSkyrikliaisAntras));
                }
            }
        }




    }
}

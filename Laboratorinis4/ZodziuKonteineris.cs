using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorinis4
{
    //Klasė, kuri yra skirta saugoti žodžiams
    class ZodziuKonteineris
    {
        private Zodis[] Zodziai { get; set; } //Žodžių masyvas
        public int Kiekis { get; private set; } //Elementų kiekis masyve
        
        /// <summary>
        /// Žodžių konstruktorius
        /// </summary>
        /// <param name="Dydis">Masyvo dydis</param>
        public ZodziuKonteineris(int Dydis)
        {
            Zodziai = new Zodis[Dydis];
            Kiekis = 0;
        }

        /// <summary>
        /// Prideda žodį į masyvą
        /// </summary>
        /// <param name="zodis">Žodis</param>
        public void PridetiZodi(Zodis zodis)
        {
            Zodziai[Kiekis++] = zodis;
        }

        /// <summary>
        /// Paima žodį iš masyvo
        /// </summary>
        /// <param name="indeksas">Elemento vieta masyve</param>
        /// <returns>Žodį</returns>
        public Zodis PaimtiZodi(int indeksas)
        {
            return Zodziai[indeksas];
        }
        
        /// <summary>
        /// Patikrina ar yra toks elementas masyve
        /// </summary>
        /// <param name="zodis">Žodis</param>
        /// <returns>True or false, ar yra toks elementas masyve</returns>
        public bool Contains(Zodis zodis)
        {
            return Zodziai.Contains(zodis);
        }
        
        /// <summary>
        /// Patikrina ar yra tokio pavadinimo žodis masyve
        /// </summary>
        /// <param name="Pavadinimas">Pavadinimas</param>
        /// <returns>True or false, ar yra toks pavadinimas</returns>
        public bool ArYraToksPavadinimas(string Pavadinimas)
        {
            for(int i = 0; i < Kiekis; i++)
            {
                if (Zodziai[i].Pavadinimas == Pavadinimas)
                {
                    return true;
                }
            }
            return false;
        }

    }
}

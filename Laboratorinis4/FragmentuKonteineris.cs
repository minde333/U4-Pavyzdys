using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorinis4
{
    /// <summary>
    /// Klasė skirta saugoti fragmentams, sudarytiems iš žodžių
    /// </summary>
    class FragmentuKonteineris
    {
        public const int Dydis = 1000; //Masyvo dydis
        private string[] Fragmentai { get; set; } //Fragmentų masyvas
        public EilutesNumeriuKonteineris[] Numeriai { get; set; } //Numerių konteinerių masyvas
        public int Count { get; private set; } //Fragmentų kiekis
        public int Kiekis { get; private set; } //Numerių konteinerių kiekis

        /// <summary>
        /// Fragmentų konteinerio konstruktorius
        /// </summary>
        public FragmentuKonteineris()
        {
            Fragmentai = new string[Dydis];
            Numeriai = new EilutesNumeriuKonteineris[Dydis];
            Count = 0;
            Kiekis = 0;
        }

        /// <summary>
        /// Įdeda į numerių masyvą, numerių konteinerį
        /// </summary>
        /// <param name="numeriai">Numerių konteineris</param>
        public void IdetiNumerius(EilutesNumeriuKonteineris numeriai)
        {
            Numeriai[Kiekis++] = numeriai;
        }

        /// <summary>
        /// Įdeda fragmentą į masyvą
        /// </summary>
        /// <param name="fragmentas"></param>
        public void IdetiFragmenta(string fragmentas)
        {
            Fragmentai[Count++] = fragmentas;
        }

        /// <summary>
        /// Paima fragmentą iš masyvo
        /// </summary>
        /// <param name="indeksas">Fragmento vieta masyve</param>
        /// <returns>Fragmentą</returns>
        public string PaimtiFragmenta(int indeksas)
        {
            return Fragmentai[indeksas];
        }


        /// <summary>
        /// Patikrina ar yra toks fragmentas masyve
        /// </summary>
        /// <param name="fragmentas">Fragmentas</param>
        /// <returns>True or false, ar yra toks fragmentas masyve</returns>
        public bool Contains(string fragmentas)
        {
            return Fragmentai.Contains(fragmentas);
        }
    }
}

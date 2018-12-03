using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorinis4
{
    /// <summary>
    /// Klasė, kuri yra skirta saugoti eilučių numerius
    /// </summary>
    class EilutesNumeriuKonteineris
    {
        public const int Dydis = 1000; //Masyvo dydis
        private int[] Numeriai { get; set; } //Numerių masyvas
        public int Count { get; private set; } //Kintamasis skirtas masyvo elementų kiekiui skaičiuoti

        /// <summary>
        /// Klasės konstruktorius
        /// </summary>
        public EilutesNumeriuKonteineris()
        {
            Numeriai = new int[Dydis];
            Count = 0;
        }

        /// <summary>
        /// Įdėti numerį į masyvą
        /// </summary>
        /// <param name="numeris">Numeris</param>
        public void IdetiNumeri(int numeris)
        {
            Numeriai[Count++] = numeris;
        }

        /// <summary>
        /// Paimti numerį iš masyvo
        /// </summary>
        /// <param name="indeksas">Elemento vieta masyve</param>
        /// <returns>Numerį</returns>
        public int PaimtiNumeri(int indeksas)
        {
            return Numeriai[indeksas];
        }

        /// <summary>
        /// Patikrina ar toks numeris yra masyve
        /// </summary>
        /// <param name="numeris">Numeris</param>
        /// <returns>True or false, priklausomai nuo to ar yra elementas toks masyve</returns>
        public bool Contains(int numeris)
        {
            return Numeriai.Contains(numeris);
        }

        
    }
}

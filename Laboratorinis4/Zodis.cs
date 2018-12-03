using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laboratorinis4
{
    /// <summary>
    /// Klasė, kuri yra skirta duomenims apie žodžius aprašyti
    /// </summary>
    class Zodis
    {
        public string Pavadinimas { get; set; } //Žodis
        public int Pasikartojimai { get; set; } //Jo pasikartojimų skaičius
        public int Eilute { get; set; } //Numeris eilutės, kurioje yra žodis
        
        /// <summary>
        /// Žodžio konstruktorius
        /// </summary>
        /// <param name="pavadinimas">Žodis</param>
        /// <param name="pasikartojimai">Pasikartojimų skaičius</param>
        /// <param name="eilute">Eilutės numeris</param>
        public Zodis(string pavadinimas, int pasikartojimai, int eilute)
        {
            Pavadinimas = pavadinimas;
            Pasikartojimai = pasikartojimai;
            Eilute = eilute;
        }

        /// <summary>
        /// Pakeičia ToString metodą
        /// </summary>
        /// <returns>Pakeistą ToString šabloną</returns>
        public override string ToString()
        {
            return String.Format("Žodis: {0:d}, Pasikartojimai: {1}", Pavadinimas, Pasikartojimai);
        }

    }
}

using System;
namespace oblig2_webapplikasjoner.Models
{
    public class Aksje
    {
        public int id { get; set; }
        public string navn { get; set; }
        public double low { get; set; }
        public double high { get; set; }
        public double open { get; set; }
        public double verdi { get; set; }
        public double omsetning { get; set; }
    }
}


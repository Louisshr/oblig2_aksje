using System;
using System.ComponentModel.DataAnnotations;

namespace oblig2_webapplikasjoner.Models
{
    public class Bruker
    {
        public int id { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Fornavn { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Etternavn { get; set; }
        [RegularExpression(@"^[a-zA-ZæøåÆØÅ. \-]{2,20}$")]
        public string Brukernavn { get; set; }
        [RegularExpression(@"^[0-9a-zA-ZæøåÆØÅ. \-]{6,20}$")]
        public string Passord { get; set; }
        //public byte[] Salt { get; set; }   
    }
}


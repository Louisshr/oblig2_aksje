using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using oblig2_webapplikasjoner.DAL;
using oblig2_webapplikasjoner.Models;

namespace oblig2_webapplikasjoner.AksjeController
{
    // statisk klasse kan brukes til å lagre id til brukeren når brukeren logger inn
    // når brukeren utfører et kjøp, blir id'en til brukeren hentet fra klassen istedenfor at den sendes fra 
    static class counter
    {
        public static int idc { get; set; }
    }

    [ApiController]

    [Route("api/[controller]")]
    public class AksjeController
    {
        private readonly IAksjeRepository db;

        public AksjeController(IAksjeRepository _db)
        {
            db = _db;
        }

        [HttpGet("hentAksjer")]
        public async Task<List<Aksje>> hentAksjer()
        {
            return await db.hentAksjer();
        }

        [HttpGet("hentEn{id}")]
        public async Task<Aksje> hentEn(int id)
        {
            return await db.hentEn(id);
        }

        [HttpGet("setCounter")]
        public bool setCounter()
        {
            try
            {
                counter.idc = 1;
                return true;
            }
            catch
            {
                return false;
            }
        }



        [HttpPost("kjopAksje")]
        public async Task<bool> kjopAksje(Salg innSalg)
        {
            return await db.kjopAksje(innSalg);
        }


        [HttpPost("getObj")]
        public bool getObj(Salg innSalg)
        {
            return true;

        }

        [HttpGet("getCounter")]
        public int getCounter()
        {
            return counter.idc;
        }


        [HttpGet("hentPortefolje")]
        public async Task<List<Kjop>> hentPortefolje()
        {
            return await db.hentPortefolje(counter.idc);
        }


        [HttpGet("hentSaldo")]
        public async Task<double> hentSaldo()
        {
            return await db.hentSaldo(counter.idc);
        }


        [HttpPost("selg")]
        public async Task<bool> selg(Salg innSelg)
        {
            return await db.selg(innSelg);
        }
    }
}


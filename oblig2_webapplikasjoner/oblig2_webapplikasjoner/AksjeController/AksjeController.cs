using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using oblig2_webapplikasjoner.DAL;
using oblig2_webapplikasjoner.Models;
using Serilog;

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
    public class AksjeController : ControllerBase
    {
        private readonly IAksjeRepository db;

        private ILogger<AksjeController> _log;

        private const string _loggetInn = "loggetInn";

        public AksjeController(IAksjeRepository _db, ILogger<AksjeController> log)
        {
            db = _db;
            _log = log;
        }

        [HttpGet("hentAksjer")]
        public async Task<ActionResult<List<Aksje>>> hentAksjer()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }
            return await db.hentAksjer();
        }

        [HttpGet("hentEn{id}")]
        public async Task<Aksje> hentEn(int id)
        {

            Aksje enAksje = await db.hentEn(id);
            if (enAksje == null)
            {
                _log.LogInformation("Fant ikke akjsen!");                
            }            
            return await db.hentEn(id);
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


        [HttpGet("hentPortefolje")]
        public async Task<List<Kjop>> hentPortefolje()
        {
            return await db.hentPortefolje(counter.idc);
        }


        [HttpGet("hentSaldo")]
        public async Task<ActionResult<double>> hentSaldo()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            return await db.hentSaldo(counter.idc);
        }


        [HttpPost("selg")]
        public async Task<bool> selg(Salg innSelg)
        {
            return await db.selg(innSelg);
        }


        [HttpPost("loggInn")]
        public async Task<ActionResult> LoggInn(Bruker bruker)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await db.LoggInn(bruker);
                if (!returnOK)
                {
                    _log.LogInformation("Innloggingen feilet for bruker " + bruker.Brukernavn);
                    HttpContext.Session.SetString(_loggetInn, "");
                    return Ok(false);
                }
                HttpContext.Session.SetString(_loggetInn, "LoggetInn");
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }

        [HttpGet("loggUt")]
        public void loggUt()
        {
            HttpContext.Session.SetString(_loggetInn, "");
        }        
    }
}


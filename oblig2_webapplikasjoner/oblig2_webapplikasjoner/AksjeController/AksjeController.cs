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

            List<Aksje> alleAksjer = await db.hentAksjer();

            if (alleAksjer == null)
            {
                _log.LogInformation("Kunne ikke hente aksjer");
                return NotFound("Aksjene kunne ikke hentes");
            }
            return Ok(alleAksjer);
        }

        [HttpGet("hentEn{id}")]
        public async Task<ActionResult<Aksje>> hentEn(int id)
        {
            Aksje enAksje = await db.hentEn(id);
            if (enAksje == null)
            {
                _log.LogInformation("Fant ikke akjsen!");
                return NotFound("Fant ikke aksjen!");
            }
            return Ok(enAksje);
        }


        [HttpPost("kjopAksje")]
        public async Task<ActionResult> kjopAksje(Salg innSalg)
        {
            bool returOK = await db.kjopAksje(innSalg);
            if (!returOK)
            {
                _log.LogInformation("Kjøp av aksje ble ikke gjennomført!");
                return Ok(returOK);
            }
            return Ok(returOK);            
        }


        [HttpPost("getObj")]
        public bool getObj(Salg innSalg)
        {
            return true;

        }


        [HttpGet("hentPortefolje")]
        public async Task<ActionResult<List<Kjop>>> hentPortefolje()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            List<Kjop> enPortefolje = await db.hentPortefolje(counter.idc);

            if (enPortefolje == null)
            {
                _log.LogInformation("Kunne ikke hente portefolje");
                return NotFound("Portefoljen kunne ikke hentes");
            }

            return Ok(enPortefolje);
        }


        [HttpGet("hentSaldo")]
        public async Task<ActionResult<double>> hentSaldo()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggetInn)))
            {
                return Unauthorized();
            }

            double saldoHent = await db.hentSaldo(counter.idc);

            if (saldoHent == -1)
            {
                _log.LogInformation("Fant ikke saldo!");
                return NotFound("Fant ikke saldo!");
            }
            return Ok(saldoHent);
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


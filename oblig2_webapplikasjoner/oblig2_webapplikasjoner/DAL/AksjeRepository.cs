using System;
using oblig2_webapplikasjoner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Serilog;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Logging;

namespace oblig2_webapplikasjoner.DAL
{
    public class AksjeRepository : IAksjeRepository
    {
        private readonly AksjeDB db;

        private ILogger<AksjeRepository> _log;

        public AksjeRepository(AksjeDB _db, ILogger<AksjeRepository> log)
        {
            db = _db;
            _log = log;
        }

        public async Task<List<Aksje>> hentAksjer()
        {
            try
            {
                List<Aksje> alleAksjer = await db.aksjer.ToListAsync();
                return alleAksjer;
            }
            catch
            {
                return null;
            }

        }

        public async Task<Aksje> hentEn(int id)
        {

            try
            {
                Aksje enAksje = await db.aksjer.FindAsync(id);
                return enAksje;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> kjopAksje(Salg innSalg)
        {
            try
            {
                // finner først aksjen som handles, person som kjøper aksjen, og porteføljen til personen
                // Hvis en av disse feiler (returnerer null), returner funksjonen false. Som vil si at kjøpet avbrytes

                Aksje enAksje = await db.aksjer.FindAsync(innSalg.aksjeId);
                Person enPerson = await db.personer.FindAsync(AksjeController.counter.idc);
                Portfolje enPortefolje = await db.porteFoljer.FindAsync(enPerson.id);

                if (enAksje == null || enPerson == null || enPortefolje == null)
                {
                    return false;
                }


                // beregner totalpris for handelen

                var kjopPris = enAksje.verdi * innSalg.antall;

                // sjekker om totalpris er 0
                // hvis total pris er 0, prøver kunden å handle 0 aksjer, eller totalpris er større en det typen double kan lagre

                if (kjopPris == 0)
                {
                    return false;
                }

                // sjekker om kunde har nok penger på konto

                if (enPerson.saldo < kjopPris)
                {
                    return false;
                }

                // oppretter nytt kjop

                Kjop nyttKjop = new Kjop();
                nyttKjop.person = enPerson;
                nyttKjop.aksje = enAksje;
                nyttKjop.antall = innSalg.antall;
                nyttKjop.pris = kjopPris;


                // oppretter en bool variabel, funnet
                // Denne holder styr på om aksjen som handles eksiterer i porteføljen til kunden eller ikke

                bool funnet = false;

                // går i igjennom porteføljen til kunden for å se om aksjen allerede finnes            

                foreach (Kjop i in enPortefolje.aksjer)
                {
                    if (i.aksje == enAksje)
                    {
                        i.pris += kjopPris;
                        i.antall += innSalg.antall;
                        funnet = true;
                    }
                }


                // aksjen ble ikke funnet i kundens portefølje
                if (!funnet)
                {
                    enPortefolje.aksjer.Add(nyttKjop);
                }

                enPerson.kjop.Add(nyttKjop);
                enPerson.saldo = enPerson.saldo - kjopPris;

                db.kjopt.Add(nyttKjop);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<Kjop>> hentPortefolje(int id)
        {
            try
            {
                Person enPerson = await db.personer.FindAsync(id);

                if (enPerson == null)
                {
                    return null;
                }

                Portfolje portefolje = enPerson.portfolje;
                return portefolje.aksjer;
            }
            catch
            {
                return null;
            }
        }

        public async Task<double> hentSaldo(int id)
        {
            try
            {
                Person enPerson = await db.personer.FindAsync(id);
                return enPerson == null ? -1 : enPerson.saldo;
            }
            catch
            {
                return -1;
            }
        }


        public async Task<bool> selg(Salg innSelg)
        {
            try
            {
                // sjekker først om person som gjennomfører kjøpet, og aksjen som handles, finnes

                Person enPerson = await db.personer.FindAsync(AksjeController.counter.idc);
                Aksje enAksje = await db.aksjer.FindAsync(innSelg.aksjeId);

                // hvis person eller aksje ikke ble funnet, avbrytes kjøpet
                if (enPerson == null || enAksje == null)
                {
                    return false;
                }

                List<Kjop> aksjer_til_kunde = enPerson.portfolje.aksjer;


                foreach (Kjop kjop in aksjer_til_kunde)
                {
                    if (kjop.aksje.id == enAksje.id)
                    {
                        if (kjop.antall >= innSelg.antall)
                        {
                            var salg_pris = enAksje.verdi * innSelg.antall;
                            kjop.antall = kjop.antall - innSelg.antall;
                            kjop.pris = kjop.pris - salg_pris;
                            enPerson.saldo = enPerson.saldo + salg_pris;

                            if (kjop.antall == 0)
                            {
                                aksjer_til_kunde.Remove(kjop);
                            }

                            await db.SaveChangesAsync();
                            return true;
                        }
                        else
                        {
                            // kund
                            // en har ikke så mange aksjer som det kunden forsøker å selge
                            return false;
                        }
                    }
                }

                // Kunden har ikke den aksjen han prøver å selge
                return false;
            }
            catch
            {
                return false;
            }

        }


        // nytt

        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }


        public async Task<bool> LoggInn(Bruker bruker)
        {
            try
            {
                Brukere funnetBruker = await db.brukere.FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);
                // sjekk passordet
                byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Passord);
                if (ok)
                {
                    AksjeController.counter.idc = funnetBruker.person.id;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
    }
}


using System;
using oblig2_webapplikasjoner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace oblig2_webapplikasjoner.DAL
{
    public class AksjeRepository : IAksjeRepository
    {
        private readonly AksjeDB db;

        public AksjeRepository(AksjeDB _db)
        {
            db = _db;
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
    }
}


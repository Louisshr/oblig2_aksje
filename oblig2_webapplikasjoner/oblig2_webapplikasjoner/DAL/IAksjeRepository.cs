using System;
using oblig2_webapplikasjoner.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace oblig2_webapplikasjoner.DAL
{
    public interface IAksjeRepository
    {
        Task<List<Aksje>> hentAksjer();
        Task<Aksje> hentEn(int id);
    }
}


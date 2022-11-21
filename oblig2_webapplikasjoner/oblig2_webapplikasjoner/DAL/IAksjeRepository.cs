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
        Task<bool> kjopAksje(Salg innSalg);
        Task<List<Kjop>> hentPortefolje(int id);
        Task<double> hentSaldo(int id);
        Task<bool> selg(Salg innSelg);
    }
}


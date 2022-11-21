using System;
using Microsoft.AspNetCore.Builder;
using oblig2_webapplikasjoner.Models;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace oblig2_webapplikasjoner.DAL
{
    public static class DBinit
    {
        public static async void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AksjeDB>();

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();


                // denne må fjernes

                AksjeController.counter.idc = 1;

                Aksje aapl = await HentAksjer.initialiserAksje("AAPL");
                Aksje ibm = await HentAksjer.initialiserAksje("IBM");
                Aksje tsla = await HentAksjer.initialiserAksje("TSLA");
                Aksje dnb = await HentAksjer.initialiserAksje("DNB");
                Aksje spot = await HentAksjer.initialiserAksje("SPOT");
                Aksje twtr = await HentAksjer.initialiserAksje("TWTR");
                Aksje nflx = await HentAksjer.initialiserAksje("NFLX");
                Aksje goog = await HentAksjer.initialiserAksje("GOOG");


                context.aksjer.Add(aapl);
                context.aksjer.Add(ibm);
                context.aksjer.Add(tsla);
                context.aksjer.Add(dnb);
                context.aksjer.Add(spot);
                context.aksjer.Add(twtr);
                context.aksjer.Add(nflx);
                context.aksjer.Add(goog);


                List<Kjop> liste = new List<Kjop>();


                Person nyPerson = new Person { fornavn = "Line", etternavn = "Hansen", saldo = 10000, kjop = liste };

                List<Kjop> kjopListePortefolje = new List<Kjop>();
                Portfolje nyPortfolje = new Portfolje();
                nyPortfolje.aksjer = kjopListePortefolje;

                nyPerson.portfolje = nyPortfolje;

                context.personer.Add(nyPerson);
                context.porteFoljer.Add(nyPortfolje);
                context.SaveChanges();
            }
        }
    }
}


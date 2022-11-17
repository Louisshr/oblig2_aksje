using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using oblig2_webapplikasjoner.Models;

namespace oblig2_webapplikasjoner.DAL
{

    public class Kjop
    {
        public int id { get; set; }
        virtual public Person person { get; set; }
        public int antall { get; set; }
        public double pris { get; set; }
        virtual public Aksje aksje { get; set; }
    }

    public class Person
    {
        public int id { get; set; }
        public string fornavn { get; set; }
        public string etternavn { get; set; }
        public double saldo { get; set; }
        virtual public List<Kjop> kjop { get; set; }
        virtual public Portfolje portfolje { get; set; }
    }

    public class Portfolje
    {
        public int id { get; set; }
        //virtual public Person person { get; set; }
        virtual public List<Kjop> aksjer { get; set; }
    }

    public class AksjeDB : DbContext
	{
        public AksjeDB(DbContextOptions<AksjeDB> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Aksje> aksjer { get; set; }
        public DbSet<Kjop> kjopt { get; set; }
        public DbSet<Person> personer { get; set; }
        public DbSet<Portfolje> porteFoljer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}


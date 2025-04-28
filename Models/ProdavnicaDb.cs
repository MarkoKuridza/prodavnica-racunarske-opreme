namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ProdavnicaDb : DbContext
    {
        public ProdavnicaDb()
            : base("name=ProdavnicaDb") //ProdavnicaDb
        {
        }

        public virtual DbSet<dobavljac> dobavljacs { get; set; }
        public virtual DbSet<faktura> fakturas { get; set; }
        public virtual DbSet<kategorija_proizvoda> kategorija_proizvoda { get; set; }
        public virtual DbSet<kupac> kupacs { get; set; }
        public virtual DbSet<nabavka> nabavkas { get; set; }
        public virtual DbSet<proizvod> proizvods { get; set; }
        public virtual DbSet<stavka_fakture> stavka_fakture { get; set; }
        public virtual DbSet<stavka_nabavke> stavka_nabavke { get; set; }
        public virtual DbSet<zaposleni> zaposlenis { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<dobavljac>()
                .Property(e => e.Naziv)
                .IsUnicode(false);

            modelBuilder.Entity<dobavljac>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<dobavljac>()
                .HasMany(e => e.nabavkas)
                .WithRequired(e => e.dobavljac)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<faktura>()
                .HasMany(e => e.stavka_fakture)
                .WithRequired(e => e.faktura)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<kategorija_proizvoda>()
                .Property(e => e.Naziv)
                .IsUnicode(false);

            modelBuilder.Entity<kategorija_proizvoda>()
                .Property(e => e.Opis)
                .IsUnicode(false);

            modelBuilder.Entity<kategorija_proizvoda>()
                .HasMany(e => e.proizvods)
                .WithRequired(e => e.kategorija_proizvoda)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<kupac>()
                .Property(e => e.Ime)
                .IsUnicode(false);

            modelBuilder.Entity<kupac>()
                .Property(e => e.Prezime)
                .IsUnicode(false);

            modelBuilder.Entity<kupac>()
                .Property(e => e.Telefon)
                .IsUnicode(false);

            modelBuilder.Entity<kupac>()
                .HasMany(e => e.fakturas)
                .WithRequired(e => e.kupac)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<nabavka>()
                .HasMany(e => e.stavka_nabavke)
                .WithRequired(e => e.nabavka)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proizvod>()
                .Property(e => e.Naziv)
                .IsUnicode(false);

            modelBuilder.Entity<proizvod>()
                .Property(e => e.Opis)
                .IsUnicode(false);

            modelBuilder.Entity<proizvod>()
                .HasMany(e => e.stavka_nabavke)
                .WithRequired(e => e.proizvod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<proizvod>()
                .HasMany(e => e.stavka_fakture)
                .WithRequired(e => e.proizvod)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zaposleni>()
                .Property(e => e.Ime)
                .IsUnicode(false);

            modelBuilder.Entity<zaposleni>()
                .Property(e => e.Prezime)
                .IsUnicode(false);

            modelBuilder.Entity<zaposleni>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<zaposleni>()
                .Property(e => e.Lozinka)
                .IsUnicode(false);

            modelBuilder.Entity<zaposleni>()
                .Property(e => e.Uloga)
                .IsUnicode(false);

            modelBuilder.Entity<zaposleni>()
                .HasMany(e => e.fakturas)
                .WithRequired(e => e.zaposleni)
                .HasForeignKey(e => e.IdProdavca)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<zaposleni>()
                .HasMany(e => e.nabavkas)
                .WithRequired(e => e.zaposleni)
                .HasForeignKey(e => e.IdMenadzera)
                .WillCascadeOnDelete(false);
        }
    }
}

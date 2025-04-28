namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("proizvod")]
    public partial class proizvod
    {
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public proizvod()
        {
            stavka_nabavke = new HashSet<stavka_nabavke>();
            stavka_fakture = new HashSet<stavka_fakture>();
        }

        public proizvod(int id, string naziv, int kolicina, decimal cijena)
        {
            IdProizvoda = id;
            Naziv = naziv;
            Kolicina = kolicina;
            Cijena = cijena;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProizvoda { get; set; }

        [StringLength(45)]
        public string Naziv { get; set; }

        public int Kolicina { get; set; }

        public decimal Cijena { get; set; }

        [StringLength(200)]
        public string Opis { get; set; }

        public int IdKategorije { get; set; }

        public virtual kategorija_proizvoda kategorija_proizvoda { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stavka_nabavke> stavka_nabavke { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stavka_fakture> stavka_fakture { get; set; }
    }
}

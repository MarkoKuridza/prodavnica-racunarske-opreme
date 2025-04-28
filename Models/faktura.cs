namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("faktura")]
    public partial class faktura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public faktura()
        {
            stavka_fakture = new HashSet<stavka_fakture>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrojFakture { get; set; }

        [Column(TypeName = "date")]
        public DateTime Datum_Izdavanja { get; set; }

        public decimal Ukupan_Iznos { get; set; }

        public int IdKupca { get; set; }

        public int IdProdavca { get; set; }

        public virtual zaposleni zaposleni { get; set; }

        public virtual kupac kupac { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stavka_fakture> stavka_fakture { get; set; }
    }
}

namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("kupac")]
    public partial class kupac
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public kupac()
        {
            fakturas = new HashSet<faktura>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdKupca { get; set; }

        [StringLength(45)]
        public string Ime { get; set; }

        [StringLength(45)]
        public string Prezime { get; set; }

        [StringLength(45)]
        public string Telefon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<faktura> fakturas { get; set; }
    }
}

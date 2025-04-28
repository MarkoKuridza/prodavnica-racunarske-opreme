namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("zaposleni")]
    public partial class zaposleni
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public zaposleni()
        {
            fakturas = new HashSet<faktura>();
            nabavkas = new HashSet<nabavka>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdZaposlenog { get; set; }

        [StringLength(45)]
        public string Ime { get; set; }

        [StringLength(45)]
        public string Prezime { get; set; }

        [StringLength(45)]
        public string Email { get; set; }

        [StringLength(250)]
        public string Lozinka { get; set; }

        [Column(TypeName = "enum")]
        [Required]
        [StringLength(65532)]
        public string Uloga { get; set; }

        [Column(TypeName = "date")]
        public DateTime Datum_zaposlenja { get; set; }

        public sbyte Aktivan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<faktura> fakturas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<nabavka> nabavkas { get; set; }
    }
}

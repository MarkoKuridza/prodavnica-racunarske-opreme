namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("nabavka")]
    public partial class nabavka
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public nabavka()
        {
            stavka_nabavke = new HashSet<stavka_nabavke>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdNabavke { get; set; }

        [Column(TypeName = "date")]
        public DateTime Datum_Nabavke { get; set; }

        public decimal Ukupan_Iznos { get; set; }

        public int IdMenadzera { get; set; }

        public int IdDobavljaca { get; set; }

        public virtual dobavljac dobavljac { get; set; }

        public virtual zaposleni zaposleni { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<stavka_nabavke> stavka_nabavke { get; set; }
    }
}

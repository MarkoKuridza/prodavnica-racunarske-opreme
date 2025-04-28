namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("stavka_nabavke")]
    public partial class stavka_nabavke
    {
        public int Kolicina { get; set; }

        public decimal Iznos { get; set; }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProizvoda { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdNabavke { get; set; }

        public virtual nabavka nabavka { get; set; }

        public virtual proizvod proizvod { get; set; }
    }
}

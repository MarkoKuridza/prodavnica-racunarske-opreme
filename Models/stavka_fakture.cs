namespace Projekat_A_Prodavnica_racunarske_opreme
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("stavka_fakture")]
    public partial class stavka_fakture
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BrojFakture { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdProizvoda { get; set; }

        public int Kolicina { get; set; }

        public decimal Iznos { get; set; }

        public virtual faktura faktura { get; set; }

        public virtual proizvod proizvod { get; set; }
    }
}

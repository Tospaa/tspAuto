using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("Isler")]
    public class IsModel
    {
        [Key]
        public int ID { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string IsTuru { get; set; }
        public DateTime BitisTarihi { get; set; }
        public Kullanici IlgiliKisi { get; set; }
        public string DosyaTuru { get; set; }
        public int DosyaID { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("DosyaIcra_tt")]
    public class DosyaIcra : IData_tspAuto, IDosya_tspAuto
    {
        [Key]
        public int ID { get; set; }
        public string DosyaTuru { get; set; }
        public string DosyaNo { get; set; }
        public string ArsivNo { get; set; }
        public virtual MuvekkilSahis Alacakli { get; set; }
        public virtual MuvekkilSahis AlacakliVekil { get; set; }
        public virtual MuvekkilSahis Borclu { get; set; }
        public virtual MuvekkilSahis BorcluVekil { get; set; }
        public string IcraDairesi { get; set; }
        public string Log { get; set; }
    }
}

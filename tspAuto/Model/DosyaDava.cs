using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("DosyaDava_tt")]
    public class DosyaDava : IData_tspAuto, IDosya_tspAuto
    {
        [Key]
        public int ID { get; set; }
        public string DosyaTuru { get; set; }
        public string DosyaNo { get; set; }
        public string ArsivNo { get; set; }
        public virtual MuvekkilSahis Davaci { get; set; }
        public virtual MuvekkilSahis DavaciVekil { get; set; }
        public virtual MuvekkilSahis Davali { get; set; }
        public virtual MuvekkilSahis DavaliVekil { get; set; }
        public string Durum { get; set; }
        public string DavaTuru { get; set; }
        public string Mahkeme { get; set; }
        public string DavaKonusu { get; set; }
        public string Log { get; set; }
    }
}

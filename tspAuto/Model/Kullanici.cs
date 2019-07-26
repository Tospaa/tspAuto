using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("Kullanicilar")]
    public class Kullanici : IDataModel_tspAuto
    {
        [Key]
        public int ID { get; set; }
        public string KullaniciAdi { get; set; }
        public string SifreHash { get; set; }
        public string Unvan { get; set; }
        public string IsimSoyisim { get; set; }
        public string Email { get; set; }
        public Yetkiler Yetki { get; set; }
    }

    public enum Yetkiler { Yonetici, Avukat, Stajyer, Yetkisiz }
}

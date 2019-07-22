using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("MuvekkilSirket_tt")]
    public class MuvekkilSirket : IDataModel_tspAuto
    {
        [Key]
        public int ID { get; set; }
        public string MuvekkilNo { get; set; }
        public string MuvekkilTuru { get; set; }
        public string NoterIsmi { get; set; }
        public DateTime VekaletTarihi { get; set; }
        public string VekYevmiyeNo { get; set; }
        public bool AhzuKabza { get; set; }
        public bool Feragat { get; set; }
        public bool Ibra { get; set; }
        public bool Sulh { get; set; }
        public string Banka { get; set; }
        public string Sube { get; set; }
        public string IBANno { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string SirketTuru { get; set; }
        public string SirketUnvan { get; set; }
        public string VergiDairesi { get; set; }
        public string VergiNo { get; set; }
        public string MersisNo { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("MuvekkilSirket_tt")]
    public class MuvekkilSirket : IData_tspAuto, IMuvekkil_tspAuto
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

        public string GetConcatenatedString(string[] columnsArray)
        {
            string concatenatedString = string.Empty;

            foreach (string member in columnsArray)
            {
                concatenatedString += " " + this[member];
            }

            return concatenatedString;
        }

        public string this[string property]
        {
            get
            {
                if (property == "MuvekkilNo") { return MuvekkilNo; }
                else if (property == "MuvekkilTuru") { return MuvekkilTuru; }
                else if (property == "NoterIsmi") { return NoterIsmi; }
                else if (property == "VekYevmiyeNo") { return VekYevmiyeNo; }
                else if (property == "Banka") { return Banka; }
                else if (property == "Sube") { return Sube; }
                else if (property == "IBANno") { return IBANno; }
                else if (property == "Adres") { return Adres; }
                else if (property == "Telefon") { return Telefon; }
                else if (property == "Fax") { return Fax; }
                else if (property == "Email") { return Email; }
                else if (property == "SirketTuru") { return SirketTuru; }
                else if (property == "SirketUnvan") { return SirketUnvan; }
                else if (property == "VergiDairesi") { return VergiDairesi; }
                else if (property == "VergiNo") { return VergiNo; }
                else if (property == "MersisNo") { return MersisNo; }
                else { throw new MissingMemberException("Referenced property is not an indexed member of the current object."); }
            }
        }
    }
}

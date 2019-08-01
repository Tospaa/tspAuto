using System;
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
                if (property == "DosyaTuru") { return DosyaTuru; }
                else if (property == "DosyaNo") { return DosyaNo; }
                else if (property == "ArsivNo") { return ArsivNo; }
                else if (property == "Davaci") { return Davaci.IsimSoyisim; }
                else if (property == "DavaciVekil") { return (DavaciVekil != null) ? DavaciVekil.IsimSoyisim : string.Empty; }
                else if (property == "Davali") { return Davali.IsimSoyisim; }
                else if (property == "DavaliVekil") { return (DavaliVekil != null) ? DavaliVekil.IsimSoyisim : string.Empty; }
                else if (property == "Durum") { return Durum; }
                else if (property == "DavaTuru") { return DavaTuru; }
                else if (property == "Mahkeme") { return Mahkeme; }
                else if (property == "DavaKonusu") { return DavaKonusu; }
                else { throw new MissingMemberException("Referenced property is not an indexed member of the current object."); }
            }
        }
    }
}

using System;
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
                else if (property == "Alacakli") { return Alacakli.IsimSoyisim; }
                else if (property == "AlacakliVekil") { return (AlacakliVekil != null) ? AlacakliVekil.IsimSoyisim : string.Empty; }
                else if (property == "Borclu") { return Borclu.IsimSoyisim; }
                else if (property == "BorcluVekil") { return (BorcluVekil != null) ? BorcluVekil.IsimSoyisim : string.Empty; }
                else if (property == "IcraDairesi") { return IcraDairesi; }
                else { throw new MissingMemberException("Referenced property is not an indexed member of the current object."); }
            }
        }
    }
}

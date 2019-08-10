using System;

namespace tspAuto.Model
{
    public interface IMuvekkil_tspAuto
    {
        string MuvekkilNo { get; set; }
        string MuvekkilTuru { get; set; }
        string NoterIsmi { get; set; }
        DateTime VekaletTarihi { get; set; }
        string VekYevmiyeNo { get; set; }
        bool AhzuKabza { get; set; }
        bool Feragat { get; set; }
        bool Ibra { get; set; }
        bool Sulh { get; set; }
        string Banka { get; set; }
        string Sube { get; set; }
        string IBANno { get; set; }
        string Adres { get; set; }
        string Telefon { get; set; }
        string Fax { get; set; }
        string Email { get; set; }
    }
}

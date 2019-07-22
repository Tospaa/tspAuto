using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tspAuto.Model
{
    [Table("Hatirlaticilar")]
    public class HatirlaticiModel : IDataModel_tspAuto
    {
        [Key]
        public int ID { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public DateTime Zaman { get; set; }
        public string HatirlaticiTablo { get; set; }
        public long HatirlaticiID { get; set; }
    }
}

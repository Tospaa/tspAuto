using System.Data.Entity;
using tspAuto.Domain;
using tspAuto.Model;

namespace tspAuto
{
    public class DbConnection : DbContext
    {
        public DbConnection() : base("tspAutoDb")
        {
            Database.SetInitializer(new MyDbConnectionInitializer());
        }
        public DbSet<MuvekkilSahis> MuvekkilSahis_tt { get; set; }
        public DbSet<MuvekkilSirket> MuvekkilSirket_tt { get; set; }
        public DbSet<HatirlaticiModel> Hatirlaticilar { get; set; }
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<DosyaDava> DosyaDava_tt { get; set; }
        public DbSet<DosyaIcra> DosyaIcra_tt { get; set; }
        public DbSet<IsModel> Isler { get; set; }
    }

    public class MyDbConnectionInitializer : CreateDatabaseIfNotExists<DbConnection>
    {
        protected override void Seed(DbConnection db)
        {
            db.Kullanicilar.Add(new Kullanici
            {
                KullaniciAdi = "admin",
                SifreHash = MethodPack.HashPassword("admin"),
                Unvan = "sysdev",
                IsimSoyisim = "admin",
                Yetki = Yetkiler.Yonetici
            });
            db.SaveChanges();
        }
    }
}

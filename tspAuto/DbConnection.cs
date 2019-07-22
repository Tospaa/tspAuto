using System.Data.Entity;
using tspAuto.Model;

namespace tspAuto
{
    public class DbConnection : DbContext
    {
        public DbConnection() : base("tspAutoDb")
        {
            //Database.SetInitializer(new DropCreateDatabaseAlways<DbConnection>());
        }
        public DbSet<MuvekkilSahis> MuvekkilSahis_tt { get; set; }
        public DbSet<MuvekkilSirket> MuvekkilSirket_tt { get; set; }
        public DbSet<HatirlaticiModel> Hatirlaticilar { get; set; }
    }
}

using System.Data.Entity;

namespace NLTestApp.Models
{
	class DatabaseContext : DbContext
	{
		public DatabaseContext(string connectionString) : base(connectionString)
		{
		}

		public DbSet<Martian> Martians { get; set; }
	}
}
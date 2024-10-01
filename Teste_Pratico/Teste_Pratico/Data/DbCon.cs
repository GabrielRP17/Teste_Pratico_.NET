using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using Teste_Pratico.Model;

namespace Teste_Pratico.Data
{
	public class DbCon : DbContext
	{

		public DbCon(DbContextOptions options) : base(options)
		{
		}
		public DbSet<Produto> Produtos {  get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}

	}
}

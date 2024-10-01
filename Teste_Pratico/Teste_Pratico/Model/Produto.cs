using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Teste_Pratico.Model
{
	public class Produto
	{
		public int Id { get; set; }
		public string Nome_Produto { get; set; }
		public string Descricao { get; set; }
		public decimal Preco { get; set; }
		public int Quantidade_Estoque { get; set; }
		public string Categoria { get; set; }
	}
}

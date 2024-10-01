using Teste_Pratico.Interface;
using Teste_Pratico.Model;
using Teste_Pratico.Repositorio;

namespace Teste_Pratico.Service
{
	public class ServiceProduto : IServiceProduto
	{
		private readonly IRepositorioProduto _repositorioProduto;

		public ServiceProduto(IRepositorioProduto repositorioProduto)
		{
			_repositorioProduto = repositorioProduto;
		}

		public async Task<IEnumerable<Produto>> ListarTodos()
		{
			return await _repositorioProduto.ListarTodos();
		}

		public async Task<Produto> ObterPorId(int id)
		{
			return await _repositorioProduto.ObterPorId(id);
		}

		public async Task Cadastrar(Produto produto)
		{
			// Validações de negócio, ex: preço não pode ser negativo
			if (produto.Preco < 0)
				throw new ArgumentException("O preço não pode ser negativo");

			await _repositorioProduto.Cadastrar(produto);
		}

		public async Task Atualizar(Produto produto)
		{
			await _repositorioProduto.Atualizar(produto);
		}

		public async Task Remover(int id)
		{
			await _repositorioProduto.Remover(id);
		}

		public async Task ImportarProdutosArquivo(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("Arquivo não encontrado.", filePath);
			}

			var lines = await File.ReadAllLinesAsync(filePath);
			foreach (var line in lines)
			{
				var values = line.Split(',');

				if (values.Length == 5)
				{
					var produto = new Produto
					{
						Nome_Produto = values[0],
						Descricao = values[1],
						Preco = decimal.Parse(values[2]),
						Quantidade_Estoque = int.Parse(values[3]),
						Categoria = values[4]
					};

					await _repositorioProduto.Cadastrar(produto);
				}
			}
		}
	}
}

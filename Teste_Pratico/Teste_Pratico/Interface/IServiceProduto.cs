using Teste_Pratico.Model;

namespace Teste_Pratico.Interface
{
	public interface IServiceProduto
	{
		Task<IEnumerable<Produto>> ListarTodos();
		Task<Produto> ObterPorId(int id);
		Task Cadastrar(Produto produto);
		Task Atualizar(Produto produto);
		Task Remover(int id);
		Task ImportarProdutosArquivo(string filePath);
	}
}

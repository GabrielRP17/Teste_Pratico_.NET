using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using Teste_Pratico.Data;
using Teste_Pratico.Interface;
using Teste_Pratico.Model;

namespace Teste_Pratico.Repositorio
{
	public class RepositorioProduto : IRepositorioProduto
	{
		private readonly DbCon _context;

		public RepositorioProduto(DbCon context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Produto>> ListarTodos()
		{
			return await _context.Produtos.ToListAsync();
		}

		public async Task<Produto> ObterPorId(int id)
		{
			return await _context.Produtos.FindAsync(id);
		}

		public async Task Cadastrar(Produto produto)
		{
			await _context.Produtos.AddAsync(produto);
			await _context.SaveChangesAsync();
		}

		public async Task Atualizar(Produto produto)
		{
			_context.Produtos.Update(produto);
			await _context.SaveChangesAsync();
		}

		public async Task Remover(int id)
		{
			var produto = await _context.Produtos.FindAsync(id);
			if (produto != null)
			{
				_context.Produtos.Remove(produto);
				await _context.SaveChangesAsync();
			}
		}
	}
}


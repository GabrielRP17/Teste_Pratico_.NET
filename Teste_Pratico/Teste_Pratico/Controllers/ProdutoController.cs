using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;
using Teste_Pratico.Data;
using Teste_Pratico.Interface;
using Teste_Pratico.Model;
using Teste_Pratico.Repositorio;

namespace Teste_Pratico.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProdutoController : ControllerBase
	{
		private readonly IServiceProduto _serviceProduto;
		private readonly DbCon _context;

		public ProdutoController(IServiceProduto serviceProduto, DbCon context)
		{
			_context = context;
			_serviceProduto = serviceProduto;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
		{
			var produtos = await _serviceProduto.ListarTodos();
			return Ok(produtos);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Produto>> GetProdutoByID(int id)
		{
			var produto = await _serviceProduto.ObterPorId(id);
			if (produto == null)
			{
				return NotFound();
			}
			return Ok(produto);
		}

		[HttpPost]
		public async Task<ActionResult<Produto>> PostProduto([FromBody] Produto produto)
		{
			await _serviceProduto.Cadastrar(produto);
			return CreatedAtAction(nameof(GetProdutoByID), new { id = produto.Id }, produto);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> EditarProduto(int id, [FromBody] Produto produto)
		{
			if (id != produto.Id)
			{
				return BadRequest();
			}

			await _serviceProduto.Atualizar(produto);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> ExcluirProduto(int id)
		{
			await _serviceProduto.Remover(id);
			return NoContent();
		}

		[HttpPost("upload")]
		public async Task<IActionResult> UploadCsv(IFormFile file)
		{
			//verificação de Arquivo
			if (file == null || file.Length == 0)
				return BadRequest("Nenhum arquivo foi enviado.");

			//Leitura do Arquivo
			var produtos = new List<Produto>();

			using (var reader = new StreamReader(file.OpenReadStream()))
			{
				string line;
				while ((line = await reader.ReadLineAsync()) != null)
				{
					// Regex para capturar o padrão do arquivo
					Console.WriteLine($"linha após a substituição: {line}");

					var regex = new Regex(@"^(\d{14})\s+([A-Z\s]+?)\s+([A-Z\s0-9]+?)\s+(.*?)\s+(\d+)\s+ESTOQUE(\d+)$");
					var match = regex.Match(line);

					if (match.Success)
					{
						var id = match.Groups[1].Value.Trim();
						var categoria = match.Groups[2].Value.Trim();
						var nome = match.Groups[3].Value.Trim();
						var descricao = match.Groups[4].Value.Trim();
						var precoString = match.Groups[5].Value;  
						var quantidadeString = match.Groups[6].Value; 

						if (int.TryParse(id, out int produtoId) && decimal.TryParse(precoString, out decimal preco))
						{
							preco /= 100; // Ajustando o preço

							var produto = new Produto
							{
								Id = produtoId,
								Categoria = categoria,
								Nome_Produto = nome,
								Descricao = descricao,
								Preco = preco,
								Quantidade_Estoque = int.Parse(quantidadeString)
							};

							produtos.Add(produto);
						}
						else
						{
							Console.WriteLine($"Erro ao converter ID ou Preço. ID: {id}, Preço: {precoString}");
						}
					}
					else
					{
						Console.WriteLine($"Nenhum match encontrado para a linha: {line}");
					}
				}
			}

			using (var transaction = await _context.Database.BeginTransactionAsync())
			{
				try
				{
					await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Produtos ON");
					_context.Produtos.AddRange(produtos);
					await _context.SaveChangesAsync();
					await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Produtos OFF");

					await transaction.CommitAsync();
				}
				catch (Exception ex)
				{
					await transaction.RollbackAsync();
					return StatusCode(500, $"Erro ao inserir produtos: {ex.Message}");
				}
			}
			return Ok($"{produtos.Count} produtos foram inseridos com sucesso!");
		}
	}
}



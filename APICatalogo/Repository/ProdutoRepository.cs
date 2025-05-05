using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) : base(context) { }

        //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        //{
        //    return GetAll()
        //            .OrderBy(p => p.Nome)
        //            .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
        //            .Take(produtosParameters.PageSize).ToList();
        //}

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();

            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();

            //var produtosPagedList = PagedList<Produto>.ToPagedList(produtosOrdenados, produtosParameters.PageNumber, produtosParameters.PageSize);

            var produtosPagedList = await produtosOrdenados.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);

            return produtosPagedList;
        }

        public async Task<IEnumerable<Produto>> GetProdutoPorCategoriaAsync(int id)
        {
            var produtos =  await GetAllAsync();
            return produtos.Where(c => c.CategoriaId == id);
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams)
        {
            var produtos = await GetAllAsync();

            if(produtosFiltroParams.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParams.PrecoCriterio))
            {
                if(produtosFiltroParams.PrecoCriterio.Equals("maior", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParams.Preco.Value).OrderBy(op => op.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("menor", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParams.Preco.Value).OrderBy(op => op.Preco);
                }
                else if(produtosFiltroParams.PrecoCriterio.Equals("igual", StringComparison.OrdinalIgnoreCase))
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParams.Preco.Value).OrderBy(op => op.Preco);
                }
            }

            //var produtosFiltrados = PagedList<Produto>.ToPagedList(produtos.AsQueryable(), produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParams.PageNumber, produtosFiltroParams.PageSize);

            return produtosFiltrados;
        }
    }
}

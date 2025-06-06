﻿using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repository.Interfaces
{
    public interface IProdutoRepository: IRepository<Produto>
    {
        //IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters);
        Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParams);
        Task<IEnumerable<Produto>> GetProdutoPorCategoriaAsync(int id);   
    }
}

using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repository.Interfaces
{
    public interface ICategoriaRepository: IRepository<Categoria>
    {
        Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters);
        Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltroNome categoriasParameters);
    }
}

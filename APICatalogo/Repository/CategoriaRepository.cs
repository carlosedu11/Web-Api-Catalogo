using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace APICatalogo.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context):base(context) { }

        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();

            var categoriasOrdenadas = categorias.OrderBy(c => c.CategoriaId).AsQueryable();

            //var categoriasPagedList= PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var categoriasFiltradas = await categoriasOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas;

        }

        public async Task<IPagedList<Categoria>> GetCategoriasFiltroNomeAsync(CategoriaFiltroNome categoriasParameters)
        {
            var categorias = await GetAllAsync();

            if(!string.IsNullOrEmpty(categoriasParameters.Nome))
            {
                categorias = categorias.Where(c => c.Nome.Contains(categoriasParameters.Nome));
            }

            //var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParameters.PageNumber, categoriasParameters.PageSize);

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);

            return categoriasFiltradas;
        }
    }
}

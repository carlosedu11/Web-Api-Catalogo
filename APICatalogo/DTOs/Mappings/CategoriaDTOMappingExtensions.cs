using APICatalogo.Models;

namespace APICatalogo.DTOs.Mappings
{
    public static class CategoriaDTOMappingExtensions
    {
        public static CategoriaDTO? ToCategoriaDTO(this Categoria categoria)
        {
            if (categoria is null)
                return null;

            var categoriaDTO = new CategoriaDTO()
            {
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImagemUrl = categoria.Imagem
            };

            return categoriaDTO;
        }

        public static Categoria? ToCategoria(this CategoriaDTO categoriaDTO)
        {
            if(categoriaDTO is null)
                return null;

            var categoria = new Categoria()
            {
                CategoriaId = categoriaDTO.CategoriaId,
                Nome = categoriaDTO.Nome,
                Imagem = categoriaDTO.ImagemUrl
            };

            return categoria;

        }

        public static List<CategoriaDTO>? ToListCategoriaDTO(this IEnumerable<Categoria> categorias)
        {
            if(categorias is null || !categorias.Any())
                return new List<CategoriaDTO>();

            return categorias.Select(categoria => new CategoriaDTO
            {
                CategoriaId= categoria.CategoriaId,
                Nome= categoria.Nome,
                ImagemUrl = categoria.Imagem
            }).ToList();
        }

    }
}

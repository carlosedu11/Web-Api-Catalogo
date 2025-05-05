using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using APICatalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoriasController> _logger;

    public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet("pagination")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriasParameters categoriasParameters)
    {
        var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasAsync(categoriasParameters);
        return ObterCategorias(categorias);

    }


    [HttpGet("GetCategoriasFiltroNomeAsync")]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasFiltroNomeAsync([FromQuery]CategoriaFiltroNome categoriaFiltroNome)
    {
        var categoriasFiltradas = await _unitOfWork.CategoriaRepository.GetCategoriasFiltroNomeAsync(categoriaFiltroNome);

        return ObterCategorias(categoriasFiltradas);
    }

    private ActionResult<IEnumerable<CategoriaDTO>> ObterCategorias(IPagedList<Categoria> categorias)
    {
        var metadata = new
        {
            categorias.Count,
            categorias.PageSize,
            categorias.PageCount,
            categorias.TotalItemCount,
            categorias.HasNextPage,
            categorias.HasPreviousPage,
        };

        Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));

        var categoriasDTO = categorias.ToListCategoriaDTO();

        //var categoriasDTO = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

        return Ok(categoriasDTO);
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetAsync()
    {
        var categorias =  await _unitOfWork.CategoriaRepository.GetAllAsync();

        if(categorias == null)
        {
            return NotFound("Não existe categorias...");
        }

        var categoriasDTO = CategoriaDTOMappingExtensions.ToListCategoriaDTO(categorias);

        return Ok(categoriasDTO);
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public async Task<ActionResult<CategoriaDTO>> GetAsync(int id)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id= {id} não encontrada...");
            return NotFound($"Categoria com id= {id} não encontrada...");
        }

        var categoriaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoria);

        return Ok(categoriaDTO);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDTO)
    {
        if (categoriaDTO is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = CategoriaDTOMappingExtensions.ToCategoria(categoriaDTO);

        var categoriaCriada = _unitOfWork.CategoriaRepository.Create(categoria);
        await _unitOfWork.CommitAsync();

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDTO.CategoriaId }, categoriaCriada);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDTO)
    {
        if (id != categoriaDTO.CategoriaId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest("Dados inválidos");
        }

        var categoria = CategoriaDTOMappingExtensions.ToCategoria(categoriaDTO);

        _unitOfWork.CategoriaRepository.Update(categoria);
        await _unitOfWork.CommitAsync();
        return Ok(categoria);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<CategoriaDTO>> Delete(int id)
    {
        var categoria = await _unitOfWork.CategoriaRepository.GetAsync(c => c.CategoriaId == id);

        if (categoria == null)
        {
            _logger.LogWarning($"Categoria com id={id} não encontrada...");
            return NotFound($"Categoria com id={id} não encontrada...");
        }

        var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);

        await _unitOfWork.CommitAsync();

        var categoriaDTO = CategoriaDTOMappingExtensions.ToCategoriaDTO(categoriaExcluida);


        return Ok(categoriaDTO);
    }
}
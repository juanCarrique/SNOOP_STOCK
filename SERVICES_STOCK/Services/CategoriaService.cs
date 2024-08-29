using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Services.Services
{
    public class CategoriaService
    {
        private readonly CategoriaDAO _categoriaDAO;

        public CategoriaService(CategoriaDAO categoriaDAO)
        {
            _categoriaDAO = categoriaDAO;
        }

        public async Task<IEnumerable<CategoriaDTO>> GetCategorias()
        {
            var categorias = await _categoriaDAO.GetCategorias();
            var categoriasDTO = categorias.Select(c => new CategoriaDTO
            {
                Id = c.Id,
                Nombre = c.Nombre
            }).ToList();

            return categoriasDTO;
        }

        public async Task<CategoriaDTO> GetCategoria(int id)
        {
            var categoria = await _categoriaDAO.GetCategoria(id);

            if (categoria == null)
            {
                return null;
            }

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }

        public async Task<bool> CategoriaExists(int id)
        {
            return await _categoriaDAO.CategoriaExists(id);
        }

        public async Task<bool> PutCategoria(CategoriaDTO categoriaDTO)
        {
            try
            {
                var categoria = await _categoriaDAO.GetCategoria(categoriaDTO.Id);

                if (categoria == null)
                {
                    return false;
                }

                categoria.Nombre = categoriaDTO.Nombre;

                _categoriaDAO.UpdateCategoria(categoria);

                await _categoriaDAO.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> PostCategoria(CategoriaDTO categoriaDTO)
        {            
            var categoria = new Categoria
            {
                Nombre = categoriaDTO.Nombre
            };

            _categoriaDAO.AddCategoria(categoria);

            await _categoriaDAO.SaveChangesAsync();

            return categoria.Id;
        }

        public async Task<bool> DeleteCategoria(int id)
        {
            var categoria = await _categoriaDAO.GetCategoria(id);

            if (categoria == null)
            {
                return false;
            }

            _categoriaDAO.DeleteCategoria(categoria);

            await _categoriaDAO.SaveChangesAsync();

            return true;
        }
    }
}

using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class ProductoService
    {
        private readonly ProductoDAO _productoDAO;
        private readonly CategoriaDAO _categoriaDAO;

        public ProductoService(ProductoDAO productoDAO, CategoriaDAO categoriaDAO)
        {
            _productoDAO = productoDAO;
            _categoriaDAO = categoriaDAO;
        }

        public async Task<IEnumerable<ProductoDTO>> GetProductos()
        {
            var productos = await _productoDAO.GetProductos();
            var productosDTO = productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Detalle = p.Detalle,
                Precio = p.Precio,
                CategoriaId = p.Categoria.Id,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo
            }).ToList();

            return productosDTO;
        }

        public async Task<ProductoDTO> GetProducto(int id)
        {
            var producto = await _productoDAO.GetProducto(id);

            if (producto == null)
            {
                return null; 
            }

            return new ProductoDTO
            {
                Id = producto.Id,
                Detalle = producto.Detalle,
                Precio = producto.Precio,
                CategoriaId = producto.Categoria.Id, 
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo
            };
        }

        public async Task<bool> PutProducto(ProductoDTO productoDTO)
        {
            try
            {
                var producto = await _productoDAO.GetProducto(productoDTO.Id);

                if (producto == null)
                {
                    return false;
                }

                producto.Detalle = productoDTO.Detalle;
                producto.Precio = productoDTO.Precio;
                producto.Stock = productoDTO.Stock;
                producto.StockMinimo = productoDTO.StockMinimo;
                
                var categoria = await _categoriaDAO.GetCategoria(productoDTO.CategoriaId);

                if (categoria == null)
                {
                    // Retorna 400 Bad Request si la categoría no existe
                    throw new ArgumentException("La categoría proporcionada no existe.");
                }

                producto.Categoria = categoria;

                _productoDAO.UpdateProducto(producto);

                await _productoDAO.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw; 
            }
        }

        public async Task<int> PostProducto(ProductoDTO productoDTO)
        {
            var categoria = await _categoriaDAO.GetCategoria(productoDTO.CategoriaId);

            if (categoria == null)
            {
                // Retorna 400 Bad Request si la categoría no existe
                throw new ArgumentException("La categoría proporcionada no existe.");
            }

            var producto = new Producto
            {
                Detalle = productoDTO.Detalle,
                Precio = productoDTO.Precio,
                Categoria = categoria,
                Stock = productoDTO.Stock,
                StockMinimo = productoDTO.StockMinimo
            };

            _productoDAO.AddProducto(producto);

            await _productoDAO.SaveChangesAsync();

            return producto.Id;
        }

        public async Task<bool> DeleteProducto(int id)
        {
            var producto = await _productoDAO.GetProducto(id);

            if (producto == null)
            {
                return false;
            }

            _productoDAO.DeleteProducto(producto);

            await _productoDAO.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProductoExists(int id)
        {
            return await _productoDAO.ProductoExists(id);
        }

        public async Task<bool> PatchProducto(int id, ProductoUpdateDTO productoUpdateDTO)
        {
            try
            {
                var producto = await _productoDAO.GetProducto(id);

                if (producto == null)
                {
                    return false; 
                }

                if (productoUpdateDTO.Detalle != null)
                    producto.Detalle = productoUpdateDTO.Detalle;

                if (productoUpdateDTO.Precio.HasValue)
                    producto.Precio = productoUpdateDTO.Precio.Value;

                if (productoUpdateDTO.Stock.HasValue)
                    producto.Stock = productoUpdateDTO.Stock.Value;

                if (productoUpdateDTO.StockMinimo.HasValue)
                    producto.StockMinimo = productoUpdateDTO.StockMinimo.Value;

                if (productoUpdateDTO.CategoriaId.HasValue)
                {
                    var categoria = await _categoriaDAO.GetCategoria(productoUpdateDTO.CategoriaId.Value);
                    if (categoria != null)
                    {
                        producto.Categoria = categoria;
                    }
                    else
                    {
                        throw new ArgumentException("La categoría proporcionada no existe.");
                    }
                }


                _productoDAO.UpdateProducto(producto);
                await _productoDAO.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<CategoriaDTO> GetCategoriaByProductoId(int productoId)
        {
            Producto producto = await _productoDAO.GetProducto(productoId);
            Categoria categoria = producto.Categoria;

            return new CategoriaDTO
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre
            };
        }
    }
}

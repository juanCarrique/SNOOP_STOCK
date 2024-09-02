using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class ProductoService
    {
        private readonly ProductoDAO _productoDAO;
        private readonly CategoriaDAO _categoriaDAO;
        private readonly ComposicionDAO _composicionDAO;

        public ProductoService(ProductoDAO productoDAO, CategoriaDAO categoriaDAO, ComposicionDAO composicionDAO)
        {
            _productoDAO = productoDAO;
            _categoriaDAO = categoriaDAO;
            _composicionDAO = composicionDAO;
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

        public async Task<ProductoDTO> PutProducto(ProductoDTO productoDTO)
        {
            try
            {
                var producto = await _productoDAO.GetProducto(productoDTO.Id);

                if (producto == null)
                    return null;

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
            catch (DbUpdateConcurrencyException)
            {
                throw; 
            }
        }

        public async Task<int> PostProducto(ProductoDTO productoDTO)
        {
            var categoria = await _categoriaDAO.GetCategoria(productoDTO.CategoriaId);

            if (categoria == null)
                throw new ArgumentException("La categoría proporcionada no existe.");

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

        public async Task<int> PostProductoCompuesto(ProductoCompuestoDTO productoCompuestoDTO)
        {
            var categoria = await _categoriaDAO.GetCategoria(productoCompuestoDTO.CategoriaId);

            if (categoria == null)
            {
                // Retorna 400 Bad Request si la categoría no existe
                throw new ArgumentException("La categoría proporcionada no existe.");
            }

            int stock = await CalcularStockPorComponentes(productoCompuestoDTO.Componentes);

            var producto = new Producto
            {
                Detalle = productoCompuestoDTO.Detalle,
                Precio = productoCompuestoDTO.Precio,
                Categoria = categoria,
                Stock = stock,
                StockMinimo = productoCompuestoDTO.StockMinimo
            };

            _productoDAO.AddProducto(producto);

            await _productoDAO.SaveChangesAsync();

            await AgregarComponentes(producto, productoCompuestoDTO.Componentes);

            return producto.Id;
        }

        public async Task<ProductoDTO> DeleteProducto(int id)
        {
            var producto = await _productoDAO.GetProducto(id);

            if (producto == null)
                return null;

            var productoDTO = new ProductoDTO
            {
                Id = producto.Id,
                Detalle = producto.Detalle,
                Precio = producto.Precio,
                CategoriaId = producto.Categoria.Id,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo
            };

            _productoDAO.DeleteProducto(producto);

            await _productoDAO.SaveChangesAsync();

            return productoDTO;
        }

        public async Task<bool> ProductoExists(int id)
        {
            return await _productoDAO.ProductoExists(id);
        }

        public async Task<ProductoDTO> PatchProducto(int id, ProductoUpdateDTO productoUpdateDTO)
        {
            try
            {
                var producto = await _productoDAO.GetProducto(id);

                if (producto == null)
                    return null;

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

        private async Task<int> CalcularStockPorComponentes(ICollection<ComponenteDTO> componentes)
        {
            int stock = int.MaxValue;

            foreach (var componente in componentes)
            {
                Producto producto = await _productoDAO.GetProducto(componente.ComponenteId);
                int stockComponente = (int)Math.Floor((double)producto.Stock / componente.Cantidad);
                stock = Math.Min(stockComponente,stock);
            }

            return stock;
        }

        private async Task AgregarComponentes(Producto producto, ICollection<ComponenteDTO> componentes)
        {

            foreach (var componente in componentes)
            {
                Producto componenteProducto = await _productoDAO.GetProducto(componente.ComponenteId);
                Composicion composicion = new Composicion
                {
                    Producto = producto,
                    Componente = componenteProducto,
                    Cantidad = componente.Cantidad
                };

                _composicionDAO.AddComposicion(composicion);
            }

            await _composicionDAO.SaveChangesAsync();

        }

        public async Task ActualizarStockProducto(Producto producto, int diferenciaStock)
        {
            var productosCompuestos = await _composicionDAO.GetProductosCompuestos();
            if (productosCompuestos.Any(p => p.Id == producto.Id))
            {
                var componentes = await _composicionDAO.GetComposicionesByProductoId(producto.Id);
                foreach (var componente in componentes)
                {
                    await ActualizarStockProducto(componente.Componente, componente.Cantidad * diferenciaStock);
                }
            }
            
            producto.Stock += diferenciaStock;
            _productoDAO.UpdateProducto(producto);
            await _productoDAO.SaveChangesAsync();

        }
    }
}

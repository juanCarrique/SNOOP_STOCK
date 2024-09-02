using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Services;

namespace Services.Services
{
    public class ComposicionService
    {
        private readonly ComposicionDAO _composicionDAO;
        private readonly ProductoDAO _productoDAO;

        public ComposicionService(ComposicionDAO composicionDAO, ProductoDAO productoDAO)
        {
            _composicionDAO = composicionDAO;
            _productoDAO = productoDAO;
        }

        public async Task<IEnumerable<ComposicionDTO>> GetComposiciones()
        {
            var composiciones = await _composicionDAO.GetComposiciones();
            var composicionesDTO = composiciones.Select(c => new ComposicionDTO
            {
                Id = c.Id,
                ProductoId = c.Producto.Id,
                ComponenteId = c.Componente.Id,
                Cantidad = c.Cantidad
            }).ToList();

            return composicionesDTO;
        }

        public async Task<ComposicionDTO> GetComposicion(int id)
        {
            var composicion = await _composicionDAO.GetComposicion(id);

            if (composicion == null)
            {
                return null;
            }

            return new ComposicionDTO
            {
                Id = composicion.Id,
                ProductoId = composicion.Producto.Id,
                ComponenteId = composicion.Componente.Id,
                Cantidad = composicion.Cantidad
            };
        }

        public async Task<IEnumerable<ProductoDTO>> GetComponentesByProductoId(int id)
        {
            var composiciones = await _composicionDAO.GetComposicionesByProductoId(id);
            var productos = composiciones.Select(c => c.Componente).ToList();

            var productosDTO = productos.Select(p => new ProductoDTO
            {
                Id = p.Id,
                Detalle = p.Detalle,
                Precio = p.Precio,
                CategoriaId = p.CategoriaId,
                Stock = p.Stock,
                StockMinimo = p.StockMinimo
            }).ToList();

            return productosDTO;
        }

        public async Task<ComposicionDTO> PutComposicion(ComposicionDTO composicionDTO)
        {
            try
            {
                var composicion = await _composicionDAO.GetComposicion(composicionDTO.Id);

                if (composicion == null)
                    return null;

                composicion.Cantidad = composicionDTO.Cantidad;

                var producto = await _productoDAO.GetProducto(composicionDTO.ProductoId);

                if (producto == null)
                {
                    // Retorna 400 Bad Request si la categoría no existe
                    throw new ArgumentException("El producto proporcionado no existe.");
                }

                composicion.Producto = producto;

                var componente = await _productoDAO.GetProducto(composicionDTO.ComponenteId);
                if (componente == null)
                {
                    // Retorna 400 Bad Request si la categoría no existe
                    throw new ArgumentException("El componente proporcionado no existe.");
                }
                composicion.Componente = componente;

                _composicionDAO.UpdateComposicion(composicion);

                await _composicionDAO.SaveChangesAsync();

                return new ComposicionDTO
                {
                    Id = composicionDTO.Id,
                    ProductoId = composicionDTO.ProductoId,
                    ComponenteId = composicionDTO.ComponenteId,
                    Cantidad = composicionDTO.Cantidad
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> PostComposicion(ComposicionDTO composicionDTO)
        {
            var producto = await _productoDAO.GetProducto(composicionDTO.ProductoId);

            if (producto == null)
            {
                throw new ArgumentException("El producto proporcionado no existe.");
            }

            var componente = await _productoDAO.GetProducto(composicionDTO.ComponenteId);
            if (componente == null)
            {
                throw new ArgumentException("El componente proporcionado no existe.");
            }

            var composicion = new Composicion
            {
                Producto = producto,
                Componente = componente,
                Cantidad = composicionDTO.Cantidad
            };

            _composicionDAO.AddComposicion(composicion);

            await _composicionDAO.SaveChangesAsync();

            return composicion.Id;
        }

        public async Task<ComposicionDTO> DeleteComposicion(int id)
        {
            var composicion = await _composicionDAO.GetComposicion(id);

            if (composicion == null)
                return null;

            var composicionDTO = new ComposicionDTO
            {
                Id = composicion.Id,
                ProductoId = composicion.Producto.Id,
                ComponenteId = composicion.Componente.Id,
                Cantidad = composicion.Cantidad
            };

            _composicionDAO.DeleteComposicion(composicion);
            await _composicionDAO.SaveChangesAsync();

            return composicionDTO;
        }

        public async Task<bool> ComposicionExists(int id)
        {
            return await _composicionDAO.ComposicionExists(id);
        }

        public async Task<ComposicionDTO> PatchComposicion(int id, ComposicionUpdateDTO composicionUpdateDTO)
        {
            try
            {
                var composicion = await _composicionDAO.GetComposicion(id);

                if (composicion == null)
                    return null;


                if (composicionUpdateDTO.Cantidad != null)
                    composicion.Cantidad = composicionUpdateDTO.Cantidad.Value;

                if (composicionUpdateDTO.ProductoId.HasValue)
                {
                    var producto = await _productoDAO.GetProducto(composicionUpdateDTO.ProductoId.Value);
                    if (producto != null)
                    {
                        composicion.Producto = producto;
                    }
                    else
                    {
                        throw new ArgumentException("El producto proporcionado no existe.");
                    }
                }

                if (composicionUpdateDTO.ComponenteId.HasValue)
                {
                    var componente = await _productoDAO.GetProducto(composicionUpdateDTO.ComponenteId.Value);
                    if (componente != null)
                    {
                        composicion.Componente = componente;
                    }
                    else
                    {
                        throw new ArgumentException("El componente proporcionado no existe.");
                    }
                }


                _composicionDAO.UpdateComposicion(composicion);
                await _composicionDAO.SaveChangesAsync();

                return new ComposicionDTO
                {
                    Id = composicion.Id,
                    ProductoId = composicion.Producto.Id,
                    ComponenteId = composicion.Componente.Id,
                    Cantidad = composicion.Cantidad
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }
    }
}

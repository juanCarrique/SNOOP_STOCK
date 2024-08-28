using DataAccess;
using Domain.Models;

namespace Services.Services
{
    public class ReposicionService
    {
        private readonly ReposicionDAO _reposicionDAO;
        private readonly ProductoDAO _productoDAO;
        private readonly ProveedorDAO _proveedorDAO;

        public ReposicionService(ReposicionDAO reposicionDAO, ProductoDAO productoDAO, ProveedorDAO proveedorDAO)
        {
            _reposicionDAO = reposicionDAO;
            _productoDAO = productoDAO;
            _proveedorDAO = proveedorDAO;
        }

        public async Task<IEnumerable<ReposicionDTO>> GetReposiciones()
        {
            var reposiciones = await _reposicionDAO.GetReposiciones();
            var reposicionesDTO = reposiciones.Select(r => new ReposicionDTO
            {
                Id = r.Id,
                ProductoId = r.Producto.Id,
                ProveedorId = r.Proveedor.Id,
                Cantidad = r.Cantidad
            }).ToList();

            return reposicionesDTO;
        }

        public async Task<ReposicionDTO> GetReposicion(int id)
        {
            var reposicion = await _reposicionDAO.GetReposicion(id);

            if (reposicion == null)
            {
                return null;
            }

            return new ReposicionDTO
            {
                Id = reposicion.Id,
                ProductoId = reposicion.Producto.Id,
                ProveedorId = reposicion.Proveedor.Id,
                Cantidad = reposicion.Cantidad
            };
        }

        public async Task<ProductoDTO> GetProductoByReposicionId(int id)
        {
            Reposicion reposicion = await _reposicionDAO.GetReposicion(id);
            Producto producto = reposicion.Producto;


            if (producto == null)
            {
                throw new ArgumentException("Producto not found for the given Reposicion ID.");
    }

            return new ProductoDTO
            {
                Id = producto.Id,
                Detalle = producto.Detalle,
                Precio = producto.Precio,
                CategoriaId = producto.CategoriaId,
                Stock = producto.Stock,
                StockMinimo = producto.StockMinimo
            };
        }

        public async Task<ProveedorDTO> GetProveedorByReposicionId(int id)
        {
            Reposicion reposicion = await _reposicionDAO.GetReposicion(id);
            Proveedor proveedor = reposicion.Proveedor;

            return new ProveedorDTO
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Telefono = proveedor.Telefono,
                Mail = proveedor.Mail
            };
        }

        public async Task<bool> PutReposicion(ReposicionDTO reposicionDTO)
        {
            try
            {
                var reposicion = await _reposicionDAO.GetReposicion(reposicionDTO.Id);

                if (reposicion == null)
                {
                    return false;
                }

                if(!await _productoDAO.ProductoExists(reposicionDTO.ProductoId))
                {
                    throw new ArgumentException("Producto no encontrado");
                }

                if (!await _proveedorDAO.ProveedorExists(reposicionDTO.ProveedorId))
                {
                    throw new ArgumentException("Proveedor no encontrado");
                }
                reposicion.Proveedor = await _proveedorDAO.GetProveedor(reposicionDTO.ProveedorId);

                ReposicionUpdateDTO reposicionUpdateDTO = new ReposicionUpdateDTO
                {
                    ProductoId = reposicionDTO.ProductoId,
                    ProveedorId = reposicionDTO.ProveedorId,
                    Cantidad = reposicionDTO.Cantidad
                };

                reposicion = await ActualizarReposicion(reposicion, reposicionUpdateDTO);

                _reposicionDAO.UpdateReposicion(reposicion);
                await _reposicionDAO.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> PostReposicion(ReposicionDTO reposicionDTO)
        {
            var producto = await _productoDAO.GetProducto(reposicionDTO.ProductoId);
            var proveedor = await _proveedorDAO.GetProveedor(reposicionDTO.ProveedorId);

            if (producto == null)
            {
                throw new ArgumentException("Producto no encontrado");
            }

            if (proveedor == null)
            {
                throw new ArgumentException("Proveedor no encontrado");
            }

            //Actualizo el stock del producto
            producto.Stock += reposicionDTO.Cantidad;

            var reposicion = new Reposicion
            {
                Producto = producto,
                Proveedor = proveedor,
                Cantidad = reposicionDTO.Cantidad
            };

            _reposicionDAO.AddReposicion(reposicion);
            _productoDAO.UpdateProducto(producto);

            await _reposicionDAO.SaveChangesAsync();

            return reposicion.Id;
        }

        public async Task<bool> DeleteReposicion(int id)
        {
            var reposicion = await _reposicionDAO.GetReposicion(id);

            if (reposicion == null)
            {
                return false;
            }

            // Actualizo el stock del producto
            var producto = await _productoDAO.GetProducto(reposicion.ProductoId);
            producto.Stock -= reposicion.Cantidad;

            _reposicionDAO.DeleteReposicion(reposicion);
            _productoDAO.UpdateProducto(producto);

            await _reposicionDAO.SaveChangesAsync();

            return true;
        }

        public async Task<bool> PatchReposicion(int id, ReposicionUpdateDTO reposicionUpdateDTO)
        {
            try
            {
                var reposicion = await _reposicionDAO.GetReposicion(id);


                if (reposicion == null)
                {
                    return false;
                }               

                if (reposicionUpdateDTO.ProveedorId.HasValue) 
                {
                    if (!await _proveedorDAO.ProveedorExists(reposicionUpdateDTO.ProveedorId.Value))
                        throw new ArgumentException("Proveedor no encontrado");


                    reposicion.Proveedor = await _proveedorDAO.GetProveedor(reposicionUpdateDTO.ProveedorId.Value);
                }


                if (reposicionUpdateDTO.ProductoId.HasValue)
                {
                    if (!await _productoDAO.ProductoExists(reposicionUpdateDTO.ProductoId.Value))
                        throw new ArgumentException("Producto no encontrado");
                }

                if (reposicionUpdateDTO.ProductoId.HasValue || reposicionUpdateDTO.Cantidad.HasValue)
                    reposicion = await ActualizarReposicion(reposicion, reposicionUpdateDTO);


                _reposicionDAO.UpdateReposicion(reposicion);
                await _reposicionDAO.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw new ArgumentException("errorazo");
            }
        }

        private async Task<Reposicion> ActualizarReposicion(Reposicion reposicion, ReposicionUpdateDTO reposicionUpdateDTO)
        {
            // TODO - Pulir lógica de actualización de stock
            var productoAnterior = await _productoDAO.GetProducto(reposicion.ProductoId);

            Reposicion reposicionNueva = reposicion;


            if (reposicionUpdateDTO.ProductoId.HasValue && reposicion.ProductoId != reposicionUpdateDTO.ProductoId.Value )
            {
                var productoNuevo = await _productoDAO.GetProducto(reposicionUpdateDTO.ProductoId.Value);

                // Corrijo stock del producto anterior
                productoAnterior.Stock -= reposicion.Cantidad;

                // Actualizo stock del producto nuevo
                if (reposicionUpdateDTO.Cantidad.HasValue)
                    reposicionNueva.Cantidad = reposicionUpdateDTO.Cantidad.Value;

               productoNuevo.Stock += reposicionNueva.Cantidad;

                // Actualizo reposicion
                reposicionNueva.Producto = productoNuevo;
                _productoDAO.UpdateProducto(productoNuevo);

            }
            else if (reposicion.Cantidad != reposicionUpdateDTO.Cantidad.Value)
            {
                var diferencia = reposicionUpdateDTO.Cantidad.Value - reposicion.Cantidad;

                // Actualizo stock del producto
                productoAnterior.Stock += diferencia;

                // Actualizo reposicion
                reposicionNueva.Cantidad = reposicionUpdateDTO.Cantidad.Value;

            }

            _productoDAO.UpdateProducto(productoAnterior);
            await _reposicionDAO.SaveChangesAsync();

            return reposicionNueva;

        }

            public async Task<bool> ReposicionExists(int id)
        {
            return await _reposicionDAO.ReposicionExists(id);
        }

    }
}

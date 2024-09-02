using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Services.Services
{
    public class FacturaService
    {
        private readonly FacturaDAO _facturaDAO;
        private readonly ItemFacturaDAO _itemFacturaDAO;
        private readonly ProductoDAO _productoDAO;
        private readonly ClienteDAO _clienteDAO;
        private readonly UsuarioDAO _usuarioDAO;
        private readonly ProductoService _productoService;

        public FacturaService(FacturaDAO facturaDAO, 
            ItemFacturaDAO itemFacturaDAO, 
            ProductoDAO productoDAO, 
            ClienteDAO clienteDAO, 
            UsuarioDAO usuarioDAO,
            ProductoService productoService)
        {
            _facturaDAO = facturaDAO;
            _itemFacturaDAO = itemFacturaDAO;
            _productoDAO = productoDAO;
            _clienteDAO = clienteDAO;
            _usuarioDAO = usuarioDAO;
            _productoService = productoService;
        }

        public async Task<IEnumerable<FacturaDTO>> GetFacturas()
        {
            var facturas = await _facturaDAO.GetFacturas();
            var facturasDTO = facturas.Select(f => new FacturaDTO
            {
                Id = f.Id,
                Fecha = f.Fecha,
                ClienteId = f.Cliente?.Id ?? null,
                ItemsFactura = f.ItemsFactura.Select(i => new ItemFacturaDTO
                {
                    Id = i.Id,
                    Cantidad = i.Cantidad,
                    ProductoId = i.ProductoId
                }).ToList()
            }).ToList();

            return facturasDTO;
        }

        public async Task<FacturaDTO> GetFactura(int id)
        {
            var factura = await _facturaDAO.GetFactura(id);

            if (factura == null)
                return null;

            return new FacturaDTO
            {
                Id = factura.Id,
                Fecha = factura.Fecha,
                ClienteId = factura.Cliente?.Id ?? null,
                ItemsFactura = factura.ItemsFactura.Select(i => new ItemFacturaDTO
                {
                    Id = i.Id,
                    Cantidad = i.Cantidad,
                    ProductoId = i.Producto.Id
                }).ToList()
            };
        }

        public async Task<ICollection<ItemFacturaDTO>> GetFacturaItems(int id)
        {
            var facturaItems = await _itemFacturaDAO.GetItemsFacturaByFacturaId(id);
            var itemsFacturaDTO = facturaItems.Select(i => new ItemFacturaDTO
            {
                Id = i.Id,
                Cantidad = i.Cantidad,
                ProductoId = i.Producto.Id
            }).ToList();

            return itemsFacturaDTO;
        }

        public async Task<FacturaDTO> PutFactura(FacturaDTO facturaDTO)
        {
            try
            {
                var factura = await _facturaDAO.GetFactura(facturaDTO.Id);

                if (factura == null)
                    return null;

                var cliente = await _clienteDAO.GetCliente(facturaDTO.ClienteId.Value);
                if (cliente == null)
                    throw new ArgumentException($"Cliente con id:{facturaDTO.ClienteId} no encontrado.");

                var vendedor = await _usuarioDAO.GetUsuario(facturaDTO.VendedorId);
                if (vendedor == null)
                    throw new ArgumentException($"Vendedor con id:{facturaDTO.VendedorId} no encontrado.");


                //Primero devuelvo el stock de los productos viejos de la factura
                await ActualizarStockFactura(factura, false);

                var itemsFactura = await ReemplazarItemsFactura(factura, facturaDTO.ItemsFactura);

                factura.Fecha = facturaDTO.Fecha ?? factura.Fecha;
                factura.Total = itemsFactura.Sum(i => i.Precio * i.Cantidad);
                factura.Numero = facturaDTO.Numero;
                factura.Tipo = facturaDTO.Tipo;
                factura.Cliente = cliente;
                factura.Vendedor = vendedor;
                factura.ItemsFactura = itemsFactura;

                //Actualizo el stock de los nuevos productos de la factura
                await ActualizarStockFactura(factura, true);

                _facturaDAO.UpdateFactura(factura);
                await _facturaDAO.SaveChangesAsync();

                return facturaDTO;
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> PostFactura(FacturaDTO facturaDTO)
        {
            //Creo la factura
            var factura = new Factura
            {
                Fecha = facturaDTO.Fecha ?? DateTime.Now,
                Numero = facturaDTO.Numero, //TODO: Generar un numero de factura
                Tipo = facturaDTO.Tipo,
                Cliente = await _clienteDAO.GetCliente(facturaDTO.ClienteId.Value),
                Vendedor = await _usuarioDAO.GetUsuario(facturaDTO.VendedorId)
            };

            // Creo los itemsFactura
            var itemsFactura = await CrearItemsFactura(facturaDTO.ItemsFactura, factura);

            factura.ItemsFactura = itemsFactura;
            factura.Total = itemsFactura.Sum(i => i.Precio * i.Cantidad);

            //Actualizo el stock de los productos
            await ActualizarStockFactura(factura, true);

            _facturaDAO.AddFactura(factura);
            await _facturaDAO.SaveChangesAsync();
                

            return factura.Id;
        }

        public async Task<FacturaDTO> PatchFactura(int id, FacturaUpdateDTO facturaUpdateDTO)
        {
            var factura = await _facturaDAO.GetFactura(id);

            if (factura == null)
                return null;

            if (facturaUpdateDTO.Fecha.HasValue)
                factura.Fecha = facturaUpdateDTO.Fecha.Value;


            if (facturaUpdateDTO.Numero.HasValue)
                factura.Numero = facturaUpdateDTO.Numero.Value;
            

            if (facturaUpdateDTO.Tipo != null)
                factura.Tipo = facturaUpdateDTO.Tipo;
            
            if (facturaUpdateDTO.ClienteId.HasValue)
            {
                var cliente = await _clienteDAO.GetCliente(facturaUpdateDTO.ClienteId.Value);
                if (cliente == null)
                    throw new ArgumentException($"Cliente con id:{facturaUpdateDTO.ClienteId} no encontrado.");
            }

            if (facturaUpdateDTO.VendedorId.HasValue)
            {
                var vendedor = await _usuarioDAO.GetUsuario(facturaUpdateDTO.VendedorId.Value);
                if (vendedor == null)
                    throw new ArgumentException($"Vendedor con id:{facturaUpdateDTO.VendedorId} no encontrado.");
            }

            if (facturaUpdateDTO.ItemsFactura != null)
            {
                await ActualizarStockFactura(factura, false);

                var itemsFactura = await ReemplazarItemsFactura(factura, facturaUpdateDTO.ItemsFactura);

                factura.ItemsFactura = itemsFactura;
                factura.Total = itemsFactura.Sum(i => i.Precio * i.Cantidad);

                await ActualizarStockFactura(factura, true);
            }

            _facturaDAO.UpdateFactura(factura);
            await _facturaDAO.SaveChangesAsync();

            return new FacturaDTO
            {
                Id = factura.Id,
                Fecha = factura.Fecha,
                ClienteId = factura.Cliente?.Id ?? null,
                ItemsFactura = factura.ItemsFactura.Select(i => new ItemFacturaDTO
                {
                    Id = i.Id,
                    Cantidad = i.Cantidad,
                    ProductoId = i.Producto.Id
                }).ToList()
            };
        }

        public async Task<FacturaDTO> DeleteFactura(int id)
        {
            var factura = await _facturaDAO.GetFactura(id);

            if (factura == null)
                return null;

            await ActualizarStockFactura(factura, false);

            var facturaDTO = new FacturaDTO
            {
                Id = factura.Id,
                Fecha = factura.Fecha,
                ClienteId = factura.Cliente?.Id ?? null,
                ItemsFactura = factura.ItemsFactura.Select(i => new ItemFacturaDTO
                {
                    Id = i.Id,
                    Cantidad = i.Cantidad,
                    ProductoId = i.Producto.Id
                }).ToList()
            };

            _facturaDAO.DeleteFactura(factura);
            await _facturaDAO.SaveChangesAsync();

            return facturaDTO;
        }

        public async Task<bool> FacturaExists(int id)
        {
            return await _facturaDAO.FacturaExists(id);
        }

        private async Task<ICollection<ItemFactura>> CrearItemsFactura(ICollection<ItemFacturaDTO> itemsFacturaDTO, Factura factura)
        {
            var itemsFactura = new List<ItemFactura>();

            foreach (ItemFacturaDTO itemDTO in itemsFacturaDTO)
            {
                var producto = await _productoDAO.GetProducto(itemDTO.ProductoId);

                if (producto == null)
                    throw new ArgumentException($"Producto con id:{itemDTO.ProductoId} no encontrado.");

                var itemFactura = new ItemFactura
                {
                    Id = itemDTO.Id,
                    Cantidad = itemDTO.Cantidad,
                    Producto = producto,
                    Factura = factura,
                    Precio = itemDTO.Precio ?? producto.Precio
                };


                itemsFactura.Add(itemFactura);
                _itemFacturaDAO.AddItemFactura(itemFactura);
            }

            await _itemFacturaDAO.SaveChangesAsync();

            return itemsFactura;
        }

        private async Task<ICollection<ItemFactura>> ReemplazarItemsFactura(Factura factura, ICollection<ItemFacturaDTO> itemsFacturaDTO)
        {
            var itemsFactura = new List<ItemFactura>();

            var itemsActuales = await _itemFacturaDAO.GetItemsFacturaByFacturaId(factura.Id);

            foreach (ItemFactura item in itemsActuales)
                _itemFacturaDAO.DeleteItemFactura(item);

            foreach (ItemFacturaDTO itemDTO in itemsFacturaDTO)
            {
                var producto = await _productoDAO.GetProducto(itemDTO.ProductoId);

                if (producto == null)
                    throw new ArgumentException($"Producto con id:{itemDTO.ProductoId} no encontrado.");

                var itemFacturaDTO = new ItemFactura
                {
                    Id = itemDTO.Id,
                    Cantidad = itemDTO.Cantidad,
                    Producto = producto,
                    Factura = factura,
                    Precio = itemDTO.Precio ?? producto.Precio
                };

                itemsFactura.Add(itemFacturaDTO);
                _itemFacturaDAO.AddItemFactura(itemFacturaDTO);
            }

            await _itemFacturaDAO.SaveChangesAsync();

            return itemsFactura;
        }

        private async Task ActualizarStockFactura(Factura factura, bool bajoStock)
        {
            var diferenciaStock = bajoStock ? -1 : 1;

            foreach (ItemFactura item in factura.ItemsFactura)
            {
                Producto producto = await _productoDAO.GetProducto(item.Producto.Id);
                await _productoService.ActualizarStockProducto(producto, item.Cantidad * diferenciaStock);
            }
        }
    }
}

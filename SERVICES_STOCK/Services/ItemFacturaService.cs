using DataAccess;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ItemFacturaService
    {
        private readonly ItemFacturaDAO _itemFacturaDAO;
        private readonly ProductoDAO _productoDAO;
        private readonly FacturaDAO _facturaDAO;

        public ItemFacturaService(ItemFacturaDAO itemFacturaDAO, ProductoDAO productoDAO, FacturaDAO facturaDAO)
        {
            _itemFacturaDAO = itemFacturaDAO;
            _productoDAO = productoDAO;
            _facturaDAO = facturaDAO;
        }

        public async Task<IEnumerable<ItemFacturaDTO>> GetItemsFactura()
        {
            var itemsFactura = await _itemFacturaDAO.GetItemsFactura();
            return itemsFactura.Select(i => new ItemFacturaDTO
            {
                Id = i.Id,
                Cantidad = i.Cantidad,
                Precio = i.Precio,
                ProductoId = i.ProductoId,
                FacturaId = i.FacturaId
            });
        }

        public async Task<ItemFacturaDTO> GetItemFactura(int id)
        {
            var itemFactura = await _itemFacturaDAO.GetItemFactura(id);

            if (itemFactura == null)
                return null;

            return new ItemFacturaDTO
            {
                Id = itemFactura.Id,
                Cantidad = itemFactura.Cantidad,
                Precio = itemFactura.Precio,
                ProductoId = itemFactura.ProductoId,
                FacturaId = itemFactura.FacturaId
            };
        }

        public async Task<IEnumerable<ItemFacturaDTO>> GetItemsFacturaByFacturaId(int facturaID)
        {
            var itemsFactura = await _itemFacturaDAO.GetItemsFacturaByFacturaId(facturaID);

            return itemsFactura.Select(i => new ItemFacturaDTO
            {
                Id = i.Id,
                Cantidad = i.Cantidad,
                Precio = i.Precio,
                ProductoId = i.ProductoId,
                FacturaId = i.FacturaId
            });
        }


        public async Task<ItemFacturaDTO> PutItemFactura(ItemFacturaDTO itemFacturaDTO)
        {
            try
            {
                var itemFactura = await _itemFacturaDAO.GetItemFactura(itemFacturaDTO.Id);

                if (itemFactura == null)
                    return null;


                var factura = await _facturaDAO.GetFactura(itemFacturaDTO.FacturaId);
                var producto = await _productoDAO.GetProducto(itemFacturaDTO.ProductoId);

                if (factura == null)
                    throw new ArgumentException("Factura no encontrada.");

                if(producto == null)
                    throw new ArgumentException("Producto no encontrado.");

                await ActualizarStock(itemFactura, itemFacturaDTO);

                itemFactura.Factura = factura;
                itemFactura.Producto = producto;
                itemFactura.Cantidad = itemFacturaDTO.Cantidad;
                itemFactura.Precio = itemFacturaDTO.Precio ?? itemFactura.Precio;
                itemFactura.Producto = producto;


                return new ItemFacturaDTO
                {
                    Id = itemFactura.Id,
                    Cantidad = itemFactura.Cantidad,
                    Precio = itemFactura.Precio,
                    ProductoId = itemFactura.ProductoId,
                    FacturaId = itemFactura.FacturaId
                };
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task<int> PostItemFactura(ItemFacturaDTO itemFacturaDTO)
        {
            var producto = await _productoDAO.GetProducto(itemFacturaDTO.ProductoId);
            var factura = await _facturaDAO.GetFactura(itemFacturaDTO.FacturaId);

            if (producto == null)
                throw new ArgumentException("Producto no encontrado.");

            if (factura == null)
                throw new ArgumentException("Factura no encontrada.");

            var itemFactura = new ItemFactura
            {
                Cantidad = itemFacturaDTO.Cantidad,
                Precio = itemFacturaDTO.Precio ?? producto.Precio,
                Producto = producto,
                Factura = factura
            };

            await ActualizarStock(itemFactura, null);

            _itemFacturaDAO.AddItemFactura(itemFactura);
            await _itemFacturaDAO.SaveChangesAsync();

            return itemFactura.Id;
        }

        public async Task<ItemFacturaDTO> DeleteItemFactura(int id)
        {
            var itemFactura = await _itemFacturaDAO.GetItemFactura(id);

            if (itemFactura == null)
                return null;

            var itemFacturaDTO = new ItemFacturaDTO
            {
                Id = itemFactura.Id,
                Cantidad = itemFactura.Cantidad,
                Precio = itemFactura.Precio,
                ProductoId = itemFactura.ProductoId,
                FacturaId = itemFactura.FacturaId
            };

            var producto = itemFactura.Producto;
            producto.Stock += itemFactura.Cantidad;

            _productoDAO.UpdateProducto(producto);
            _itemFacturaDAO.DeleteItemFactura(itemFactura);
            await _itemFacturaDAO.SaveChangesAsync();

            return itemFacturaDTO;
        }

        private async Task<ItemFacturaDTO> PatchItemFactura(int id,ItemFacturaUpdateDTO itemFacturaUpdateDTO)
        {
            var itemFactura = await _itemFacturaDAO.GetItemFactura(id);

            if (itemFactura == null)
                return null;

            if (itemFacturaUpdateDTO.FacturaId.HasValue)
            {
                var factura = await _facturaDAO.GetFactura(itemFacturaUpdateDTO.FacturaId ?? itemFactura.FacturaId);
                if (factura == null)
                    throw new ArgumentException("Factura no encontrada.");
                itemFactura.Factura = factura;
            }

            if (itemFacturaUpdateDTO.ProductoId.HasValue)
            {
                var producto = await _productoDAO.GetProducto(itemFacturaUpdateDTO.ProductoId ?? itemFactura.ProductoId);
                if (producto == null)
                    throw new ArgumentException("Producto no encontrado.");
                itemFactura.Producto = producto;
            }

            if (itemFacturaUpdateDTO.Cantidad.HasValue)
                itemFactura.Cantidad = itemFacturaUpdateDTO.Cantidad ?? itemFactura.Cantidad;

            if (itemFacturaUpdateDTO.Precio.HasValue)
                itemFactura.Precio = itemFacturaUpdateDTO.Precio ?? itemFactura.Precio;

            var itemFacturaDTO = new ItemFacturaDTO
            {
                Id = itemFactura.Id,
                Cantidad = itemFactura.Cantidad,
                Precio = itemFactura.Precio,
                ProductoId = itemFactura.ProductoId,
                FacturaId = itemFactura.FacturaId
            };

            await ActualizarStock(itemFactura, itemFacturaDTO);

            _itemFacturaDAO.UpdateItemFactura(itemFactura);
            await _itemFacturaDAO.SaveChangesAsync();

            return itemFacturaDTO;
        }

        private async Task ActualizarStock(ItemFactura itemFactura, ItemFacturaDTO itemFacturaDTO)
        {
            
            if (itemFacturaDTO != null)
            {
                var productoActual = itemFactura.Producto;


                if (productoActual.Id != itemFacturaDTO.ProductoId)
                {
                    var productoNuevo = await _productoDAO.GetProducto(itemFacturaDTO.ProductoId);

                    productoActual.Stock += itemFactura.Cantidad;

                    productoNuevo.Stock -= itemFacturaDTO.Cantidad;

                    _productoDAO.UpdateProducto(productoActual);
                    _productoDAO.UpdateProducto(productoNuevo);

                }
                else
                {
                    productoActual.Stock += itemFactura.Cantidad - itemFacturaDTO.Cantidad;
                    _productoDAO.UpdateProducto(productoActual);
                }
            }
            else
            {
                Producto producto = itemFactura.Producto;
                producto.Stock -= itemFactura.Cantidad;
                _productoDAO.UpdateProducto(producto);
            }

            await _productoDAO.SaveChangesAsync();
        }

        public async Task<bool> ItemFacturaExists(int id)
        {
            return await _itemFacturaDAO.ItemFacturaExists(id);
        }
    }
}

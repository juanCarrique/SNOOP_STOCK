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

        //TODO: Implementar POST, PATCH, DELETE ...

        private async Task ActualizarStock(ItemFactura itemFactura, ItemFacturaDTO itemFacturaDTO)
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


            await _productoDAO.SaveChangesAsync();
        }

        public async Task<bool> ItemFacturaExists(int id)
        {
            return await _itemFacturaDAO.ItemFacturaExists(id);
        }
    }
}

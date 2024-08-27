namespace Services
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        public string Detalle { get; set; } = null!;

        public decimal Precio { get; set; }

        public int CategoriaId { get; set; }

        public int Stock { get; set; }

        public int? StockMinimo { get; set; }
    }
}

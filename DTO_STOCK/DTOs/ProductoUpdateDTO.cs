namespace DTO_STOCK
{
    public class ProductoUpdateDTO
    {

        public string? Detalle { get; set; } = null!;

        public decimal? Precio { get; set; }

        public int? CategoriaId { get; set; }

        public int? Stock { get; set; }

        public int? StockMinimo { get; set; }
    }
}

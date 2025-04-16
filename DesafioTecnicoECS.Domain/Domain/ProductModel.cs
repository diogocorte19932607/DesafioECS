namespace DesafioTecnicoECS.Domain.Domain
{
    public class ProductModel : BaseDomain
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}

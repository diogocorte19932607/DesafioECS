namespace DesafioTecnicoECS.Domain.Domain
{
    public class ProductCreateModel
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

    }
    public class ProductEditModel : ProductCreateModel
    {
        public Guid Id { get; set; }
    }
}

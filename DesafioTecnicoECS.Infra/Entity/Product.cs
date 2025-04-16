namespace DesafioTecnicoECS.Infra.Entity
{
    public class Product : BaseEntity
    {

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }
    }
}

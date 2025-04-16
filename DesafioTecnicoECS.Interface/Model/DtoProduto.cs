namespace DesafioTecnicoECS.Interface.Model
{
    public class DtoProduto
    {
        public string Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

    }
    public class DtoProdutoiCreate
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

    }
}
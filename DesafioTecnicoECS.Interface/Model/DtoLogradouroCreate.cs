using System;

namespace DesafioTecnicoECS.Interface.Model
{
    public class DtoLogradouroCreate
    {
        public string CEP { get; set; } = null!;
        public string Rua { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string Complemento { get; set; }

        public Guid IdCliente { get; set; }
    }

    public class DtoLogradouro : DtoLogradouroCreate
    {
        public Guid Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnicoECS.Infra.Entity
{
    public class Logradouro : BaseEntity
    {
        public string CEP { get; set; } = null!;
        public string Rua { get; set; } = null!;
        public string Cidade { get; set; } = null!;
        public string Bairro { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public string Numero { get; set; } = null!;
        public string? Complemento { get; set; }

        public Guid IdCliente { get; set; }

        [ForeignKey("IdCliente")]
        public virtual Cliente Cliente { get; set; }
    }
}

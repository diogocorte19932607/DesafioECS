using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnicoECS.Domain.Domain
{
    public class ClienteCreateModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public Logotipo Logotipo { get; set; }

    }

    public class ClienteEditModel : ClienteCreateModel
    {
        public Guid Id { get; set; }
    }

    public class Logotipo
    {
        public string Base64 { get; set; }
        public string Extensao { get; set; }
    }
}

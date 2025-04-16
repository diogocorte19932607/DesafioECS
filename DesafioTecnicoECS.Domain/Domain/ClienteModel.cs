using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesafioTecnicoECS.Domain.Domain
{
    public class ClienteModel : BaseDomain
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Logotipo { get; set; }
        public string Base64 { get; set; }
        public string Formato { get; set; }
    }
}

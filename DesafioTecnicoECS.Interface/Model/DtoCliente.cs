namespace DesafioTecnicoECS.Interface.Model
{
    public class DtoCliente
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Logotipo { get; set; }
        public string Base64 { get; set; }
        public string Formato { get; set; }
        public string LogoBase64 { get => (string.IsNullOrWhiteSpace(Base64) ? "" : $"data:image/{Formato.Replace(".", "")};base64,{Base64}"); }

    }
    public class DtoClienteCreate
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DtoLogotipo Logotipo { get; set; }
        
    }
    public class DtoLogotipo
    {
        public string Base64 { get; set; }
        public string Extensao { get; set; }
    }
}
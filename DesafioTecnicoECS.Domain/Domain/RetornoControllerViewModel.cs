namespace DesafioTecnicoECS.Domain.Domain
{
    public class RetornoControllerViewModel<ExibicaoMensagemViewModel, TObjeto>
    {
        public ExibicaoMensagemViewModel ExibicaoMensagem { get; set; }
        public TObjeto Objeto { get; set; }
    }
}

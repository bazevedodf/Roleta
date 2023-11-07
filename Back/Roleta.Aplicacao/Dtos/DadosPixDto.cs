
namespace Roleta.Aplicacao.Dtos
{
    public class DadosPixDto
    {
        public decimal Amount { get; set; }//Valor da cobrança Exemplo 15.20 = R$15,20
        public string PayerQuestion { get; set; }//Informação adicional para o pagador
        public string External_Id { get; set; }//Identificador único da sua aplicação para esse QRCode
        public string PayerName { get; set; } //Nome do pagador
        public string PayerDocument { get; set; } //Número do CPF ou CNPJ do pagador
    }
}

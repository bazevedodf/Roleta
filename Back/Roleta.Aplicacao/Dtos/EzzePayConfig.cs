namespace Roleta.Aplicacao.Dtos
{
    public class EzzePayConfig
    {
        public string ApiUrl { get; set; }
        public string Client_Id { get; set; }
        public string Client_Secret { get; set; }
        public string Signature_Secret { get; set; }
        public int TaxaSaque { get; set; }
        public int TaxaDeposito { get; set; }
    }
}

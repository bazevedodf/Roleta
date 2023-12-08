using Newtonsoft.Json;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;

namespace Roleta.Aplicacao
{
    public class EzzePayService : IEzzePayService
    {
        private readonly EzzePayConfig _credential;

        public EzzePayService(EzzePayConfig credential)
        {
            _credential = credential;
        }

        public bool ValidaAssinaturaWebHook(string reqTimestamp, string requestPayload, string reqSignature)
        {
            try
            {
                long ts;
                if (long.TryParse(reqTimestamp, out ts))
                {
                    using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_credential.Signature_Secret)))
                    {
                        byte[] data = Encoding.UTF8.GetBytes(ts + "." + requestPayload);
                        byte[] hash = hmac.ComputeHash(data);

                        string signedPayload = BitConverter.ToString(hash).Replace("-", "").ToLower();

                        return signedPayload.Equals(reqSignature, StringComparison.OrdinalIgnoreCase);
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<string> GetTokenAsync()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri(_credential.ApiUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _credential.Client_Id, _credential.Client_Secret))));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                    var content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await client.PostAsync("v2/oauth/token", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var tokenResponseContent = await response.Content.ReadAsStringAsync();
                        dynamic jsonObject = JsonConvert.DeserializeObject(tokenResponseContent);
                        return jsonObject.access_token;
                    }
                    else
                    {
                        throw new Exception($"Erro ao obter AccessToken: " + response.StatusCode);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erro ao solicitar token: {ex.Message}");
                }
            }
        }

        public async Task<PixDto> GetPixAsync(DadosPixDto dadosPix)
        {
            try
            {
                var resultPix = new PixDto();

                var accessToken = await GetTokenAsync();
                
                if (!string.IsNullOrEmpty(accessToken))
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(_credential.ApiUrl);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var paymentData = new
                            {
                                amount = dadosPix.Amount,
                                payerQuestion = dadosPix.PayerQuestion,
                                external_id = dadosPix.External_Id,
                                payer = new
                                {
                                    name = dadosPix.PayerName,
                                    document = dadosPix.PayerDocument.Replace(".", "").Replace("/", "").Replace("-", "")
                                }
                            };

                            var jsonPayload = JsonConvert.SerializeObject(paymentData);
                            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await client.PostAsync("v2/pix/qrcode", content);

                            if (response.IsSuccessStatusCode)
                            {
                                var qrCodeResponseContent = await response.Content.ReadAsStringAsync();
                                dynamic qrCodeResponse = JsonConvert.DeserializeObject(qrCodeResponseContent);

                                resultPix.Status = qrCodeResponse.status;
                                resultPix.QRCode = "data:image/jpeg;base64, " + qrCodeResponse.base64image;
                                resultPix.QRCodeText = qrCodeResponse.emvqrcps;
                                resultPix.TransactionId = qrCodeResponse.transactionId;
                                resultPix.Message = "Gerado com sucesso";
                                return resultPix;
                            }
                            else
                            {
                                resultPix.Status = "error";
                                resultPix.Message = $"Erro ao obter AccessToken: {response.StatusCode}";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        resultPix.Status = "error";
                        resultPix.Message = ex.Message;
                    }
                }
                else
                {
                    resultPix.Status = "error";
                    resultPix.Message = "Erro ao obter AccessToken";
                }

                return resultPix;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PixDto> ConsultaPixAsync(string transactionId)
        {
            try
            {
                var resultPix = new PixDto();
                //Status dos QRCodes
                //PENDING - APPROVED - EXPIRED - RETURNED

                var accessToken = await GetTokenAsync();

                if (!string.IsNullOrEmpty(accessToken))
                {
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(_credential.ApiUrl);
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            HttpResponseMessage response = await client.GetAsync($"v2/pix/qrcode/{transactionId}/detail");

                            if (response.IsSuccessStatusCode)
                            {
                                var qrCodeResponseContent = await response.Content.ReadAsStringAsync();
                                dynamic qrCodeResponse = JsonConvert.DeserializeObject(qrCodeResponseContent);

                                resultPix.Status = qrCodeResponse.status;
                                resultPix.QRCode = "data:image/jpeg;base64, " + qrCodeResponse.base64image;
                                resultPix.QRCodeText = qrCodeResponse.emvqrcps;
                                resultPix.TransactionId = qrCodeResponse.transactionId;
                                resultPix.Message = "Gerado com sucesso";
                                return resultPix;
                            }
                            else
                            {
                                resultPix.Status = $"ERROR";
                                resultPix.Message = $"Erro ao consultar o QrCode";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        resultPix.Status = "error";
                        resultPix.Message = ex.Message;
                    }
                }
                else
                {
                    resultPix.Status = "error";
                    resultPix.Message = "Erro ao obter AccessToken";
                }

                return resultPix;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto> SaquePix(SaqueDto saque, UserGameDto user)
        {
            var accessToken = await GetTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_credential.ApiUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    
                    var paymentData = new
                    {
                        amount = saque.Valor,
                        description = "Saque Pix",
                        external_id = "",
                        creditParty = new {
                            name = $"{user.FirstName} {user.LastName}" ,
                            keyType = user.TipoChavePix, //CPF - TELEFONE - EMAIL - CHAVE_ALEATORIA
                            key = user.ChavePix,
                            taxId = user.CPF
                        }
                    };

                    var jsonPayload = JsonConvert.SerializeObject(paymentData);
                    var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("v2/pix/payment", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var qrCodeResponseContent = await response.Content.ReadAsStringAsync();
                        dynamic qrCodeResponse = JsonConvert.DeserializeObject(qrCodeResponseContent);

                        saque.Status = qrCodeResponse.status;
                        saque.TransactionId = qrCodeResponse.transactionId;
                        saque.DataStatus = qrCodeResponse.createdAt;
                        saque.TextoInformativo = qrCodeResponse.creditParty.bank;
                        return saque;
                    }
                }
            }

            return null;
        }
    }
}

export interface Saque {
  id : number;
  transactionId : string;
  valor : number;
  status : string;
  textoInformativo : string;
  dataStatus : Date;
  dataCadastro : Date;
  userId : string;
}

import { UserDash } from "./Identity/UserDash";

export interface Pagamento {
  id : number;
  nome : string;
  cpf : string;
  transactionId : string;
  qrCode : string;
  qrCodeText : string;
  valor : number;
  status : string;
  dataStatus : Date;
  dataCadastro : Date;
  user: UserDash;
}

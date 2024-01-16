import { Carteira } from "./Carteira";
import { Pagamento } from "./Pagamento";
import { Saque } from "./Saque";

export interface UserGame {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  phoneNumber: string;
  cpf: string;
  tipoChavePix: string;
  chavePix: string;
  isAfiliate: boolean;
  demoAcount: boolean;
  parentEmail: string;
  dataCadastro: Date;
  verified: boolean;
  carteira: Carteira;
  pagamentos: Pagamento[];
  saques: Saque[];
}

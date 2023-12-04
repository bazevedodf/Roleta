import { Carteira } from "./Carteira";

export interface UserUpdate {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  cpf: string;
  chavePix: string;
  phoneNumber: string;
  isAfiliate : boolean;
  afiliateCode : string;
  comissao: number;
  isBlocked : boolean;
  demoAcount: boolean;
  dataCadastro: Date;
  carteira: Carteira;
}

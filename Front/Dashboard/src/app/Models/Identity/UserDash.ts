import { Carteira } from '../Carteira';
export interface UserDash {
  userName: string;
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  cpf: string;
  chavePix: string;
  isAfiliate : boolean;
  afiliateCode : string;
  comissao: number;
  ValorComissao: number;
  isBlocked : boolean;
  dataCadastro: Date;
  token: string;
  role: string;
  carteira: Carteira;
}

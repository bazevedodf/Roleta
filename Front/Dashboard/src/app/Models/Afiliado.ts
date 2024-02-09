import { Carteira } from "./Carteira";

export interface Afiliado {
  firstName : string ;
  lastName : string ;
  userName : string ;
  email : string ;
  isAfiliate : boolean ;
  comissao : number ;
  isBlocked : boolean ;
  demoAcount : boolean ;
  carteira : Carteira ;
  totalDepositos : number ;
  totalFaturamento : number ;
}

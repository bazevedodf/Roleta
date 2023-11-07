import { GiroRoleta } from "./GiroRoleta";
import { Pagamento } from "./Pagamento";
import { Saque } from "./Saque";

export interface UserGame {
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  freeSpin: number;
  saldoDeposito: number;
  saldoSaque: number;
  verified: boolean;
  pagamentos: Pagamento[];
  girosRoleta: GiroRoleta[];
  saques: Saque[];
}

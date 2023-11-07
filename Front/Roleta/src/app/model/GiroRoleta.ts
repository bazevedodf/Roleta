import { User } from "./identity/user"

export interface GiroRoleta {
  id: number;
  valorAposta: number;
  posicao: number;
  multiplicador: number;
  userId: string;
  user: User;
  data: Date;
}

import { BancaPagadora } from "./BancaPagadora";

export interface RoletaSorte {
  id: number;
  nome: string;
  saldoLucro: number;
  premiacaoMaxima: number;
  valorMinimoSaque: number;
  valorMaximoSaque: number;
  percentualBanca: number;
  taxaSaque: number;
  bloquearSaque: boolean;
  taxaPerda: number;
  contagemPerda: number;
  bancasPagadoras: BancaPagadora[];
}

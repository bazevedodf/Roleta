export interface Pagamento {
  nome: string;
  cPF: string;
  transactionId: string;
  valor: number;
  status: string;
  dataStatus: Date;
  dataCadastro: Date;
  produtoId: number;
}

export interface Saque {
  id: number
  transactionId: string
  valor: number
  status: string
  description: string
  textoInformativo: string
  dataStatus:  Date
  dataCadastro: Date
}

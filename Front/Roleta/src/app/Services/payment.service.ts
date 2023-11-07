import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DadosPix } from '@app/model/DadosPix';
import { Pix } from '@app/model/Pix';
import { environment } from '@environments/environment';
import { Observable, map, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  private baseUrl = environment.apiURL+ 'api/Payment'

  constructor(private http: HttpClient) { }

  public gerarPix(model: DadosPix): Observable<any>{
    return this.http.post(`${this.baseUrl}/GetPix`, model);
  }

  /* public consultarPix(transacao: string): Observable<any>{
    const params = {
      transactionId : transacao
    }
    return this.http.get(`${this.baseUrl}/ConsultaQRCode`,{params});
  } */
}

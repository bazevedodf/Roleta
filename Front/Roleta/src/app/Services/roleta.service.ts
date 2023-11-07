import { DecimalPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { Observable, take, lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class RoletaService {

  private baseUrl = environment.apiURL+ 'api/Roleta'

  constructor(private http: HttpClient) { }

  public SpinBet(vlAposta: number, fSpin: boolean): Observable<any>{
    const params = {
      valorAposta : vlAposta,
      freeSpin : fSpin
    }
    return this.http.get(`${this.baseUrl}/SpinBet`, {params}).pipe(take(1));
  }

  public Saque(vlSaque: number): Observable<any>{
    const params = {
      valor : vlSaque
    }
    return this.http.get(`${this.baseUrl}/Saque`, {params}).pipe(take(1));
  }

  public GetOfertas(): Observable<any>{
    return this.http.get(`${this.baseUrl}/GetOfertas`).pipe(take(1));
  }

  public GetItens(): Observable<any> {
    return this.http.get(`${this.baseUrl}/ItensRoleta`).pipe(take(1));
  }

}

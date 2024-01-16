import { formatDate } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserDash } from '@app/Models/Identity/UserDash';
import { Pagamento } from '@app/Models/Pagamento';
import { PaginatedResult } from '@app/Models/Pagination';
import { UserUpdate } from '@app/Models/UserUpdate';
import { environment } from '@environments/environment';
import { Observable, map, take } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashBoardService {

  public baseUrl = environment.apiURL + 'api/Dashboard';

  constructor(private http: HttpClient) { }

  public GetTotais(): Observable<any>{
    return this.http.get(`${this.baseUrl}/GetDashData`).pipe(take(1));
  }

  public getUserDash(email: string): Observable<UserUpdate>{
    const params = {
      email : email
    }
    return this.http.get<UserUpdate>(`${this.baseUrl}/GetUser`, {params}).pipe(take(1));
  }

  public GetUsers(page?: number, itemsPerPage?: number, dataIni?: string, dataFim?: string, paramName?: string): Observable<PaginatedResult<UserDash[]>>{
    const paginatedResult : PaginatedResult<UserDash[]> = new PaginatedResult<UserDash[]>();
    let params = new HttpParams;

    if(page && itemsPerPage){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if(dataIni && dataFim){
      params = params.append('dataIni', dataIni.toString());
      params = params.append('dataFim', dataFim.toString());
    }

    if(paramName){
      params = params.append('term', paramName.toString());
    }

    return this.http
      .get(`${this.baseUrl}/GetUsers`, { observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body as UserDash[];
          if(response.headers.has('PaginationUser')){
            paginatedResult.pagination = JSON.parse(response.headers.get('PaginationUser') as any);
          }
          return paginatedResult;
        }));
  }

  public GetUsersNomeData(page?: number, itemsPerPage?: number, dataIni?: string, dataFim?: string, paramName?: string): Observable<PaginatedResult<UserDash[]>>{
    const paginatedResult : PaginatedResult<UserDash[]> = new PaginatedResult<UserDash[]>();
    let params = new HttpParams;

    if(page && itemsPerPage){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if(dataIni && dataFim){
      params = params.append('dataIni', dataIni.toString());
      params = params.append('dataFim', dataFim.toString());
    }

    if(paramName){
      params = params.append('term', paramName.toString());
    }

    return this.http
      .get(`${this.baseUrl}/GetUsers`, { observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body as UserDash[];
          if(response.headers.has('PaginationUser')){
            paginatedResult.pagination = JSON.parse(response.headers.get('PaginationUser') as any);
          }
          return paginatedResult;
        }));
  }

  public GetPagamentos(page?: number, itemsPerPage?: number, dataIni?: string, dataFim?: string): Observable<PaginatedResult<Pagamento[]>>{
    const paginatedResult : PaginatedResult<Pagamento[]> = new PaginatedResult<Pagamento[]>();
    let params = new HttpParams;

    if(page && itemsPerPage){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if(dataIni && dataFim){
      params = params.append('dataIni', dataIni.toString());
      params = params.append('dataFim', dataFim.toString());
    }

    return this.http
      .get(`${this.baseUrl}/GetPagamentos`, { observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body as Pagamento[];
          if(response.headers.has('PaginationPag')){
            paginatedResult.pagination = JSON.parse(response.headers.get('PaginationPag') as any);
          }
          return paginatedResult;
        }));
  }

  public putUser(userDash: UserUpdate): Observable<UserUpdate> {
    return this.http
      .put<UserUpdate>(`${this.baseUrl}/PutUser`, userDash)
      .pipe(take(1));
  }
}

import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/Models/Identity/User';
import { Pagamento } from '@app/Models/Pagamento';
import { PaginatedResult } from '@app/Models/Pagination';
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

  public GetUsers(page?: number, itemsPerPage?: number): Observable<PaginatedResult<User[]>>{
    const paginatedResult : PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams;

    if(page && itemsPerPage){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    return this.http
      .get(`${this.baseUrl}/GetUsers`, { observe: 'response', params })
      .pipe(
        take(1),
        map((response) => {
          paginatedResult.result = response.body as User[];
          if(response.headers.has('PaginationUser')){
            paginatedResult.pagination = JSON.parse(response.headers.get('PaginationUser') as any);
          }
          return paginatedResult;
        }));
  }

  public GetPagamentos(page?: number, itemsPerPage?: number): Observable<PaginatedResult<Pagamento[]>>{
    const paginatedResult : PaginatedResult<Pagamento[]> = new PaginatedResult<Pagamento[]>();

    let params = new HttpParams;

    if(page && itemsPerPage){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
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
}

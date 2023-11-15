import { Component, OnInit } from '@angular/core';
import { DashBoardTotal } from '@app/Models/DashBoardTotal';
import { User } from '@app/Models/Identity/User';
import { Pagamento } from '@app/Models/Pagamento';
import { PaginatedResult, Pagination } from '@app/Models/Pagination';
import { DashBoardService } from '@app/Services/dash-board.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public users: User[] = [];
  public pagamentos: Pagamento[] = [];
  public totais = {} as DashBoardTotal;
  public userPagination = {} as Pagination;
  public pagamentoPagination = {} as Pagination;

  constructor( private dashboradService: DashBoardService,
               private toastr: ToastrService,
               private spinner: NgxSpinnerService){}

  ngOnInit(): void {
    this.userPagination = { currentPage: 1, itemsPerPage: 7, totalItems: 1 } as Pagination;
    this.pagamentoPagination = { currentPage: 1, itemsPerPage: 7, totalItems: 1 } as Pagination;
    this.carregarTotais();
    this.carregarUsers();
    this.carregarPagamentos();
  }

  public carregarUsers(): void {
    this.spinner.show();
    this.dashboradService
      .GetUsers(this.userPagination.currentPage, this.userPagination.itemsPerPage).subscribe({
        next:(paginatedResult: PaginatedResult<User[]>) => {
          this.users = paginatedResult.result;
          this.userPagination = paginatedResult.pagination !!;
          console.log(this.users);
        },
        error:(error:any) => {}
      }).add(() => this.spinner.hide());
  }

  public carregarPagamentos(): void {
    this.spinner.show();
    this.dashboradService
      .GetPagamentos(this.pagamentoPagination.currentPage, this.pagamentoPagination.itemsPerPage).subscribe({
        next:(paginatedResult: PaginatedResult<Pagamento[]>) => {
          this.pagamentos = paginatedResult.result;
          this.pagamentoPagination = paginatedResult.pagination !!;
          console.log(this.pagamentos);
        },
        error:(error:any) => {}
      }).add(() => this.spinner.hide());
  }

  public carregarTotais(): void {
    this.spinner.show();
    this.dashboradService
      .GetTotais().subscribe({
        next:(result: DashBoardTotal) => {
          if (result){
            this.totais = result;
            console.log(this.totais);
          }

        },
        error:(error:any) => {
          this.toastr.error('Erro ao consultar o servidor', 'Error!')
        }
      }).add(() => this.spinner.hide());
  }

  public userPageChanged(event: any): void{
    this.userPagination.currentPage = event.page;
    this.carregarUsers();
  }

  public pagamentoPageChanged(event: any): void{
    this.pagamentoPagination.currentPage = event.page;
    this.carregarPagamentos();
  }

}

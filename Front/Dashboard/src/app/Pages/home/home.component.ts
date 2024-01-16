import { formatDate } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { DashBoardTotal } from '@app/Models/DashBoardTotal';
import { UserDash } from '@app/Models/Identity/UserDash';
import { Pagamento } from '@app/Models/Pagamento';
import { PaginatedResult, Pagination } from '@app/Models/Pagination';
import { DashBoardService } from '@app/Services/dash-board.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public users: UserDash[] = [];
  public pagamentos: Pagamento[] = [];
  public totais = {} as DashBoardTotal;
  public userPagination = {} as Pagination;
  public pagamentoPagination = {} as Pagination;
  public vlTotalDepositosHoje: number = 0;

  public date = new Date();
  public dtIni = new Date();
  public dtFim = new Date();
  public dtIniP = new Date();
  public dtFimP = new Date();

  get bsConfig(): any{
    return {
      isAnimated: true,
      adaptivePosition: true,
      showWeekNumbers: false
    };
  }

  constructor( private dashboradService: DashBoardService,
               private toastr: ToastrService,
               private localeService: BsLocaleService,
               private spinner: NgxSpinnerService){
    this.localeService.use('pt-br');
    this.dtFim.setDate( this.date.getDate() + 1 );
    this.dtFimP.setDate( this.date.getDate() + 1 );
  }

  ngOnInit(): void {
    this.userPagination = { currentPage: 1, itemsPerPage: 10, totalItems: 0 } as Pagination;
    this.pagamentoPagination = { currentPage: 1, itemsPerPage: 10, totalItems: 0 } as Pagination;
    this.carregarUsers();
    this.carregarTotais();
    this.carregarPagamentos();
  }

  public carregarUsers(): void {
    this.spinner.show();
    this.dashboradService
      .GetUsers(this.userPagination.currentPage,
                this.userPagination.itemsPerPage,
                formatDate(this.dtIni,'MM/dd/yyyy', 'en-US'),
                formatDate(this.dtFim,'MM/dd/yyyy', 'en-US')).subscribe({
        next:(paginatedResult: PaginatedResult<UserDash[]>) => {
          this.users = paginatedResult.result;
          this.userPagination = paginatedResult.pagination !!;
/*           this.carregarTotais();
          this.carregarPagamentos(); */
        },
        error:(error:any) => {}
      }).add(() => this.spinner.hide());
  }

  public carregarPagamentos(): void {
    this.spinner.show();
    this.dashboradService
      .GetPagamentos(this.pagamentoPagination.currentPage,
                     this.pagamentoPagination.itemsPerPage,
                     formatDate(this.dtIniP,'MM/dd/yyyy', 'en-US'),
                     formatDate(this.dtFimP,'MM/dd/yyyy', 'en-US')).subscribe({
        next:(paginatedResult: PaginatedResult<Pagamento[]>) => {
          this.pagamentos = paginatedResult.result;
          this.pagamentoPagination = paginatedResult.pagination !!;
          this.vlTotalDepositosHoje = 0;
          this.vlTotalDepositosHoje = this.pagamentos.map(p => p.valor).reduce((a, b) => this.vlTotalDepositosHoje += b, 0);
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

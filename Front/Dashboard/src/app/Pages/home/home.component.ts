import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DashBoardTotal } from '@app/Models/DashBoardTotal';
import { UserDash } from '@app/Models/Identity/UserDash';
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

  public users: UserDash[] = [];
  public pagamentos: Pagamento[] = [];
  public totais = {} as DashBoardTotal;
  public userPagination = {} as Pagination;
  public pagamentoPagination = {} as Pagination;

  constructor( private dashboradService: DashBoardService,
               private activatedRouter: ActivatedRoute,
               private toastr: ToastrService,
               private spinner: NgxSpinnerService){}

  ngOnInit(): void {
    this.verificaLink();
    this.userPagination = { currentPage: 1, itemsPerPage: 7, totalItems: 1 } as Pagination;
    this.pagamentoPagination = { currentPage: 1, itemsPerPage: 7, totalItems: 1 } as Pagination;
    this.carregarUsers();

  }

  private verificaLink(): void{
    debugger;
    const afiliateCode = this.activatedRouter.snapshot.paramMap.get('afl');
    if (afiliateCode) {
      console.log("Afiliado: "+ afiliateCode);
    }
  }

  public carregarUsers(): void {
    this.spinner.show();
    this.dashboradService
      .GetUsers(this.userPagination.currentPage, this.userPagination.itemsPerPage).subscribe({
        next:(paginatedResult: PaginatedResult<UserDash[]>) => {
          this.users = paginatedResult.result;
          this.userPagination = paginatedResult.pagination !!;
          this.carregarTotais();
          this.carregarPagamentos();
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

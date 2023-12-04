import { formatDate } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { UserDash } from '@app/Models/Identity/UserDash';
import { PaginatedResult, Pagination } from '@app/Models/Pagination';
import { DashBoardService } from '@app/Services/dash-board.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cadastro-lista',
  templateUrl: './cadastro-lista.component.html',
  styleUrls: ['./cadastro-lista.component.scss']
})
export class CadastroListaComponent implements OnInit {

  private locale: string = 'pt-br';
  modalRef?: BsModalRef;

  public users: UserDash[] = [];
  public userSelected!: UserDash;
  public userPagination = {} as Pagination;

  public date = new Date();
  public dtIni = new Date();
  public dtFim = new Date();
  public paramName: string = '';

  get bsConfig(): any{
    return {
      isAnimated: true,
      adaptivePosition: true,
      showWeekNumbers: false
    };
  }

  constructor( private dashboradService: DashBoardService,
               private toastr: ToastrService,
               private router: Router,
               private localeService: BsLocaleService,
               private spinner: NgxSpinnerService,
               private modalService: BsModalService){

    this.localeService.use('pt-br');
    this.dtFim.setDate( this.date.getDate() + 1 );

  }

  ngOnInit(): void {
    this.userPagination = { currentPage: 1, itemsPerPage: 20, totalItems: 1 } as Pagination;
    this.carregarUsers();
  }

  public carregarUsers(): void {
    this.spinner.show();
    this.dashboradService
      .GetUsers(this.userPagination.currentPage,
                this.userPagination.itemsPerPage,
                formatDate(this.dtIni,'MM/dd/yyyy', 'en-US'),
                formatDate(this.dtFim,'MM/dd/yyyy', 'en-US'),
                this.paramName).subscribe({
        next:(paginatedResult: PaginatedResult<UserDash[]>) => {
          this.users = paginatedResult.result;
          this.userPagination = paginatedResult.pagination !!;
        },
        error:(error:any) => {
          this.toastr.error('Erro ao carregar usuários');
        }
      }).add(() => this.spinner.hide());
  }

  public userPageChanged(event: any): void{
    this.userPagination.currentPage = event.page;
    this.carregarUsers();
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  public selectUser(email: string): void{
    //const receita = this.users.find( item => item.email === email);
    //this.userSelected = receita as UserDash;
    //console.log(this.userSelected);
    this.router.navigate([`/cadastros/detalhe/${email}`]);
  }

}

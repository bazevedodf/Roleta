import { formatDate } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Afiliado } from '@app/Models/Afiliado';
import { PaginatedResult, Pagination } from '@app/Models/Pagination';
import { Saque } from '@app/Models/Saque';
import { UserUpdate } from '@app/Models/UserUpdate';
import { DashBoardService } from '@app/Services/dash-board.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-afiliado-lista',
  templateUrl: './afiliado-lista.component.html',
  styleUrls: ['./afiliado-lista.component.scss']
})
export class AfiliadoListaComponent implements OnInit {

  private locale: string = 'pt-br';
  modalRef?: BsModalRef;
  public form!: FormGroup;

  public users: Afiliado[] = [];
  public userSelected!: UserUpdate;
  public userPagination = {} as Pagination;
  public comprovantePix = {} as Saque;
  public includeBlocks: boolean = false;

  public date = new Date();
  public dtIni = new Date();
  public dtFim = new Date();
  public paramName: string = '';
  public tipoSaque: boolean = true;
  public valorTrasferencia: number = 0;
  public errorMsg: string = '';
  public mensagem: string = '';

  get bsConfig(): any{
    return {
      isAnimated: true,
      adaptivePosition: true,
      showWeekNumbers: false
    };
  }

  public get f(): any {
    return this.form.controls;
  }

  constructor( private dashboradService: DashBoardService,
               private toastr: ToastrService,
               private router: Router,
               private spinner: NgxSpinnerService,
               private fb: FormBuilder,
               private localeService: BsLocaleService,
               private modalService: BsModalService){}

  ngOnInit(): void {
    this.localeService.use('pt-br');
    this.dtFim.setDate( this.date.getDate() + 1 );
    this.userPagination = { currentPage: 1, itemsPerPage: 20, totalItems: 1 } as Pagination;

    this.paramName = 'bazevedo';
    this.carregarAfiliados();
  }

  public toggle(operacao: string): void{
    if (operacao == 'Saque'){
      this.tipoSaque = true;
      this.valorTrasferencia = this.userSelected.carteira.saldoAtual;
    }
    else{
      this.tipoSaque = false;
      this.valorTrasferencia = 0;
    }
  }

  public enviarPix(): void{
    if (this.valorTrasferencia <= 0){
      this.errorMsg = 'ValorInvalido';
      return;
    }
    if (this.userSelected.firstName == '' || this.userSelected.firstName == undefined) {
      this.errorMsg = 'DadosInvalidos';
      return;
    }
    if (this.userSelected.lastName == '' || this.userSelected.lastName == undefined) {
      this.errorMsg = 'DadosInvalidos';
      return;
    }
    if (this.userSelected.tipoChavePix == '' || this.userSelected.chavePix == ''){
      this.errorMsg = 'DadosInvalidos';
      return;
    }

    this.spinner.show();
    this.dashboradService.TransferenciaPix(this.userSelected.email, this.tipoSaque, this.valorTrasferencia).subscribe({
      next: (saque : Saque) => {
        if (saque){
          this.comprovantePix = saque;
          this.getUserSelected(this.userSelected.email);
          this.toastr.success("Transferencia realizada com sucesso!", "Sucesso!");
        }
      },
      error: (error:any) => {
        this.errorMsg = 'RetornoInvalido';
        this.mensagem = error.error;
        this.toastr.error(error.error, "Erro!");
      },
    }).add(() => this.spinner.hide());

  }

  public carregarAfiliados(): void {
    this.spinner.show();
    this.dashboradService
      .GetAfiliates(this.userPagination.currentPage,
                this.userPagination.itemsPerPage,
                formatDate(this.dtIni,'MM/dd/yyyy', 'en-US'),
                formatDate(this.dtFim,'MM/dd/yyyy', 'en-US'),
                this.paramName, this.includeBlocks).subscribe({
        next:(paginatedResult: PaginatedResult<Afiliado[]>) => {
          this.users = paginatedResult.result;
          this.userPagination = paginatedResult.pagination !!;
        },
        error:(error:any) => {
          this.toastr.error('Erro ao carregar usuários');
        }
      }).add(() => this.spinner.hide());
  }

  public openModal(template: TemplateRef<void>, email: string) {
    if (email !== null){
      this.spinner.show();
      this.dashboradService.getUserDash(email).subscribe({
        next: (user: UserUpdate) => {
          this.userSelected = {} as UserUpdate;
          this.userSelected = {...user};
          this.valorTrasferencia = user.carteira.saldoAtual;
          this.modalRef = this.modalService.show(template, Object.assign({}, { class: 'gray modal-lg' }));
        },
        error:(error:any) => {
          this.toastr.error('Erro ao processar pagamento!','Erro!');
        }
      }).add(() => this.spinner.hide());

    }
  }

  public getUserSelected(email: string): void{
    this.spinner.show();
    this.dashboradService.getUserDash(email).subscribe({
      next: (user: UserUpdate) => {
        console.log(user);
        this.userSelected = {} as UserUpdate;
        this.userSelected = {...user};
        this.valorTrasferencia = user.carteira.saldoAtual;
      },
      error:(error:any) => {
        this.toastr.error('Erro ao processar pagamento!','Erro!');
      }
    }).add(() => this.spinner.hide());
  }

  public setSaldoDemo(): void{
    this.spinner.show();
    this.dashboradService.SetSaldoDemo(50).subscribe({
      next:(retorno: boolean) => {
        if(retorno)
          this.toastr.success('Saldo Demo Atualizados','Sucesso!');
        else
          this.toastr.error('Não foi possível atualizar o saldo demo','Error!');
      },
      error:(error:any) => {
        this.toastr.error('Erro ao redefinir saldo demo');
      }
    }).add(() => this.spinner.hide());
  }

  public AfiliatePageChanged(event: any): void{
    this.userPagination.currentPage = event.page;
    this.carregarAfiliados();
  }
}

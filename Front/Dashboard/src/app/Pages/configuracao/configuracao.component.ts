import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { DashBoardService } from '@app/Services/dash-board.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { RoletaSorte } from '@app/Models/RoletaSorte';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-configuracao',
  templateUrl: './configuracao.component.html',
  styleUrls: ['./configuracao.component.scss']
})
export class ConfiguracaoComponent implements OnInit {

  public form!: FormGroup;
  public roletaSorte = {} as RoletaSorte;

  public dataHoje = new Date();

  public get f(): any {
    return this.form.controls;
  }

  constructor(private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private dashboradService: DashBoardService,
              private fb: FormBuilder,
              private router: Router,){

  }

  ngOnInit(): void {
    this.Validacao();
    this.carregarRoleta();
  }

  public Validacao(): void {
    this.form = this.fb.group({
      saldoLucro: [0],
      premiacaoMaxima: [0],
      valorMinimoSaque: [0],
      valorMaximoSaque: [0],
      percentualBanca: [0],
      taxaSaque: [0],
      bloquearSaque: [false],
      bancaHoje: [0],
      bancaAmanha: [0]
    });
  }

  public carregarRoleta(): void {
    this.spinner.show();
      this.dashboradService.GetRoleta().subscribe({
        next: (roleta: RoletaSorte) => {
          if(roleta) {
            this.roletaSorte = roleta;
            console.log(this.roletaSorte);
            if (roleta.bancasPagadoras.length > 1){
              this.form.controls['bancaHoje'].setValue(roleta.bancasPagadoras[0].saldoDia);
              this.form.controls['bancaAmanha'].setValue(roleta.bancasPagadoras[1].saldoDia);
            }
            if (roleta.bancasPagadoras.length == 1){
              this.form.controls['bancaHoje'].setValue(roleta.bancasPagadoras[0].saldoDia);
            }
          }
        },
        error:(error:any) => {
          this.toastr.error('Erro ao carregar  informações da roleta', 'Error');
        }
      }).add(() => this.spinner.hide());
  }

  public salvarConfig(): void {
    this.spinner.show();
    if (this.form.valid) {
      if (this.roletaSorte.bancasPagadoras.length > 0){
        this.roletaSorte.bancasPagadoras[0].saldoDia = this.f.bancaHoje.value;
        if (this.roletaSorte.bancasPagadoras.length > 2)
          this.roletaSorte.bancasPagadoras[0].saldoDia = this.f.bancaAmanha.value;
      }
      this.dashboradService.putRoleta(this.roletaSorte).subscribe({
        next: (roleta: RoletaSorte) => {
          this.toastr.success('Configurações salvas com Sucesso!', 'Sucesso');
          this.carregarRoleta();
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Error ao salvar as configurações', 'Erro');
        }
      }).add(() => this.spinner.hide());
    }
  }

  public comparaData(data : Date): string{
    if (data != undefined){
      let data1 = formatDate(data,'yyyy-MM-dd','en_US');
      let data2 = formatDate(new Date(),'yyyy-MM-dd','en_US');
      if(data1 > data2)
        return "Amanhã"
      else
        return "de Hoje";
    }
    else
      return "de Hoje"
  }

  public btnCancelar(): void{
    this.router.navigate([`home`]);
  }

}

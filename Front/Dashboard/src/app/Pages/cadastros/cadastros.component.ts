import { formatDate } from '@angular/common';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { UserDash } from '@app/Models/Identity/UserDash';
import { PaginatedResult, Pagination } from '@app/Models/Pagination';
import { DashBoardService } from '@app/Services/dash-board.service';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cadastros',
  templateUrl: './cadastros.component.html',
  styleUrls: ['./cadastros.component.scss']
})
export class CadastrosComponent implements OnInit {

  constructor( ){

  }

  ngOnInit(): void {

  }


}

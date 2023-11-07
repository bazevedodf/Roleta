import { Component, OnInit, ViewChild } from '@angular/core';
import { Item, NgxWheelComponent, TextAlignment, TextOrientation } from '../ngx-wheel/ngx-wheel.component';
import { NgxSpinnerService } from 'ngx-spinner';
import { RoletaService } from '@app/Services/roleta.service';
import { ToastrService } from 'ngx-toastr';
import { lastValueFrom } from 'rxjs';
//import { Item, NgxWheelComponent, TextAlignment, TextOrientation } from './Components/ngx-wheel/ngx-wheel.Component';

@Component({
  selector: 'app-roleta',
  templateUrl: './roleta.component.html',
  styleUrls: ['./roleta.component.scss']
})
export class RoletaComponent implements OnInit {
  @ViewChild(NgxWheelComponent, { static: false }) wheel: any;

  seed = [...Array(18).keys()]

  idToLandOn: number = 0;
  textOrientation: TextOrientation = TextOrientation.HORIZONTAL
  textAlignment: TextAlignment = TextAlignment.OUTER
  items: any = [];

  /* items = [
    {id:1, text:'Prize 1', fillStyle:'#FF0000', textFillStyle: 'white', textFontSize: '16'},
    {id:2, text:'Prize 2', fillStyle:'#000000', textFillStyle: 'white', textFontSize: '16'},
    {id:3, text:'Prize 3', fillStyle:'#FF0000', textFillStyle: 'white', textFontSize: '16'},
    {id:4, text:'Prize 4', fillStyle:'#000000', textFillStyle: 'white', textFontSize: '16'},
    {id:5, text:'Prize 5', fillStyle:'#FF0000', textFillStyle: 'white', textFontSize: '16'},
    {id:6, text:'Prize 6', fillStyle:'#000000', textFillStyle: 'white', textFontSize: '16'},
    {id:7, text:'Prize 7', fillStyle:'#FF0000', textFillStyle: 'white', textFontSize: '16'},
    {id:8, text:'Prize 8', fillStyle:'#000000', textFillStyle: 'white', textFontSize: '16'},
  ] ; */
  constructor(private spinner: NgxSpinnerService,
              private roletaService: RoletaService,
              private toastr: ToastrService){
  }

  ngOnInit(): void {
    //this.idToLandOn = this.seed[Math.floor(Math.random() * this.seed.length)];
    this.idToLandOn = 1;
    const colors = ['#FF0000', '#000000']
    this.items = this.seed.map((value) => ({
      fillStyle: colors[value % 2],
      text: `Fofo${value}`,
      id: value,
      textFillStyle: 'white',
      textFontSize: '16'
    }))
  }

  private async getItens(){
    try {
      var value = this.roletaService.GetItens();
      value = await lastValueFrom(value);
      this.items = value;
      console.log(this.items);
    } catch (error) {
      console.log("errorRes", error)
    }

  }

  public after(): void{
    console.log('after')
  }

  public before(): void{
    console.log('before');
  }

  public reset(): void{
    this.wheel.reset();
  }

  public spin(): void{

    this.wheel.spin();
  }

}

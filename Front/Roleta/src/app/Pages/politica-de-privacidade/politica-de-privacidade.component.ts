import { Component, OnInit, OnDestroy, Renderer2 } from '@angular/core';

@Component({
  selector: 'app-politica-de-privacidade',
  templateUrl: './politica-de-privacidade.component.html',
  styleUrls: ['./politica-de-privacidade.component.scss']
})
export class PoliticaDePrivacidadeComponent implements OnInit, OnDestroy{
  public isCollapsed = true;

  constructor(private renderer: Renderer2){
  }

  ngOnInit(): void {
    this.renderer.addClass(document.querySelector('body'), "bg-01");
  }

  ngOnDestroy(): void {
    this.renderer.removeClass(document.querySelector('body'), "bg-01");
  }
}

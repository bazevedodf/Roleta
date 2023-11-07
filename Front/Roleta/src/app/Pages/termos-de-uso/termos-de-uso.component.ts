import { Component, Renderer2, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-termos-de-uso',
  templateUrl: './termos-de-uso.component.html',
  styleUrls: ['./termos-de-uso.component.scss']
})
export class TermosDeUsoComponent implements OnInit, OnDestroy{
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

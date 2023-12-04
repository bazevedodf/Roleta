import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'Roleta';

  constructor(private activatedRouter: ActivatedRoute){
  }

  private verificaLink(): void{
    debugger;
    const afiliateCode = this.activatedRouter.snapshot.paramMap.get('afl');
    if (afiliateCode) {
      console.log("Afiliado: "+ afiliateCode);
    }
  }
}

import { Component, OnInit, Input, AfterViewInit, Output, EventEmitter } from '@angular/core';

export interface Item {
  text: string,
  fillStyle: string,
  id: any,
}

export enum TextAlignment {
  INNER = 'inner',
  OUTER = 'outer',
  CENTER = 'center',
}

export enum TextOrientation {
  HORIZONTAL = 'horizontal',
  VERTICAL = 'vertical',
  CURVED = 'curved',
}

@Component({
  selector: 'app-ngx-wheel',
  template: `
    <canvas (click)='!disableSpinOnClick' id='canvas' [width]='width' [height]='height'>
        Canvas not supported, use another browser.
    </canvas>
  `,
  styles: []
})
export class NgxWheelComponent implements OnInit, AfterViewInit{
  constructor() { }

  @Input() height!: number;
  @Input() idToLandOn: any;
  @Input() width!: number;
  @Input() items: Item[] = [];
  @Input() spinDuration!: number;
  @Input() spinAmount!: number;
  @Input() innerRadius!: number;
  @Input() pointerStrokeColor!: string;
  @Input() pointerFillColor!: string;
  @Input() disableSpinOnClick: boolean = true;
  @Input() textOrientation!: TextOrientation;
  @Input() textAlignment!: TextAlignment;
  //@Input() soundFile!: string;


  @Output() onSpinStart: EventEmitter<any> = new EventEmitter();
  @Output() onSpinComplete: EventEmitter<any> = new EventEmitter();

  wheel: any
  completedSpin: boolean = false;
  isSpinning: boolean = false;

  reset() {
    this.wheel.stopAnimation(false);
    this.wheel.rotationAngle = 0;
    this.wheel.ctx.clearRect(0, 0, this.wheel.ctx.canvas.width, this.wheel.ctx.canvas.height);
    this.isSpinning = false
    this.completedSpin = false
    this.ngAfterViewInit()
  }

  ngOnInit(): void {
  }

  spin(position: number) {
    if (this.completedSpin || this.isSpinning) return
    this.isSpinning = true
    this.onSpinStart.emit(null)
    const segmentsFiltered = this.wheel.segments.filter((x:any) => !!x);
    //const segmentToLandOn = segmentsFiltered.find((x:any) => this.idToLandOn === x.id)
    const segmentToLandOn = segmentsFiltered.find((x:any) => position === x.id)
    const segmentTheta = segmentToLandOn.endAngle - segmentToLandOn.startAngle
    this.wheel.animation.stopAngle = segmentToLandOn.endAngle - (segmentTheta / 4);
    this.wheel.startAnimation()
    setTimeout(() => {
      this.completedSpin = true
      this.onSpinComplete.emit(null)
    }, this.spinDuration * 1000)
  }

  ngAfterViewInit() {
    const segments = this.items
    // @ts-ignore
    this.wheel = new Winwheel({
      numSegments: segments! ? segments.length : 0,
      segments,
      innerRadius: this.innerRadius || 0,
      outerRadius: (this.height / 2) - 20,
      centerY: (this.height / 2) + 20,
      pointerFillColor: 'white',
      textOrientation : this.textOrientation,
      textAligment : this.textAlignment,
      textMargin : 0,
      textFontFamily : 'Verdana',
      textFontWeight : 'bold',
      textStrokeStyle : 'black',
      animation:
      {
        type: 'spinToStop', // Type of animation.
        duration: this.spinDuration, // How long the animation is to take in seconds.
        spins: this.spinAmount, // The number of complete 360 degree rotations the wheel is to do.
        //callbackFinished : this.alertPrize,
        //soundFile: this.soundFile,
      },
      /* pins:				// Turn pins on.
      {
        number     : 18,
        fillStyle  : 'black',
        outerRadius: 4,
      } */
    })
    // @ts-ignore
    TweenMax.ticker.addEventListener("tick", this.drawPointer.bind(this));
  }

  ngOnDestroy() {
    // @ts-ignore
    TweenMax.ticker.removeEventListener("tick")
  }

  drawPointer(){
    let c = this.wheel.ctx;
    // Create pointer.
    if (c) {
      c.save();
      c.lineWidth = 2;
      c.strokeStyle = this.pointerStrokeColor;
      c.fillStyle = this.pointerFillColor;
      c.beginPath();
      c.moveTo((this.width / 2) - 20, 2);
      c.lineTo((this.width / 2) + 20, 2);
      c.lineTo((this.width / 2), 42);
      c.lineTo((this.width / 2) - 20, 2);
      c.stroke();
      c.fill();
      c.restore();
    }
  }

  alertPrize(indicatedSegment: any)
  {
    alert("asdasdasdasd asd asd ads " + indicatedSegment.text);
  }
}

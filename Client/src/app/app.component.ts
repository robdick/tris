import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
  AfterViewInit
} from '@angular/core';
import * as Pixi from 'pixi.js';
import { TrisApiService } from 'src/services/tris-api.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterViewInit {
  title = 'tris-client';

  @ViewChild('triscanvas', { static: false }) trisCanvas: ElementRef;

  private pixiApp: Pixi.Application;
  private pixiStage: Pixi.Container;

  constructor(private trisApi: TrisApiService) {}

  public ngAfterViewInit() {
    this.pixiApp = new Pixi.Application({
      view: this.trisCanvas.nativeElement,
      width: 600,
      height: 600,
      backgroundColor: 0xffffffff
    });

    this.pixiStage = this.pixiApp.stage;

    var grid = new Pixi.Graphics();

    grid.beginFill(0x222222, 1);
    grid.drawRect(0, 0, 600, 600);
    grid.endFill();

    this.init();
  }

  private init() {
    this.trisApi.getGridConstraints().then(constraints => {
      setInterval(() => {
        this.pixiApp.render();
      }, 100);
    });

    this.trisApi
      .findTriangleByLabel('1')
      .then(triangle => {
        console.log(triangle);
      })
      .catch(response => {
        console.log(response.error);
      });
  }

  private drawGrid() {}
}

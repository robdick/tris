import {
  Component,
  ViewChild,
  ElementRef,
  AfterViewInit
} from '@angular/core';
import * as Pixi from 'pixi.js';
import { TrisApiService } from 'src/services/tris-api.service';
import { Point } from 'src/models/triangle';

@Component({
  selector: 'app-grid',
  templateUrl: './grid.component.html',
  styleUrls: ['./grid.component.scss']
})
export class GridComponent implements AfterViewInit {

  @ViewChild('triscanvas', { static: false }) trisCanvas: ElementRef;

  private static GRID_VISUAL_SCALEFACTOR = 10;

  public isSearching: boolean = false;
  public buttonText = 'Start';
  public foundMessage: string;

  private pixiApp: Pixi.Application;
  private pixiStage: Pixi.Container;
  private gridContainer: Pixi.Container;
  private gridGraphics: Pixi.Graphics;

  private pointGfx: Pixi.Graphics[] = [];

  private viewWidth: number;
  private viewHeight: number;

  private constraints: any;

  constructor(private trisApi: TrisApiService) {}

  public ngAfterViewInit() {
    this.setupPixi();
    this.setupGrid();
    this.setupPointSprites();

    this.init();
  }

  public startRandomPointSearch() {
    this.isSearching = true;
    this.buttonText = 'stop';
    this.foundMessage = '';
  }

  public stopRandomPointSearch() {
    this.isSearching = false;
    this.buttonText = 'start';
  }

  public toggleRandomPointSearch() {
    if (this.isSearching) {
      this.stopRandomPointSearch();
    } else {
      this.startRandomPointSearch();
    }
  }

  private setupPixi() {
    this.viewWidth = window.innerWidth;
    this.viewHeight = window.innerHeight;

    this.pixiApp = new Pixi.Application({
      view: this.trisCanvas.nativeElement,
      width: this.viewWidth-100,
      height: this.viewHeight-100
    });

    this.pixiStage = this.pixiApp.stage;
  }

  private setupGrid() {
    const gfx = new Pixi.Graphics();

    this.gridGraphics = gfx;

    this.gridContainer = new Pixi.Container();
    this.gridContainer.position.set(30,80);

    this.gridContainer.addChild(this.gridGraphics);
    this.pixiStage.addChild(this.gridContainer);
  }

  private setupPointSprites() {
    const createGfx = () => {
      var pointGfx = new Pixi.Graphics();
      pointGfx.beginFill(0xfffff, 1);
      pointGfx.drawCircle(0, 0, 5);
      pointGfx.endFill();
      return pointGfx;
    };

    for (let i=0; i<3; i++) {
      let gfx = createGfx();
      this.gridContainer.addChild(gfx);
      this.pointGfx.push(gfx);
    }
  }

  private async init() {
    this.constraints = await this.trisApi.getGridConstraints();

    this.constraints.viewCellSpan =
      this.constraints.cellSpan * GridComponent.GRID_VISUAL_SCALEFACTOR;

    this.drawGrid(this.constraints);

    setInterval(() => {
      if (this.isSearching) {
        this.doTurn();
      }

      this.pixiApp.render();
    }, 25);
  }

  private drawGrid(constraints) {
    const gfx = this.gridGraphics;

    gfx.clear();
    gfx.lineStyle(0);

    for (let y=0; y<constraints.columnCount; y++) {
      for (let x=0; x<constraints.rowCount; x++) {
        let posx = x * constraints.viewCellSpan;
        let posy = y * constraints.viewCellSpan;

        gfx.lineStyle(0);
        gfx.beginFill(0x8888ff,1);
        gfx.drawRect(posx+1, posy+1, constraints.viewCellSpan-2, constraints.viewCellSpan-2);
        gfx.endFill();

        gfx.lineStyle(2);
        gfx.moveTo(posx, posy);
        gfx.lineTo(posx + constraints.viewCellSpan-2, posy + constraints.viewCellSpan-2);
      }
    }
  }

  private doTurn() {
    let p1 = this.randomPointOnGrid();
    let p2 = this.randomPointOnGrid();
    let p3 = this.randomPointOnGrid();

    this.pointGfx[0].position.set(p1.x * GridComponent.GRID_VISUAL_SCALEFACTOR, p1.y * GridComponent.GRID_VISUAL_SCALEFACTOR);
    this.pointGfx[1].position.set(p2.x * GridComponent.GRID_VISUAL_SCALEFACTOR , p2.y * GridComponent.GRID_VISUAL_SCALEFACTOR);
    this.pointGfx[2].position.set(p3.x * GridComponent.GRID_VISUAL_SCALEFACTOR, p3.y * GridComponent.GRID_VISUAL_SCALEFACTOR);

    this.trisApi
      .findTriangleByPoints(p1,p2,p3)
      .then(triangle => {
        this.foundMessage = `Found triangle match for points, api payload: ${JSON.stringify(triangle)}`;
        this.stopRandomPointSearch();
      });
  }

  private randomPointOnGrid(): Point {
    let rx = Math.floor(Math.random() * this.constraints.columnCount) * this.constraints.cellSpan;
    let ry = Math.floor(Math.random() * this.constraints.rowCount) * this.constraints.cellSpan;

    return {x: rx, y: ry};
  }
}



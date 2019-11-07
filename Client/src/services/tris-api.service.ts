import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../environments/environment';
import { Point } from 'src/models/triangle';

@Injectable()
export class TrisApiService {
  constructor(private httpClient: HttpClient) {}

  public getGridConstraints(): Promise<any> {
    return this.httpClient
      .get(this.endpoint('api/triangles/constraints'))
      .toPromise();
  }

  public findTriangleByLabel(label: string): Promise<any> {
    return this.httpClient
      .get(this.endpoint(`api/triangles/${label}`))
      .toPromise();
  }

  public findTriangleByPoints(p1: Point, p2: Point, p3: Point): Promise<any> {
    return this.httpClient
      .get(
        this.endpoint(
          `api/triangles/query?p1=${p1.x},${p1.y}&p2=${p2.x},${p2.y}&p3=${p3.x},${p3.y}`
        )
      )
      .toPromise();
  }

  private endpoint(url: string): string {
    return `${environment.api.host}/${url}`;
  }
}

export type Point = { x: number; y: number };

export interface Triangle {
  gridLabel: string;
  gridRow: number;
  gridColumn: number;
  p1: Point;
  p2: Point;
  p3: Point;
}

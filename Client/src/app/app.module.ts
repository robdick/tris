import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { TrisApiService } from 'src/services/tris-api.service';
import { GridComponent } from 'src/components/grid/grid.component';

@NgModule({
  declarations: [AppComponent, GridComponent],
  imports: [BrowserModule, HttpClientModule],
  providers: [TrisApiService],
  bootstrap: [AppComponent]
})
export class AppModule {}

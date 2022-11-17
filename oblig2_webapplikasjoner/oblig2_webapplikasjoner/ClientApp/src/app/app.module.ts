import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppComponent } from './app.component';


import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from './modal/modal';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';

import { NavMeny } from './navMeny/navMeny';
import { Hovedside } from './hovedside/hovedside';
import { AppRoutingModule } from './app-routing.module';




@NgModule({
  declarations: [
        AppComponent,
        Modal,
        NavMeny,
        Hovedside
  ],
  imports: [
      BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
      FormsModule,
      NgbModule,
      HttpClientModule,
      ReactiveFormsModule,
      AppRoutingModule   
  ],
    providers: [],
    bootstrap: [AppComponent],
    entryComponents: [Modal]
})
export class AppModule { }


import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Hovedside } from './hovedside/hovedside';
import { KjopSide } from './kjopSide/kjopSide';
import { Portefolje } from './portefolje/portefolje';
import { SalgSide } from './salgSide/salgSide';
import { LoggInn } from './loggInn/loggInn';



const appRoots: Routes = [
    { path: 'hovedside', component: Hovedside },
    { path: 'portefolje', component: Portefolje },
    { path: 'kjop/:id', component: KjopSide },
    { path: 'loggInn', component: LoggInn },
    { path: 'salgSide/:id', component: SalgSide },
    { path: '', redirectTo: '/loggInn', pathMatch: 'full' }
]

@NgModule({
    imports: [
        RouterModule.forRoot(appRoots)
    ],
    exports: [
        RouterModule
    ]
})
export class AppRoutingModule { }


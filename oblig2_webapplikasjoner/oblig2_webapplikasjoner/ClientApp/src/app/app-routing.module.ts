import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { Hovedside } from './hovedside/hovedside';


const appRoots: Routes = [
    { path: 'hovedside', component: Hovedside },
    { path: '', redirectTo: '/hovedside', pathMatch: 'full' }
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


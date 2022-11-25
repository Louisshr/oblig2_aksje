import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Aksje } from '../aksjerModels/Aksje'
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { Modal } from '../modal/modal';


@Component({
    selector: 'app-aksjer',
    templateUrl: './hovedside.html'    
})


export class Hovedside {
    public aksjeListe: Array<Aksje>;

    // variabler som gjelder for aksjen som handels:

    public aksjeModal: Aksje;

    constructor(private _http: HttpClient, private modalService: NgbModal, private router: Router) {
        this._http.get<Aksje[]>("api/Aksje/hentAksjer")
            .subscribe(data => {
                this.aksjeListe = data;
            },
                error => {
                    if (error.status == 401) {
                        this.router.navigate(['/loggInn']);
                    }
                }
            )
    }    

    loggUt() {
        this._http.get("api/Aksje/loggUt")
            .subscribe(data => {
                this.router.navigate(['/loggInn']);
            })
    }

    
}
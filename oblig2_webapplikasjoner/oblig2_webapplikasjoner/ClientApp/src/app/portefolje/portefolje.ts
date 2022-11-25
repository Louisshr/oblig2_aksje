import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Kjop } from "../aksjerModels/Kjop";
import { Router } from '@angular/router';

@Component({
    selector: 'app-portefolje',
    templateUrl: './portefolje.html'
})

export class Portefolje {
    public portefoljeListe: Array<Kjop>;
    public saldoPortefolje: string;

    constructor(private _http: HttpClient, private router: Router) {
        this._http.get<number>("api/Aksje/hentSaldo")
            .subscribe(saldo => {
                if (saldo != -1) {
                    this.saldoPortefolje = saldo.toFixed(2) + " USD";
                }
                else {
                    this.saldoPortefolje = "Det har oppstått en feil, saldo kan ikke hentes";
                }
            },
                error => {
                    if (error.status == 401) {
                        this.router.navigate(['/loggInn']);
                    }
                }
            )


        this._http.get<Kjop[]>("api/Aksje/hentPortefolje")
            .subscribe(data => {
                this.portefoljeListe = data;
            })

    }
}

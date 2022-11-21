import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Kjop } from "../aksjerModels/Kjop";

@Component({
    selector: 'app-portefolje',
    templateUrl: './portefolje.html'
})

export class Portefolje {
    public portefoljeListe: Array<Kjop>;
    public saldoPortefolje: string;

    constructor(private _http: HttpClient) {
        this._http.get<number>("api/Aksje/hentSaldo")
            .subscribe(saldo => {
                if (saldo != -1) {
                    this.saldoPortefolje = saldo.toString() + " USD";
                }
                else {
                    this.saldoPortefolje = "Det har oppstått en feil, saldo kan ikke hentes";
                }
            })


        this._http.get<Kjop[]>("api/Aksje/hentPortefolje")
            .subscribe(data => {
                this.portefoljeListe = data;
            })

    }
}

// innfør denne

/*
 * this._http.get<number>("api/Person/getCounter")
            .subscribe(dataId => {
                console.log(dataId);
            })
 */

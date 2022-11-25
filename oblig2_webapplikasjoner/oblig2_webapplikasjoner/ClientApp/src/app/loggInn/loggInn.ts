import { Component } from "@angular/core";
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Bruker } from "../aksjerModels/Bruker";

@Component({
    selector: 'app-loggInn',
    templateUrl: './loggInn.html'
})


export class LoggInn {
    public LoggInnSkjema: FormGroup;
    public feilPassord: string;

    validering = {
        inputBrukernavn: [
            null, Validators.compose([Validators.required, Validators.pattern("[a-zA-ZæøåÆØÅ. \-]{2,20}")])
        ],
        inputPassord: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9A-Za-zæøåÆØÅ \d]{6,20}")])
        ]
    }

    constructor(private fb: FormBuilder, private _http: HttpClient, private router: Router) {
        this.LoggInnSkjema = fb.group(this.validering);
    }

    onSubmit() {
        const enBruker = new Bruker();
        enBruker.Brukernavn = this.LoggInnSkjema.value.inputBrukernavn;
        enBruker.Passord = this.LoggInnSkjema.value.inputPassord;

        this._http.post("api/Aksje/loggInn", enBruker)
            .subscribe(data => {
                if (data) {
                    this.feilPassord = "";
                    this.router.navigate(['/hovedside']);
                }
                else {
                    this.feilPassord = "Inloggings informasjon er ikke korrekt";
                }
            })
    }
}


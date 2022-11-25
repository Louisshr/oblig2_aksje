import { Component, OnInit } from "@angular/core";
import { HttpClient } from '@angular/common/http';
import { Modal } from '../modal/modal';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Aksje } from '../aksjerModels/Aksje';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators, FormBuilder } from '@angular/forms';
import { Salg } from '../aksjerModels/Salg';



@Component({
    selector: 'app-kjop',
    templateUrl: './kjopSide.html'
})

export class KjopSide {
    public id: number;
    public aksjeNavnBetaling: string;
    public aksjeBetaling: Aksje;
    public Skjema: FormGroup;

    validering = {
        inputAntall: [
            null, Validators.compose([Validators.required, Validators.pattern("[0-9]{1,10}")])
        ]
    }

    constructor(private _http: HttpClient, private fb: FormBuilder,
        private route: ActivatedRoute, private router: Router, private modalService: NgbModal) {

        this.Skjema = fb.group(this.validering);
    }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.id = params.id
            this._http.get<Aksje>("api/Aksje/hentEn" + this.id)
                .subscribe(data => {
                    this.aksjeBetaling = data;
                    this.aksjeNavnBetaling = "Kjøp " + "(" + this.aksjeBetaling.navn + ")";
                })
        })
    }

    onSubmit() {
        const modalRef = this.modalService.open(Modal, {
            backdrop: 'static',
            // betyr at man ikke kan klikke vekk modalen ved å trykke andre steder

            keyboard: false
            // betyr at man ikke kan klikke vekk modalen med ESC

        });

        const antall = Number(this.Skjema.value.inputAntall);

        const etSalg = new Salg();
        etSalg.aksjeId = this.id;
        etSalg.antall = antall


        this._http.post<Boolean>("api/Aksje/kjopAksje", etSalg)
            .subscribe(retur => {
                if (retur) {
                    const totalpris = antall * this.aksjeBetaling.verdi;

                    modalRef.componentInstance.headerModal = "Kvittering";
                    modalRef.componentInstance.headerBodyModal = "Handel er gjennomført. Kjøp er lagt til i din portefølje.";

                    modalRef.componentInstance.aksjeHandletModal = "Aksje handlet: " + this.aksjeBetaling.navn;
                    modalRef.componentInstance.antallAksjerModal = "Antall: " + antall;
                    modalRef.componentInstance.totalprisModal = "Totalpris: " + totalpris.toFixed(2) + " $";
                    modalRef.componentInstance._btnLabel = "Ok";
                }
                else {
                    modalRef.componentInstance.headerModal = "Feil";
                    modalRef.componentInstance.headerBodyModal = "En feil har oppstått. Kjøpet er avbrutt.";

                    modalRef.componentInstance.aksjeHandletModal = "";
                    modalRef.componentInstance.antallAksjerModal = "";
                    modalRef.componentInstance.totalprisModal = "";
                    modalRef.componentInstance._btnLabel = "Ok";
                }

            },
                error => console.log(error)
            );

        modalRef.result.then(retur => {
            // may be necesarry
            // this.aksjeBetaling = null;

            this.router.navigate(['/hovedside']);
        })
    }


}

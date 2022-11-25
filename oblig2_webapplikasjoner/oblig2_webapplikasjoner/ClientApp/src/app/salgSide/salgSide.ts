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
    selector: 'app-salg',
    templateUrl: './salgSide.html'
})

export class SalgSide {
    public id: number;
    public aksjeNavnSalg: string;
    public Skjema: FormGroup;
    public aksjeSalg: Aksje;

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
            this.id = params.id;

            this._http.get<Aksje>("api/Aksje/hentEn" + this.id)
                .subscribe(data => {
                    this.aksjeSalg = data;
                    this.aksjeNavnSalg = "Selg " + "(" + this.aksjeSalg.navn + ")";
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
        etSalg.antall = antall;

        this._http.post<Boolean>("api/Aksje/selg", etSalg)
            .subscribe(retur => {
                if (retur) {
                    const totalpris = antall * this.aksjeSalg.verdi;
                    modalRef.componentInstance.headerModal = "Kvittering";
                    modalRef.componentInstance.headerBodyModal = "Salg er gjennomført.";

                    modalRef.componentInstance.aksjeHandletModal = "Aksje solgt: " + this.aksjeSalg.navn;
                    modalRef.componentInstance.antallAksjerModal = "Antall: " + antall;
                    modalRef.componentInstance.totalprisModal = "Totalpris for salg: " + totalpris.toFixed(2) + " $";
                    modalRef.componentInstance._btnLabel = "Ok";
                }
                else {
                    modalRef.componentInstance.headerModal = "Feil";
                    modalRef.componentInstance.headerBodyModal = "En feil har oppstått. Salget er avbrutt.";

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

            this.router.navigate(['/portefolje']);
        })
    }
}



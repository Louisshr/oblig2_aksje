import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Aksje } from '../aksjerModels/Aksje'
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Modal } from '../modal/modal';


@Component({
    selector: 'app-aksjer',
    templateUrl: './hovedside.html'    
})


export class Hovedside {
    public aksjeListe: Array<Aksje>;

    // variabler som gjelder for aksjen som handels:

    public aksjeModal: Aksje;

    constructor(private _http: HttpClient, private modalService: NgbModal) {
        this._http.get<Aksje[]>("api/Aksje/hentAksjer")
            .subscribe(data => {
                this.aksjeListe = data;
            })
    }

    visModal(id) {
        const modalRef = this.modalService.open(Modal, {
            backdrop: 'static',
            // betyr at man ikke kan klikke vekk modalen ved å trykke andre steder

            keyboard: false
            // betyr at man ikke kan klikke vekk modalen med ESC

        });


        this._http.get<Aksje>("api/Aksje/hentEn" + id)
            .subscribe(data => {
                if (data != null) {
                    this.aksjeModal = data;
                    modalRef.componentInstance.aksjeNavnModal = this.aksjeModal.navn;
                }
                else {
                    this.aksjeModal = null;
                    modalRef.componentInstance.aksjeNavnModal = "Feil";
                    modalRef.componentInstance.feilmeldingModal = "Det har opstått en feil";
                }

                // må håndtere error

            })

        modalRef.result.then(retur => {
            console.log('ID:' + id);
        });
    }

}
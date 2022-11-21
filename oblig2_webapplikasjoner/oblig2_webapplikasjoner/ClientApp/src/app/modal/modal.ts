import { Component, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
    templateUrl: 'modal.html'
})


export class Modal {
    public _btnLabel: string = '';

    @Input()
    set btnLabel(value) {
        this._btnLabel = value;
    }

    constructor(public modal: NgbActiveModal) { }
}
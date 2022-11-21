import { Kjop } from "./Kjop";
import { Portefolje } from "./Portefolje";

export class Person {
    public id: number;
    public fornavn: string;
    public etternavn: string;
    public saldo: number;
    public kjop: Array<Kjop>;
    public portefolje: Portefolje;
}


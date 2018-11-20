import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { IAppConfig } from '../_models';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable()
 export class AppConfig {
    private jsonFile=`./assets/config.${environment.name}.json`;
    static settings: IAppConfig;
    static httpOptions: any;

    constructor(private http: HttpClient){}

    load() {
        return new Promise<void>((resolve, reject) => {
            this.http.get<IAppConfig>(this.jsonFile).toPromise().then(response => {
                AppConfig.settings = response;
                resolve();
            }).catch((response: any) => {
                reject(`Couldnot load file '${this.jsonFile}': ${JSON.stringify(response)} `);
            });
        });
    }
}
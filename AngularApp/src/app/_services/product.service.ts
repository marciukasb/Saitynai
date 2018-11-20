import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {  } from '@angular/common/http';
import * as Rx from 'rxjs';
import { AppConfig } from '../_services/configuration.service'
import { Product } from '../_models';

@Injectable()
export class ProductService {
    constructor(private http: HttpClient) { }

    GetAllProducts() : Rx.Observable<Product[]> {
        return this.http.get<Product[]>(`${AppConfig.settings.hostname}/product/`);
    }

    DeleteProduct(id : string) {
        return this.http.delete(`${AppConfig.settings.hostname}/product/${id}`);
    }
}
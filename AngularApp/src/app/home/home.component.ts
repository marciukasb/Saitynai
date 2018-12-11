import { Component, OnInit } from '@angular/core';

import { User, Product } from '../_models';
import { ProductService, AuthenticationService } from '../_services';
import * as jwt_decode from "jwt-decode";

@Component({templateUrl: 'home.component.html', 
styles: ['h1 { font-weight: normal; }']
})
export class HomeComponent implements OnInit {
    currentUser: string;
    currentToken: string;
    products: Product[] = [];

    constructor(private authenticationService: AuthenticationService, private productService: ProductService) {
        this.currentUser = this.getDecodedAccessToken(JSON.parse(localStorage.getItem('currentUser')).Token);
        this.currentToken = JSON.parse(localStorage.getItem('currentUser')).Token;
    }

    ngOnInit() {

        if(!this.currentUser) {
            this.authenticationService.logout();
        }
        this.productService.GetAllProducts().subscribe(products => { this.products = products; });
    }

    deleteProduct(id : string){
        this.productService.DeleteProduct(id).subscribe(() => { this.products = this.products.filter(obj => obj.Id !== id)});
    }

    copyMessage(val: string){
        const selBox = document.createElement('textarea');
        selBox.style.position = 'fixed';
        selBox.style.left = '0';
        selBox.style.top = '0';
        selBox.style.opacity = '0';
        selBox.value = `Bearer ${this.currentToken}`;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
      }

      getDecodedAccessToken(token: string): any {
        debugger;
        try{
            return jwt_decode(token);
        }
        catch(Error){
            return null;
        }
      }
}
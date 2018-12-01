import { Component, OnInit } from '@angular/core';

import { User, Product } from '../_models';
import { ProductService, AuthenticationService } from '../_services';

@Component({templateUrl: 'home.component.html', 
styles: ['h1 { font-weight: normal; }']
})
export class HomeComponent implements OnInit {
    currentUser: User;
    products: Product[] = [];

    constructor(private authenticationService: AuthenticationService, private productService: ProductService) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
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
        selBox.value = `Bearer ${this.currentUser.Token}`;
        document.body.appendChild(selBox);
        selBox.focus();
        selBox.select();
        document.execCommand('copy');
        document.body.removeChild(selBox);
      }
}
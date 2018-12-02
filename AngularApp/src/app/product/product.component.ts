import { Component, OnInit } from '@angular/core';

import { User, Product } from '../_models';
import { ProductService, AuthenticationService } from '../_services';

@Component({templateUrl: 'product.component.html'})
export class ProductComponent implements OnInit {
    currentUser: User;
    products: Product[] = [];

    constructor(private authenticationService: AuthenticationService, private productService: ProductService) {
      //  this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    ngOnInit() {
        this.authenticationService.GetRights().subscribe(res => this.currentUser.Admin = res.Admin);
        localStorage.setItem("perviousUrl", "product")
        this.productService.GetAllProducts().subscribe(products => { this.products = products; });
    }

    create() {
        debugger;
    }

    delete(id : string){
        debugger;
    //    this.productService.DeleteProduct(id).subscribe(() => { this.products = this.products.filter(obj => obj.Id !== id)});
    }

    edit(id : string) {
        debugger;
    }
}
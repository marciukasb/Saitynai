import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { User, Product } from '../_models';
import { ProductService, AuthenticationService } from '../_services';

@Component({templateUrl: 'product.component.html'})
export class ProductComponent implements OnInit {
    currentUser: User;
    products: Product[] = [];

    constructor(private authenticationService: AuthenticationService, private productService: ProductService, private router: Router) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
        this.currentUser.Admin = false;
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
        this.productService.DeleteProduct(id).subscribe(() => { this.products = this.products.filter(obj => obj.Id !== id)});
    }

    openEdit(id : string) {
        debugger;
        this.router.navigate([`edit-product/${id}`]);
    }
}
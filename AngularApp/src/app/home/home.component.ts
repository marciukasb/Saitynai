import { Component, OnInit } from '@angular/core';

import { User, Product } from '../_models';
import { UserService, ProductService } from '../_services';

@Component({templateUrl: 'home.component.html'})
export class HomeComponent implements OnInit {
    currentUser: User;
    products: Product[] = [];

    constructor(private userService: UserService, private productService: ProductService) {
        debugger;
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    ngOnInit() {
        this.productService.GetAllProducts().subscribe(products => { this.products = products; });
    }

    deleteProduct(id : string){
        this.productService.DeleteProduct(id).subscribe(() => { this.products = this.products.filter(obj => obj.Id != id)});
    }
}
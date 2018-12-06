import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import * as jwt_decode from "jwt-decode";

import { AlertService, AuthenticationService, ProductService } from '../_services';
import { User, Product } from '../_models';

@Component({templateUrl: 'editProduct.component.html'})
export class EditProductComponent implements OnInit {
    currentUser: User;
    createForm: FormGroup;
    Price: number;
    loading = false;
    submitted = false;
    product: Product;

    constructor(
        private formBuilder: FormBuilder,
        private productService: ProductService,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private router: Router) { }

    ngOnInit() {
        this.productService.GetProduct(this.route.snapshot.paramMap.get('id')).subscribe(product => 
            { 
                debugger;
                this.Price = product.Price;
                this.createForm.setValue({
                    Id: this.route.snapshot.paramMap.get('id'),
                    Name: product.Name,
                    Brand: product.Brand,
                    Price: product.Price
                })
            });
        this.createForm = this.formBuilder.group({
            Id: [],
            Name: ['', Validators.required],
            Brand: ['', Validators.required],
            Price: ['', Validators.required]
        });
    }

    get f() { return this.createForm.controls; }

    onSubmit() {
        this.submitted = true;

        if (this.createForm.invalid) {
            return;
        }

        this.loading = true;
        this.productService.UpdateProduct(this.createForm.value)
            .pipe(first())
            .subscribe(
                () => {
                    this.alertService.success('Updated successfuly', true);
                    this.router.navigate(['/product']);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }

    replace(){
        this.Price = this.createForm.value.Price.replace(",", ".");
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

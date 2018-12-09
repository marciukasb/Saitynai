import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import * as jwt_decode from "jwt-decode";

import { AlertService, AuthenticationService, ProductService } from '../_services';
import { User } from '../_models';

@Component({templateUrl: 'createProduct.component.html'})
export class CreateProductComponent implements OnInit {
    currentUser: User;
    createForm: FormGroup;
    imagePath = "https://store-cdn.arduino.cc/usa/catalog/product/cache/1/image/520x330/604a3538c15e081937dbfbd20aa60aad/a/0/a000066_featured_4.jpg";
    Price: number;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private productService: ProductService,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createForm = this.formBuilder.group({
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
        this.productService.CreateProduct(this.createForm.value)
            .pipe(first())
            .subscribe(
                () => {
                    this.alertService.success('Created successfuly', true);
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

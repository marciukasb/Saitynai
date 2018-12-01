import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AlertService, UserService, ProductService } from '../_services';

@Component({templateUrl: 'createProduct.component.html'})
export class CreateProductComponent implements OnInit {
    createForm: FormGroup;
    price: number;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private productService: ProductService,
        private alertService: AlertService) { }

    ngOnInit() {
        this.createForm = this.formBuilder.group({
            name: ['', Validators.required],
            brand: ['', Validators.required],
            price: ['', Validators.required]
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
        this.price = this.createForm.value.price.replace(",", ".");
    }
}

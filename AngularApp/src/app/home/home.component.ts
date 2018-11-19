import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs/operators';

import { User } from '../_models';
import { UserService } from '../_services';

@Component({templateUrl: 'home.component.html'})
export class HomeComponent implements OnInit {
    currentUser: string;
    users: User[] = [];

    constructor(private userService: UserService) {
        this.currentUser = localStorage.getItem('currentUser');
    }

    ngOnInit() {
    }
}
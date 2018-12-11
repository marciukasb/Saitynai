import { Component } from '@angular/core';
import { User } from '../app/_models';
import './_content/modal.less';

@Component({
    selector: 'app',
    templateUrl: 'app.component.html'
})

export class AppComponent { 
    currentUser: User;
    constructor() {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }
}
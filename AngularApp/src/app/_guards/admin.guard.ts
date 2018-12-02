import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models';


@Injectable()
export class AdminGuard implements CanActivate {
    currentUser: User;
    constructor(private router: Router) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (localStorage.getItem('currentUser')) {
            this.currentUser = <User>JSON.parse(localStorage.getItem('currentUser'));
            if(this.currentUser.Admin == true){
                // logged in so return true
                return true;
            }
        }
        let url = localStorage.getItem("perviousUrl");
        localStorage.removeItem("perviousUrl");
        this.router.navigate([url], { queryParams: { returnUrl: state.url }});

        return false;
    }
}
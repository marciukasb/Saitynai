import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { User } from '../_models';
import { AlertService, AuthenticationService, ProductService } from '../_services';
@Injectable()
export class AdminGuard implements CanActivate {
    currentUser: User;
    constructor(private router: Router, private authenticationService:AuthenticationService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        this.authenticationService.Authorize();
        return false;
    }
}
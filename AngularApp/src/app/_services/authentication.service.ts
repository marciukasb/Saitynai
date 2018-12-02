import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, first } from 'rxjs/operators';
import { User } from '../_models/user';
import { Router } from '@angular/router';


import { AppConfig } from '../_services/configuration.service';

@Injectable()
export class AuthenticationService {
    public user: User;
    constructor(private http: HttpClient, private router: Router) { }
    admin: boolean = false;

    login(username: string, password: string) {
        debugger;
        return this.http.post<User>(`${AppConfig.settings.hostname}/user/authenticate`, { Username: username, Password: password })
            .pipe(map(user => {
                if (user.Token) {
                    debugger;
                    this.user = new User(username, user.Token, user.Admin);
                    localStorage.setItem('currentUser', JSON.stringify(this.user));
                }

                return user;
            }));
    }

    logout() {
        localStorage.removeItem('currentUser');
    }


    Authorize() {
        return this.http.get(`${AppConfig.settings.hostname}/user/authorize`).pipe(first())
            .subscribe(
                () => {
                    return true;
                },
                (error) => {
                    let url = localStorage.getItem("perviousUrl");
                    localStorage.removeItem("perviousUrl");
                    this.router.navigate([url]);
                    return false;
                }
            );
    }
}
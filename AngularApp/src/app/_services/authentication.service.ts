import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { AppConfig } from '../_services/configuration.service'

@Injectable()
export class AuthenticationService {
    private user: User;
    constructor(private http: HttpClient) { }

    login(username: string, password: string) {
        debugger;
        return this.http.post<User>(`${AppConfig.settings.hostname}/user/authenticate`, { Username: username, Password: password })
            .pipe(map(user => {
                debugger;
                this.user = new User(username, user.Token);
                // login successful if there's a jwt token in the response
                if (user.Token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(this.user));
                }

                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }
}
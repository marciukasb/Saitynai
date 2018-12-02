import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { User } from '../_models/user';
import { AppConfig } from '../_services/configuration.service'

@Injectable()
export class AuthenticationService {
    public user: User;
    constructor(private http: HttpClient) { }

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
}
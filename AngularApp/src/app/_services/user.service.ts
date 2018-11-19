import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {  } from '@angular/common/http';
import * as Rx from 'rxjs';



import { environment } from '../../environments/environment';
import { User } from '../_models';

@Injectable()
export class UserService {
    constructor(private http: HttpClient) { }

    register(user: User) : Rx.Observable<User> {
        return this.http.post<User>(`${environment.apiUrl}/user/register`, user);
    }

   
}
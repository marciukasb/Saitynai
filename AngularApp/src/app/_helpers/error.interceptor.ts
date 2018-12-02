import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticationService } from '../_services';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    constructor(private authenticationService: AuthenticationService) {}

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(err => {
            debugger;
            if (err.status === 403) {
                this.authenticationService.logout();
                location.reload(true);
            }
            if (err.status === 0) {
                err.statusText = "Could not get a response";
            }
            if(err.message.toString().includes("setRequestHeader") ){
                this.authenticationService.logout();
                location.reload(true);
            }
            debugger;
            const error = err.statusText;
            return throwError(error);
        }))
    }
}
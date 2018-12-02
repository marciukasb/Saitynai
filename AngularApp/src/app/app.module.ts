import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule }    from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppComponent }  from './app.component';
import { routing }        from './app.routing';
import { AlertComponent } from './_directives';
import { AuthGuard, AdminGuard } from './_guards';
import { JwtInterceptor, ErrorInterceptor } from './_helpers';
import { AlertService, AuthenticationService, UserService, AppConfig, ProductService } from './_services';
import { HomeComponent } from './home';
import { LoginComponent } from './login';
import { ProductComponent } from './product';
import { CreateProductComponent } from './createProduct';

import { RegisterComponent } from './register';
import { APP_INITIALIZER } from '@angular/core';

export function initializeApp(appConfig: AppConfig) {
    return () => appConfig.load();
}

@NgModule({
    imports: [
        BrowserModule,
        ReactiveFormsModule,
        HttpClientModule,
        routing
    ],
    declarations: [
        AppComponent,
        AlertComponent,
        HomeComponent,
        LoginComponent,
        RegisterComponent,
        ProductComponent,
        CreateProductComponent
    ],
    providers: [
        AuthGuard,
        AdminGuard,
        AlertService,
        AuthenticationService,
        UserService,
        ProductService,
        AppConfig,
        {
            provide: APP_INITIALIZER,
            useFactory: initializeApp,
            deps: [AppConfig], multi: true
        },
        { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }

    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
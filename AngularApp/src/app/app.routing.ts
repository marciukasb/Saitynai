import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { ProductComponent } from './product';
import { CreateProductComponent } from './createProduct';
import { LoginComponent } from './login';
import { RegisterComponent } from './register';
import { AuthGuard } from './_guards';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'product', component: ProductComponent },
    { path: 'create-product', component: CreateProductComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);
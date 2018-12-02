import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './home';
import { ProductComponent } from './product';
import { CreateProductComponent } from './createProduct';
import { LoginComponent } from './login';
import { RegisterComponent } from './register';
import { AuthGuard, AdminGuard } from './_guards';

const appRoutes: Routes = [
    { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'product', component: ProductComponent, canActivate: [AuthGuard] },
    { path: 'create-product', component: CreateProductComponent, canActivate: [AdminGuard]  },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);
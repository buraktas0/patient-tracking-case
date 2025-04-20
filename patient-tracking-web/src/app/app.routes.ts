import { Routes } from '@angular/router';
import { LayoutComponent } from './common/layout/layout.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { ForgotPasswordComponent } from './pages/auth/forgot-password/forgot-password.component';
import { RegisterComponent } from './pages/auth/register/register.component';
import { PatientsComponent } from './pages/patients/patients.component';
import { PatientDetailComponent } from './pages/patients/detail/patient-detail.component';
import { PatientCreateComponent } from './pages/patients/create/patient-create.component';
import { AuthGuard } from './auth/auth.guard';

export const routes: Routes = [
    {
        path: '', component: LayoutComponent,
        children: [
            { path: 'login', component: LoginComponent },
            { path: 'forgot-password', component: ForgotPasswordComponent },
            { path: 'register', component: RegisterComponent },
            { path: 'patients', component: PatientsComponent, canActivate: [AuthGuard] },
            { path: 'patients/create', component: PatientCreateComponent, canActivate: [AuthGuard] },
            { path: 'patients/edit/:id', component: PatientCreateComponent, canActivate: [AuthGuard] },
            { path: 'patients/:id', component: PatientDetailComponent, canActivate: [AuthGuard] },
        ]
    },
];

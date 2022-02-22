import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ComplateSignupComponent } from './components/pages/complate-signup/complate-signup.component';
import { HomeComponent } from './components/pages/home/home.component';
import { UsersComponent } from './components/pages/users/users.component';
import { AuthorizeGuard } from './guards/authorize.guard';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: 'complate-signup', component: ComplateSignupComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthorizeGuard] },
  { path: 'users', component: UsersComponent, canActivate: [AuthorizeGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

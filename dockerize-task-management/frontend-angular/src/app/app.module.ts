import { HttpErrorInterceptor } from './services/common/http-error-interceptor';
import { ComplateSignupComponent } from './components/pages/complate-signup/complate-signup.component';
import { TaskAddMemberComponent } from './components/task-addmember/task-addmember.component';
import { TaskCreateComponent } from './components/task-create/task-create.component';
import { TaskComponent } from './components/task/task.component';
import { UsersComponent } from './components/pages/users/users.component';
import { ProfileComponent } from './components/pages/profile/profile.component';
import { UserSignupTicketComponent } from './components/user-signup-ticket/user-signup-ticket.component';
import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/pages/home/home.component';
import { HeaderComponent } from './components/header/header.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserFilterPipe } from './pipes/user-filter.pipe';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthorizeGuard } from './guards/authorize.guard';
import { BoardCreateComponent } from './components/board-create/board-create.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    TaskComponent,
    TaskCreateComponent,
    TaskAddMemberComponent,
    BoardCreateComponent,
    UserSignupTicketComponent,
    ProfileComponent,
    HeaderComponent,
    UserSignupTicketComponent,
    UsersComponent,
    ComplateSignupComponent,
    UserFilterPipe
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorInterceptor,
    multi: true
  }, AuthorizeGuard],
  bootstrap: [AppComponent],
  schemas:[NO_ERRORS_SCHEMA]
})
export class AppModule { }

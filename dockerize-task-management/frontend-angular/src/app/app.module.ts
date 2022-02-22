import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpErrorInterceptor } from './services/common/http-error-interceptor';
import { LoginComponent } from './components/login/login.component';
import { HomeComponent } from './components/pages/home/home.component';
import { AuthorizeGuard } from './guards/authorize.guard';
import { HeaderComponent } from './components/header/header.component';
import { TaskComponent } from './components/task/task.component';
import { TaskCreateComponent } from './components/task-create/task-create.component';
import { BoardCreateComponent } from './components/board-create/board-create.component';
import { TaskAddMemberComponent } from './components/task-addmember/task-addmember.component';
import { UserFilterPipe } from './pipes/user-filter.pipe';
import { UsersComponent } from './components/pages/users/users.component';

@NgModule({
  declarations: [
    UserFilterPipe,
    AppComponent,
    LoginComponent,
    HeaderComponent,
    TaskComponent,
    TaskCreateComponent,
    TaskAddMemberComponent,
    BoardCreateComponent,
    HomeComponent,
    UsersComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [{
    provide: HTTP_INTERCEPTORS,
    useClass: HttpErrorInterceptor,
    multi: true
  }, AuthorizeGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }

import { Component, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';
import { LoginUser } from 'src/app/models/login-user';
import { AccountService } from 'src/app/services/account.service';
import { NotifyService } from 'src/app/services/common/notify.service';
import { StorageService } from 'src/app/services/common/storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit, OnChanges {

  user: LoginUser = {
    userName: "kamran_aeff@mail.ru",
    password: "123456"
  } as LoginUser;

  constructor(private notify: NotifyService,
    private accountService: AccountService,
    private storage: StorageService,
    private router: Router) { }

  ngOnChanges(changes: SimpleChanges): void {
    this.accountService.isLoggedIn();
  }

  ngOnInit() {
  }

  onSubmit(event: any) {
    this.accountService.login(this.user).subscribe(response => {
      if (response.error) {

        this.notify.error(response.message);
      }
      else {
        this.storage.setValue('token', response.data.token);
        this.notify.success(response.message);
        window.location.reload();
      }
    });
  }

}

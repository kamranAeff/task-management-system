import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UserSignupComplate } from 'src/app/models/user-signup-complate';
import { AccountService } from 'src/app/services/account.service';
import { NotifyService } from 'src/app/services/common/notify.service';

@Component({
  selector: 'app-complate-signup',
  templateUrl: './complate-signup.component.html',
  styleUrls: ['./complate-signup.component.scss']
})
export class ComplateSignupComponent implements OnInit {
  user: UserSignupComplate = {
    email: 'akamran@code.az',
    password: '123456',
    passwordConfirm: '123456'
  } as UserSignupComplate;
  token: string = "";

  constructor(private route: ActivatedRoute, private router: Router, private accountService: AccountService,
    private notify: NotifyService) { }

  ngOnInit() {
    this.route.queryParams.subscribe((params: any) => {
      this.token = params.token;
    });
  }

  onSubmit(event: any) {
    this.accountService.complateSignup(this.user, this.token).subscribe(response => {
      if (response.error) {
        this.notify.error(response.message);
        return;
      }

      this.notify.success(response.message);
      location.pathname = '/';
    });

  }
}

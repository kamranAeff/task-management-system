import { Component } from '@angular/core';
import { LoginUser } from './models/login-user';
import { AccountService } from './services/account.service';
import { NotifyService } from './services/common/notify.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'frontend-angular';

  constructor(private notify: NotifyService, public accountService: AccountService) { }

  onMessage() {
    this.notify.confirm("Razisan?", "oke?", () => {

      this.accountService.login({
        userName: "kamran_aeff222@mail.ru",
        password: "123456"
      } as LoginUser).subscribe(data => {
        if (data.error) {

          this.notify.error(data.message);
        }
        else {
          this.notify.success(data.message);
        }

        console.log(data);
      });



    });

    this.notify.message("oke...");
    this.notify.error("this is error this is error this is error this is error ");
    this.notify.warning("this is warning this is warning this is warning this is warning ");
    this.notify.success("this is success this is success this is success this is success ");
  }
}

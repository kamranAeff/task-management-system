import { AccountService } from './services/account.service';
import { NotifyService } from './services/common/notify.service';
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'frontend-angular';
  constructor(private notify: NotifyService, public accountService: AccountService) { }
}

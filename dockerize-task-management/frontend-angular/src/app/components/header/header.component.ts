import { Component, Input, OnInit } from '@angular/core';
import { AccountInfo } from 'src/app/models/account-details';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  @Input() accountInfo: AccountInfo | undefined;

  constructor(private accountService: AccountService) { }

  ngOnInit() {
  }

  logout() {
    this.accountService.logout();
  }

}

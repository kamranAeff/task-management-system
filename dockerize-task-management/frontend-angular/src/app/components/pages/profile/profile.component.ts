import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/services/account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(public accountService: AccountService) { }

  ngOnInit() {
    this.accountService.fillInfo();

    console.log(this.accountService.accountDetails.organisations[0]);
  }

}

import { Component, OnInit } from '@angular/core';
import { AccountInfo } from 'src/app/models/account-details';
import { AccountService } from 'src/app/services/account.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  users: AccountInfo[] = [];

  constructor(public accountService: AccountService,
    private userService: UsersService) { }

  ngOnInit() {

    this.accountService.fillInfo(() => {
      this.userService.getAll().subscribe(response => {
        this.users = response;

        console.log(response);
      });
    });

  }

  setRole(userId: number, role: string) {
    this.userService.setUserRole(userId, role).subscribe(response => {

    });
  }

}

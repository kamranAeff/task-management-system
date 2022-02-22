import { Component, OnInit, ViewChild } from '@angular/core';
import { AccountInfo } from 'src/app/models/account-details';
import { AccountService } from 'src/app/services/account.service';
import { NotifyService } from 'src/app/services/common/notify.service';
import { UsersService } from 'src/app/services/users.service';
import { UserSignupTicketComponent } from '../../user-signup-ticket/user-signup-ticket.component';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss']
})
export class UsersComponent implements OnInit {
  @ViewChild('ticket') signupTicket!: UserSignupTicketComponent;
  users: AccountInfo[] = [];

  constructor(public accountService: AccountService,
    private userService: UsersService,
    private notify: NotifyService) { }

  ngOnInit() {

    this.accountService.fillInfo(() => {
      this.userService.getAll().subscribe(response => {
        this.users = response;

        console.log(response);
      });
    });

  }

  setRole(userId: number, role: string) {
    console.log(userId, role);
    this.userService.setUserRole(userId, role).subscribe(response => {

      if (response.error) {
        this.notify.error(response.message);
        return;
      }
      this.notify.success(response.message);

      this.userService.getAll().subscribe(response => {
        this.users = response;
      });
    });
  }

  createNewTicket(event: any) {
    event.state = true;
    this.signupTicket.ngChangeVisible(event)
  }

  onTicketDone(event: any) {
    document.body.classList.remove('overlay');
  }

  onTicketClose(event: any) {
    document.body.classList.remove('overlay');
  }

}

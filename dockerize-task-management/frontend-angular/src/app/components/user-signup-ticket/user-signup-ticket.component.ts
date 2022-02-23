import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { UserSignupTicket } from 'src/app/models/user-signup-ticket';
import { AccountService } from 'src/app/services/account.service';
import { NotifyService } from 'src/app/services/common/notify.service';

@Component({
  selector: 'app-user-signup-ticket',
  templateUrl: './user-signup-ticket.component.html',
  styleUrls: ['./user-signup-ticket.component.scss']
})
export class UserSignupTicketComponent implements OnInit {
  @ViewChild("createNewUser") createNewUser!: ElementRef;
  @Output() done: EventEmitter<any> = new EventEmitter();
  @Output() close: EventEmitter<any> = new EventEmitter();
  user: UserSignupTicket = {} as UserSignupTicket;

  constructor(private notify: NotifyService,
 public accountService: AccountService) { }


  ngOnInit() {
  }

  ngChangeVisible(event: any) {

    if (event.state) {
      this.accountService.fillInfo();
      this.createNewUser.nativeElement.children[0].reset();
      this.createNewUser.nativeElement.classList.add('open');
      document.body.classList.add('overlay');

    }
    else {
      this.createNewUser.nativeElement.classList.remove('open');
      document.body.classList.remove('overlay');
    }
  }

  onClose(event: any) {
    event.state = false;
    this.ngChangeVisible(event);
    this.close.emit(event);
  }

  onSubmit(event: any) {
    this.accountService.sendSignupTicket(this.user)
      .subscribe((response:any) => {

        if (response.error) {

          this.notify.error(response.message);
          return;
        }

        this.notify.success(response.message);
        event.response = response;
        event.state = false;
        this.ngChangeVisible(event);
        this.done.emit(event);
      });
  }

}

import { Component, ElementRef, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { UserChoose } from 'src/app/models/user-choose';
import { NotifyService } from 'src/app/services/common/notify.service';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task-addmember',
  templateUrl: './task-addmember.component.html',
  styleUrls: ['./task-addmember.component.scss']
})
export class TaskAddMemberComponent implements OnInit {
  @ViewChild('addmember') addmember!: ElementRef;
  @Output() onClose: EventEmitter<any> = new EventEmitter();

  findText: string = "";
  taskId!: number;
  users: UserChoose[] = [];
  constructor(private taskService: TaskService, private notify: NotifyService) { }

  ngOnInit() {
  }

  ngChangeVisible(event: any) {
    if (event.state) {
      this.taskId = event.taskId;
      this.taskService.getMembers(this.taskId)
        .subscribe(response => {
          this.users = response;
          this.addmember.nativeElement.classList.add('open');
          document.body.classList.add('overlay');
        });

    }
    else {
      this.addmember.nativeElement.classList.remove('open');
      document.body.classList.remove('overlay');
    }
  }

  ngClose(event: any) {
    this.onClose.emit(event);
    this.addmember.nativeElement.classList.remove('open');
    document.body.classList.remove('overlay');
  }

  onChange(user: UserChoose) {

    this.taskService.chooseMember(this.taskId, user)
      .subscribe(response => {

        if (user.selected)
          this.notify.success(`${user.name} tapşırıq üzrə təyin edildi`);
        else
        this.notify.warning(`${user.name} tapşırıqdan azad edildi`);
        this.users = response;
      });
  }
}

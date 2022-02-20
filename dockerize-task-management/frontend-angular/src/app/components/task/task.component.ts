import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Task } from 'src/app/models/task';
import { NotifyService } from 'src/app/services/common/notify.service';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  @Input() task!: Task;

  constructor(private notify: NotifyService) { }

  ngOnInit() {
  }
  changeStatus(event: any, status: string) {
    this.notify.confirm("Question", "You are sure?", () => {
      this.notify.success(status);
      this.task.status = status;
    });
  }

  changePriority(event: any, priority: string) {
    this.notify.confirm("Question", "You are sure?", () => {
      this.notify.success(priority);
      this.task.priority = priority;
    });
  }
}

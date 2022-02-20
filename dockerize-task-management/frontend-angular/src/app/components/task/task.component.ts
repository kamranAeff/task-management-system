import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Task } from 'src/app/models/task';
import { TaskPriorityChange } from 'src/app/models/task-priority-change copy';
import { TaskStatusChange } from 'src/app/models/task-status-change';
import { NotifyService } from 'src/app/services/common/notify.service';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  @Input() task!: Task;

  constructor(private notify: NotifyService, private taskService: TaskService) { }

  ngOnInit() {
  }
  changeStatus(event: any, status: string) {
    this.notify.confirm("Question", "You are sure?", () => {
      this.taskService.changeStatus({
        id: this.task.id,
        status
      } as TaskStatusChange)
        .subscribe(response => {
          if (response.error) {
            this.notify.error(response.message);
            return;
          }
          this.notify.success(response.message);
          this.task.status = status;
        });
    });
  }

  changePriority(event: any, priority: string) {
    this.notify.confirm("Question", "You are sure?", () => {
      this.taskService.changePriority({
        id: this.task.id,
        priority
      } as TaskPriorityChange)
        .subscribe(response => {
          if (response.error) {
            this.notify.error(response.message);
            return;
          }
          this.notify.success(response.message);
          this.task.priority = priority;
        });
    });
  }
}

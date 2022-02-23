import { Component, ElementRef, EventEmitter,  Input, OnInit, Output, ViewChild } from '@angular/core';
import { Task } from 'src/app/models/task';
import { priorities, TaskPriorityChange } from 'src/app/models/task-priority-change';
import { statuses, TaskStatusChange } from 'src/app/models/task-status-change';
import { NotifyService } from 'src/app/services/common/notify.service';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  @Input() task!: Task;
  @Output() addMember: EventEmitter<any> = new EventEmitter();

  constructor(private notify: NotifyService,private taskService: TaskService) { }

  ngOnInit() {
  }

  changeStatus(event: any, status: string) {
    this.notify.confirm("Question", "You are sure?", () => {
      this.taskService.changeStatus({
        id: this.task.id,
        status
      } as TaskStatusChange)
        .subscribe((response:any) => {
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
        .subscribe((response:any) => {
          if (response.error) {
            this.notify.error(response.message);
            return;
          }
          this.notify.success(response.message);
          this.task.priority = priority;
        });
    });
  }


  getStatuses(): string[] {
    return statuses;
  }

  getPriorities(): string[] {
    return priorities;
  }

  onAddMember(event: any, id: number) {
    event.taskId = id;
    this.addMember.emit(event);
  }
}

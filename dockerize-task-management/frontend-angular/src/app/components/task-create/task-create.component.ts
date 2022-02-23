import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { TaskCreateModel } from 'src/app/models/task';
import { priorities } from 'src/app/models/task-priority-change';
import { statuses } from 'src/app/models/task-status-change';
import { AccountService } from 'src/app/services/account.service';
import { NotifyService } from 'src/app/services/common/notify.service';
import { TaskService } from 'src/app/services/task.service';

@Component({
  selector: 'app-task-create',
  templateUrl: './task-create.component.html',
  styleUrls: ['./task-create.component.scss']
})
export class TaskCreateComponent implements OnInit {
  @ViewChild("createNewTask") createNewTask!: ElementRef;
  @Output() done: EventEmitter<any> = new EventEmitter();
  @Output() close: EventEmitter<any> = new EventEmitter();
  @Input() show: boolean = false;
  @Input() task!: TaskCreateModel;

  constructor(private taskService: TaskService,
  private notify: NotifyService,
  private accountService: AccountService) { }


  ngOnInit() {
  }

  onClose(event: any) {
    this.show = false;
    this.createNewTask.nativeElement.reset();
    this.close.emit(event);
  }

  onSubmit(event: any) {
    this.taskService.add(this.task)
      .subscribe((response:any) => {
        this.show = false;
        event.response = response;
        this.createNewTask.nativeElement.reset();
        this.done.emit(event);
      });
  }

  getStatuses(): string[] {
    return statuses;
  }

  getPriorities(): string[] {
    return priorities;
  }
}

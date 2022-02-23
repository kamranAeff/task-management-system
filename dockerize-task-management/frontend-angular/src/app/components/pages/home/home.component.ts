import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Board, BoardCreateModel } from 'src/app/models/board';
import { TaskCreateModel } from 'src/app/models/task';
import { priorities } from 'src/app/models/task-priority-change';
import { statuses } from 'src/app/models/task-status-change';
import { AccountService } from 'src/app/services/account.service';
import { BoardsService } from 'src/app/services/boards.service';
import { NotifyService } from 'src/app/services/common/notify.service';
import { TaskService } from 'src/app/services/task.service';
import { TaskAddMemberComponent } from '../../task-addmember/task-addmember.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  @ViewChild('addmember') addMember!: TaskAddMemberComponent;
  showCreateNewTaskModal: boolean = false;
  showCreateBoardModal: boolean = false;
  task: TaskCreateModel = {} as TaskCreateModel;
  board: BoardCreateModel = {} as BoardCreateModel;

  boards: Board[] = [];

  constructor(public accountService: AccountService,
    private boardService: BoardsService,
    private taskService: TaskService,
    private notify: NotifyService) { }

  ngOnInit() {
    this.accountService.fillInfo();

    this.boardService.getAll()
      .subscribe((response: any) => {
        this.boards = response;
      });
  }

  createNewTaskOpen(event: any, id: number) {
    const that = this;
    that.task.priority = "Normal";
    that.task.deadline = new Date();
    that.task.deadline.setDate(this.task.deadline.getDate() + 1);
    this.task.boardId = id;

    that.accountService.getUsers().subscribe((response: any) => {
      this.task.users = response;
      that.showCreateNewTaskModal = true;
      document.body.classList.add('overlay');
    });
  }

  createNewBoardOpen(event: any) {
    this.showCreateBoardModal = true;
    document.body.classList.add('overlay');
  }

  onTaskDone(e: any) {
    if (e.response.error) {
      this.notify.error(e.response.message);
      return;
    }
    this.notify.success(e.response.message);
    this.showCreateNewTaskModal = false;
    document.body.classList.remove('overlay');

    this.boardService.getAll()
      .subscribe(response => {
        this.boards = response;
      });
  }

  onBoardDone(e: any) {
    if (e.response.error) {
      this.notify.error(e.response.message);
      return;
    }
    this.notify.success(e.response.message);
    this.showCreateBoardModal = false;
    document.body.classList.remove('overlay');

    this.boardService.getAll()
      .subscribe(response => {
        this.boards = response;
      });
  }

  onClose(e: any) {
    this.showCreateBoardModal = false;
    this.showCreateNewTaskModal = false;
    document.body.classList.remove('overlay');
  }

  getStatuses(): string[] {
    return statuses;
  }

  getPriorities(): string[] {
    return priorities;
  }

  onAddMember(event: any) {
    event.state = true;
    this.addMember.ngChangeVisible(event);
  }

  onCloseMemberModal(event: any) {
    this.boardService.getAll()
      .subscribe(response => {
        this.boards = response;
      });
  }
}

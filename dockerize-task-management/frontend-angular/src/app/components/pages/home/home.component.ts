import { Component, OnInit } from '@angular/core';
import { Board } from 'src/app/models/board';
import { AccountService } from 'src/app/services/account.service';
import { BoardsService } from 'src/app/services/boards.service';
import { NotifyService } from 'src/app/services/common/notify.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  boards: Board[] = [];

  constructor(public accountService: AccountService,
    private boardService: BoardsService,
    private notify: NotifyService) { }

  ngOnInit() {
    this.accountService.fillInfo();

    this.boardService.getAll()
      .subscribe(response => {
        this.boards = response;

        console.log(response);
      });
  }

}

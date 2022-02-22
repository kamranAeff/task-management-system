import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { BoardCreateModel } from 'src/app/models/board';
import { BoardsService } from 'src/app/services/boards.service';

@Component({
  selector: 'app-board-create',
  templateUrl: './board-create.component.html',
  styleUrls: ['./board-create.component.scss']
})
export class BoardCreateComponent implements OnInit {
  @ViewChild("createBoard") createBoard!: ElementRef;
  @Output() done: EventEmitter<any> = new EventEmitter();
  @Output() close: EventEmitter<any> = new EventEmitter();
  @Input() show: boolean = false;
  @Input() board!: BoardCreateModel;

  constructor(private boardService: BoardsService) { }

  ngOnInit() {
  }

  onClose(event: any) {
    this.show = false;
    this.createBoard.nativeElement.reset();
    this.close.emit(event);
  }

  onSubmit(event: any) {
    this.boardService.add(this.board)
      .subscribe(response => {
        this.show = false;
        event.response = response;
        this.createBoard.nativeElement.reset();
        this.done.emit(event);
      });
  }
}

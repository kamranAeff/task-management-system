import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JsonResponse } from '../models/json-response';
import { Task, TaskCreateModel } from '../models/task';
import { TaskPriorityChange } from '../models/task-priority-change';
import { TaskStatusChange } from '../models/task-status-change';
import { UserChoose } from '../models/user-choose';

@Injectable({
  providedIn: 'root'
})
export class TaskService {


  constructor(private httpClient: HttpClient) { }

  changeStatus(model: TaskStatusChange): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/tasks/change-status`, model);
  }

  changePriority(model: TaskPriorityChange): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/tasks/change-priority`, model);
  }

  add(model: TaskCreateModel): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/tasks/add`, {
      title: model.title,
      boardId: model.boardId,
      description: model.description,
      deadline: model.deadline,
      priority: model.priority,
      mappedUserIds: model.users.filter(u => u.selected == true).map(u => u.id)
    });
  }

  getMembers(taskId: number): Observable<UserChoose[]> {
    return this.httpClient.get<UserChoose[]>(`${environment.apiUrl}/tasks/${taskId}/members`);
  }

  chooseMember(taskId: number, choose: UserChoose): Observable<UserChoose[]> {
    return this.httpClient.post<UserChoose[]>(`${environment.apiUrl}/tasks/choose-member`, {
      id: choose.id,
      taskId,
      selected: choose.selected
    });
  }

}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { JsonResponse } from '../models/json-response';
import { TaskPriorityChange } from '../models/task-priority-change copy';
import { TaskStatusChange } from '../models/task-status-change';

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

}

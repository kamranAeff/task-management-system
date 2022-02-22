import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Board, BoardCreateModel } from '../models/board';
import { JsonResponse } from '../models/json-response';

@Injectable({
  providedIn: 'root'
})
export class BoardsService {

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Board[]> {
    return this.httpClient.get<Board[]>(`${environment.apiUrl}/boards`, {
      headers: {
        "dateTimeOutFormat": "d MMMM, yyyy HH:mm"
      }
    });
  }

  add(model: BoardCreateModel): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/boards/add`, {
      title: model.title,
      description: model.description
    });
  }
}

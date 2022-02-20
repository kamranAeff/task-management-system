import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Board } from '../models/board';

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
}

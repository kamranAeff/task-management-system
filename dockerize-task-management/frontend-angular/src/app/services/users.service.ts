import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountInfo } from '../models/account-details';
import { JsonResponse } from '../models/json-response';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private httpClient: HttpClient) { }


  getAll(): Observable<AccountInfo[]> {
    return this.httpClient.get<AccountInfo[]>(`${environment.apiUrl}/users`);
  }

  setUserRole(id: number, role: string): Observable<JsonResponse> {
    return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/users/set-role`, {
      id,
      role
    });
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountInfo } from '../models/account-details';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private httpClient: HttpClient) { }


  getAll(): Observable<AccountInfo[]> {
    return this.httpClient.get<AccountInfo[]>(`${environment.apiUrl}/users`);
  }

  setUserRole(id: number, role: string): Observable<AccountInfo[]> {
    return this.httpClient.post<AccountInfo[]>(`${environment.apiUrl}/users/set-role`, {
      id,
      role
    });
  }
}

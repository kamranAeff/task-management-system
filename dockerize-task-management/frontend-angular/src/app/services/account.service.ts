import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { StorageService } from './common/storage.service';
import { AccountSelectedTab } from './common/account-selected-tab';
import { LoginResponse, LoginUser } from '../models/login-user';
import { AccountInfo } from '../models/account-details';
import { Router } from '@angular/router';
import { UserChoose } from '../models/user-choose';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    constructor(private httpClient: HttpClient,
        private storage: StorageService,
        private route: Router) { }

    selectedTab: AccountSelectedTab = AccountSelectedTab.None;
    accountDetails: AccountInfo = {} as AccountInfo;
    loggedIn: boolean = false;

    getToken(): string {
        return this.storage.getValue('token');
    }

    isLoggedIn(): boolean {
        let token: string = this.getToken();
        this.loggedIn = (token || '').length > 0;
        return this.loggedIn;
    }

    login(user: LoginUser): Observable<LoginResponse> {
        return this.httpClient.post<LoginResponse>(`${environment.apiUrl}/account/login`, user);
    }

    checkToken() {
        this.httpClient.get<boolean>(`${environment.apiUrl}/account/check-token`)
            .subscribe(response => {
                this.loggedIn = false;
            }, () => {
                this.logout();
                console.log('expired');
            });
    }

    getUsers(): Observable<UserChoose[]> {
        return this.httpClient.get<UserChoose[]>(`${environment.apiUrl}/account/users`);
    }

    fillInfo(callback: any = null) {
        this.httpClient.get<AccountInfo>(`${environment.apiUrl}/account/account-info`)
            .subscribe(response => {
                this.accountDetails = response;
                console.log(response);

                if (callback != null && typeof callback == 'function')
                    callback();
            });
    }

    logout() {
        this.storage.remove('token');
        this.loggedIn = false;
        this.accountDetails = {} as AccountInfo;
        this.route.navigate(['/']);
    }

}

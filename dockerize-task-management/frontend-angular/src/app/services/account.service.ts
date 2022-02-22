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
import { JsonResponse } from '../models/json-response';
import { UserSignupTicket } from '../models/user-signup-ticket';
import { UserSignupComplate } from '../models/user-signup-complate';

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


    sendSignupTicket(model: UserSignupTicket): Observable<JsonResponse> {
        return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/account/create-sigup-ticket`, model);
    }

    //http://localhost:4200/complate-signup?token=qAJEK7BhUmLC2BmRQuP967R9cDae6RjpAV5Mc%2F3l4VK9sLjdPsWW%2FcDMC%2F9quv%2FdWix%2Bz57B6k3SM9WTUVdPFQ%3D%3D
    complateSignup(model: UserSignupComplate, token: string): Observable<JsonResponse> {
        console.log(model,token);
        return this.httpClient.post<JsonResponse>(`${environment.apiUrl}/account/complate-signup`, model, {
            headers: {
                'signupToken': token
            }
        });
    }
}

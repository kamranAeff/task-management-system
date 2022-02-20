import { Injectable } from '@angular/core';
import { EMPTY, Observable, catchError, switchMap } from 'rxjs';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse } from '@angular/common/http';
import { NotifyService } from './notify.service';
import { StorageService } from './storage.service';
import { AccountService } from '../account.service';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class HttpErrorInterceptor implements HttpInterceptor {
    constructor(private notify: NotifyService,
        private storage: StorageService,
        private account: AccountService,
        private router: Router) {

    }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        document.querySelectorAll(`span[name*="error-"]`).forEach((item: any) => {
            item.innerHTML = '';
            document.querySelector(`[name="${item.getAttribute('name').replace('error-', '')}"]`)?.classList.remove('is-invalid');
        });

        return next.handle(this.addTokenHeader(request))
            .pipe(catchError((error: HttpErrorResponse) => {
                let errorMsg = '';
                if (error.error instanceof ErrorEvent) {
                    console.log('this is client side error');
                    errorMsg = `Error: ${error.error.message}`;
                }
                else {
                    console.log('this is server side error', error);
                    errorMsg = `Error Code: ${error.status},  Message: ${error.message}`;

                    if (error.status == 401) {
                        this.storage.remove('token');
                        this.notify.warning('Sistemə giriş etməlisiz!');
                        this.account.loggedIn = false;
                        this.router.navigate(['/login']);
                    }
                    else if (error.status == 403) {
                        this.notify.warning('Səlahiyyətiniz yoxdur!');
                    }
                }

                if (error.error?.errors != undefined) {
                    for (const key in error.error.errors) {
                        let element = document.querySelector(`[name="${key}"]`);

                        if (element != null) {
                            element.classList.add('is-invalid')
                            let span = document.querySelector(`[name="error-${key}"]`);

                            if (span == null) {
                                span = document.createElement('span');
                                span.setAttribute('name', `error-${key}`);

                                if (element.nextSibling) {
                                    element.parentNode?.insertBefore(span, element.nextSibling);
                                } else {
                                    element.parentNode?.append(span);
                                }
                            }

                            span.classList.add('error');
                            span.innerHTML = `${error.error.errors[key][0]}`;
                        }


                    }
                }

                console.log('intercept', error);
                return EMPTY;
            })
            )
    }

    private addTokenHeader(request: HttpRequest<any>): HttpRequest<any> {
        const token = this.storage.getValue("token");

        if (token != null)
            return request.clone({ headers: request.headers.set('Authorization', `Bearer ${token}`) });
        else
            return request;
    }
}
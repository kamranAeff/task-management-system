import { Injectable } from '@angular/core';

declare let alertify: any;

@Injectable({
  providedIn: 'root'
})
export class NotifyService {

  constructor() { }

  confirm(title: string, message: string, okCallback: () => any) {
    alertify.confirm(title, message, (e: any) => {
      if (e) {
        okCallback();
      } else {
      }
    }, () => { }).set('labels', { ok: 'Accept', cancel: 'Cancel' });
  }

  success(message: string) {
    alertify.success(message);
  }

  error(message: string) {
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }

}

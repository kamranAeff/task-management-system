import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { AccountService } from "../services/account.service";
import { NotifyService } from "../services/common/notify.service";

@Injectable()
export class AuthorizeGuard implements CanActivate {
    constructor(private accountService: AccountService,
        private notify: NotifyService,
        private router: Router) { }

    canActivate(
        route: ActivatedRouteSnapshot,
        state: RouterStateSnapshot
    ): boolean {
        let logged = this.accountService.isLoggedIn();

        if (logged) {
            return true;
        }
        this.notify.warning("Sistemə giriş etməlisiniz!");
        return false;
    }
}
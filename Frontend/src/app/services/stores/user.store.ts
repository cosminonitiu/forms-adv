import { Inject, Injectable } from '@angular/core';
import { MSAL_GUARD_CONFIG, MsalGuardConfiguration, MsalService, MsalBroadcastService } from '@azure/msal-angular';
import { AccountInfo, AuthenticationResult, EventMessage, EventType, InteractionStatus, PopupRequest, RedirectRequest, SilentRequest } from '@azure/msal-browser';
import { filter, Observable, Subject, takeUntil } from 'rxjs';
import { NotificationsService } from '../entities/notifications.service';
import { Notification } from '../../shared/models/notification';

//https://learn.microsoft.com/en-us/azure/active-directory-b2c/enable-authentication-angular-spa-app
//https://learn.microsoft.com/en-us/entra/identity-platform/access-token-claims-reference
//https://learn.microsoft.com/en-us/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient

@Injectable({
  providedIn: 'root',
})
export class UserStore {
    constructor(
        @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
        private authService: MsalService,
        private broadcastService: MsalBroadcastService
    ) {}

    loginDisplay = false;
    private readonly _destroying$ = new Subject<void>();

    public currentAccount: AccountInfo | null = null;
    public currentNotifications: Notification[] = [];

    public initStore() {
        this.authService.instance.enableAccountStorageEvents(); // Optional - This will enable ACCOUNT_ADDED and ACCOUNT_REMOVED events emitted when a user logs in or out of another tab or window
        this.broadcastService.msalSubject$
          .pipe(
            filter((msg: EventMessage) => msg.eventType === EventType.ACCOUNT_ADDED || msg.eventType === EventType.ACCOUNT_REMOVED || msg.eventType === EventType.LOGIN_SUCCESS || msg.eventType === EventType.ACQUIRE_TOKEN_SUCCESS),
          )
          .subscribe((result: EventMessage) => {
            if(result.eventType === EventType.LOGIN_SUCCESS) {
                const payload = result.payload as AuthenticationResult;
                this.authService.instance.setActiveAccount(payload.account);
                this.currentAccount = payload.account;
            } else if(result.eventType === EventType.ACQUIRE_TOKEN_SUCCESS) {
                //console.log(result)
             }else {
                if (this.authService.instance.getAllAccounts().length === 0) {
                    window.location.pathname = "/";
                } else {
                    this.setLoginDisplay();
                }
            }
            
          });
        
        this.broadcastService.inProgress$
          .pipe(
            filter((status: InteractionStatus) => status === InteractionStatus.None),
            takeUntil(this._destroying$)
          )
          .subscribe(() => {
            this.setLoginDisplay();
            this.checkAndSetActiveAccount();
          })

        // var request = {
        //     scopes: ['https://aweb2c2.onmicrosoft.com/15ad4d5a-dd9b-42ab-96a4-cb39cea16fab/tasks.read'],
        // };
        // this.authService.acquireTokenSilent(request).subscribe((data: AuthenticationResult) => {
        //     console.log(data)
        // })
    }

    setLoginDisplay() {
        if(this.authService.instance.getAllAccounts().length > 0) {
            this.loginDisplay = true;
            this.currentAccount = this.authService.instance.getAllAccounts()[0]; 
        } else {
            this.loginDisplay = false;
        }
    }

    checkAndSetActiveAccount(){
        /**
         * If no active account set but there are accounts signed in, sets first account to active account
         * To use active account set here, subscribe to inProgress$ first in your component
         * Note: Basic usage demonstrated. Your app may require more complicated account selection logic
         */
        let activeAccount = this.authService.instance.getActiveAccount();

        if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
            let accounts = this.authService.instance.getAllAccounts();
            this.authService.instance.setActiveAccount(accounts[0]);
        }
    }

    loginRedirect() {
        if (this.msalGuardConfig.authRequest){
            this.authService.loginRedirect({...this.msalGuardConfig.authRequest} as RedirectRequest);
        } else {
            this.authService.loginRedirect();
        }
    }

    loginPopup() {
        if (this.msalGuardConfig.authRequest){
            this.authService.loginPopup({...this.msalGuardConfig.authRequest} as PopupRequest)
            .subscribe((response: AuthenticationResult) => {
                this.authService.instance.setActiveAccount(response.account);
            }, (e ) => {
                console.log(e)
            });
            } else {
            this.authService.loginPopup()
                .subscribe((response: AuthenticationResult) => {
                this.authService.instance.setActiveAccount(response.account);
            });
        }
    }

    logout(popup?: boolean) {
        if (popup) {
            this.authService.logoutPopup({
            mainWindowRedirectUri: "/"
            });
        } else {
            this.authService.logoutRedirect();
        }
    }

    ngOnDestroy(): void {
        this._destroying$.next(undefined);
        this._destroying$.complete();
    }
}
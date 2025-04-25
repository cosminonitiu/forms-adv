import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { RouterOutlet } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { BrowserModule } from '@angular/platform-browser';
import {DragDropModule} from '@angular/cdk/drag-drop';
import { MatButtonModule } from '@angular/material/button';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatTableModule } from '@angular/material/table';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';
import { MatMenuModule } from '@angular/material/menu';
import {MatProgressBarModule} from '@angular/material/progress-bar';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

import { LucideAngularModule } from 'lucide-angular';
import { RequestCardComponent } from './components/request-card/request-card.component';
import { NewRequestComponent } from './pages/requests/new-request/new-request.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SubmitRequestComponent } from './pages/requests/submit-request/submit-request.component';
import { StringLimitPipe } from './pipes/string-limit.pipe';
import { ApproveRequestComponent } from './pages/requests/approve-request/approve-request.component';
import { ConditionalVisibilityDialogComponent } from './pages/requests/new-request/conditional-visibility-dialog/conditional-visibility-dialog.component';

import { icons } from 'lucide-angular';

import { environment } from '../environments/environment';

import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import {
  IPublicClientApplication,
  PublicClientApplication,
  InteractionType,
  BrowserCacheLocation,
  LogLevel,
} from '@azure/msal-browser';
import {
  MsalGuard,
  MsalInterceptor,
  MsalBroadcastService,
  MsalInterceptorConfiguration,
  MsalModule,
  MsalService,
  MSAL_GUARD_CONFIG,
  MSAL_INSTANCE,
  MSAL_INTERCEPTOR_CONFIG,
  MsalGuardConfiguration,
  MsalRedirectComponent,
} from '@azure/msal-angular';
import { FailedLoginComponent } from './components/failed-login/failed-login.component';
import { ProfileComponent } from './components/profile/profile.component';
import { HomeComponent } from './pages/home/home.component';

export function loggerCallback(logLevel: LogLevel, message: string) {
  //console.log(message); //TODO
}

export function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.msalConfig.auth.clientId,
      authority: environment.msalConfig.auth.authority,
      redirectUri: 'http://localhost:4200',
      postLogoutRedirectUri: '/',
      knownAuthorities: ['aweb2c2.b2clogin.com']
    },
    cache: {
      cacheLocation: 'localStorage',
      storeAuthStateInCookie: false
    },
    system: {
      allowPlatformBroker: false, // Explicitly disable WAM broker
      loggerOptions: {
        loggerCallback,
        logLevel: LogLevel.Verbose,
        piiLoggingEnabled: true,
      },
    },
  });
}

export function MSALInterceptorConfigFactory(): MsalInterceptorConfiguration {
  const protectedResourceMap = new Map<string, Array<string>>();
  protectedResourceMap.set(
    environment.apiConfig.uri,
    environment.apiConfig.scopes
  );

  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap,
  };
}

export function MSALGuardConfigFactory(): MsalGuardConfiguration {
  return {
    interactionType: InteractionType.Redirect,
    authRequest: {
      scopes: [...environment.apiConfig.scopes],
      authority: environment.msalConfig.auth.authority,
      prompt: 'select_account'
    },
    loginFailedRoute: '/login-failed',
  };
}


@NgModule({
  declarations: [
    AppComponent,
    RequestCardComponent,
    FailedLoginComponent,
    NewRequestComponent,
    HomeComponent,
    SubmitRequestComponent,
    ApproveRequestComponent,
    ConditionalVisibilityDialogComponent,
    ProfileComponent
  ],
  imports: [
    CommonModule,
    RouterOutlet,
    MatMenuModule,
    AppRoutingModule,
    //NoopAnimationsModule,
    MatProgressBarModule,
    MatProgressSpinnerModule,
    MsalModule,
    BrowserModule,
    DragDropModule,
    LucideAngularModule.pick(icons),
    MatButtonModule,
    MatToolbarModule,
    MatListModule,
    MatTableModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatSlideToggleModule,
    StringLimitPipe,  
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory,
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useFactory: MSALGuardConfigFactory,
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory,
    },
    MsalService,
    MsalGuard,
    MsalBroadcastService
  ],
  bootstrap: [
    AppComponent, MsalRedirectComponent
  ]
})
export class AppModule { }

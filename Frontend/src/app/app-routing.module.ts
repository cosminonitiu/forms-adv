import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { NewRequestComponent } from './pages/new-request/new-request.component';
import { NotFoundPageComponent } from './pages/not-found-page/not-found-page.component';
import { RequestsComponent } from './pages/requests/requests.component';
import { MsalGuard } from '@azure/msal-angular';
import { FailedLoginComponent } from './components/failed-login/failed-login.component';
import { BrowserUtils } from '@azure/msal-browser';
import { ProfileComponent } from './components/profile/profile.component';
import { HomeComponent } from './pages/home/home.component';

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'newrequests' },
      { path: 'manage/:requestId', component: NewRequestComponent },
      { path: 'myrequests', component: RequestsComponent },
      { path: 'myrequests/:requestId', component: RequestsComponent },
      { path: 'approve', component: RequestsComponent },
      { path: 'not-found', component: NotFoundPageComponent },
      { path: 'profile', component: ProfileComponent },
      { path: '**', redirectTo: '/not-found', pathMatch: 'full' }
    ],
    canActivateChild: [MsalGuard]
  },
  {
    path: 'login-failed',
    component: FailedLoginComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { initialNavigation: !BrowserUtils.isInIframe() && !BrowserUtils.isInPopup() ? 'enabledBlocking' : 'disabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule {}
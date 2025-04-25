import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NotificationComponent } from '../../components/notification/notification.component';

@Injectable({
  providedIn: 'root'
})
export class SnackbarHelperService {
  snackBarNotificationDurationMs = 4000;
  public isSnackbarAlreadyActive: boolean = false;

  constructor(
    private snackBar: MatSnackBar,
    //private translate: TranslateService
  ) {}

  public async createSuccessNotification(
    message = '',
    color = 'theme-success-background'
  ) {
    this.createNotification(message, color);
  }

  public async createErrorNotification(
    message = '',
    color = 'theme-danger-background'
  ) {
    this.createNotification(message, color);
  }

  dismissNotification() {
    this.isSnackbarAlreadyActive = false;
    this.snackBar.dismiss();
  }

  private createNotification(message: string, color: string) {
    this.isSnackbarAlreadyActive = true;
    this.snackBar.openFromComponent(NotificationComponent, {
      duration: this.snackBarNotificationDurationMs,
      data: { message },
      panelClass: color
    });
  }
}

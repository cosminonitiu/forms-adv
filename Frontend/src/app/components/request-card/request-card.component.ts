import { Component, input, output } from '@angular/core';
import { Trash } from 'lucide-angular';
import { FormRequest } from '../../shared/models/form-request';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { SubmitRequestComponent } from '../../pages/requests/submit-request/submit-request.component';

@Component({
  selector: 'app-request-card',
  standalone: false,
  templateUrl: './request-card.component.html',
  styleUrl: './request-card.component.scss'
})
export class RequestCardComponent {
  
  constructor(
    private router: Router,
    private dialog: MatDialog
  ){}

  request = input<FormRequest>();
  manageMode = input<boolean>(false) 
  readonly Trash = Trash;
  generatePdf = output<FormRequest>()

  editRequest() {
    const id = this.request()?.id;
    if(id) {
      this.router.navigate(['/manage/' + id])
    }
  }

  exportToPdf() {
    const request = this.request();
    if(request) {
      this.generatePdf.emit(request);
    }
  }

  createRequest() {
    const dialogRef = this.dialog.open(SubmitRequestComponent, {
      panelClass: 'dialog-box',
      data: { draft: false, request: this.request(), submittedRequest: null}
    })
  }
}

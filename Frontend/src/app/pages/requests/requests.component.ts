import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileDown, LucideAngularModule, Plus, Trash2 } from 'lucide-angular';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { RequestsService } from '../../services/entities/requests.service';
import { SubmittedRequest } from '../../shared/models/submitted-requests';
import { SubmittedRequestsService } from '../../services/entities/submitted-requests.service';
import { forkJoin } from 'rxjs';
import { FormRequest } from '../../shared/models/form-request';
import { UserStore } from '../../services/stores/user.store';
import { MatDialog } from '@angular/material/dialog';
import { SubmitRequestComponent } from './submit-request/submit-request.component';
import { ApproveRequestComponent } from './approve-request/approve-request.component';

@Component({
  selector: 'app-requests',
  standalone: false,
  templateUrl: './requests.component.html',
  styleUrl: './requests.component.scss'
})
export class RequestsComponent implements OnInit{

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private userStore: UserStore,
    private FormRequestsService: RequestsService,
    private submittedRequestService: SubmittedRequestsService,
    private dialog: MatDialog
  ) {}

  readonly Plus = Plus;
  readonly Trash2 = Trash2;
  readonly FileDown = FileDown;

  public approvalMode = false;
  private userId: string | undefined = undefined;

  ngOnInit(): void {
    this.userId = this.userStore.currentAccount?.localAccountId;
    if(this.userId) {
      if(this.router.url === '/approve') {
        this.approvalMode = true;
        this.initializeDataApprove();
      }  else {
        this.initializeDataMyRequests();
      }
    }
  }

  private initializeDataApprove() {
    if(this.userId) {
      this.submittedRequestService.apiApprovalRequestsGet(this.userId).subscribe((data: SubmittedRequest[]) => {
        this.submittedRequests = data;
        this.filteredRequests = this.submittedRequests;
      })
    }
  }
  private initializeDataMyRequests() {
    if(this.userId) {
      forkJoin([
        this.submittedRequestService.apiSubmittedRequestsGet(this.userId),
        this.FormRequestsService.apiFormRequestsGet()
      ])
      .subscribe((data: [SubmittedRequest[], FormRequest[]]) => {
        this.submittedRequests = data[0];
        this.route.params.subscribe(params => {
          const requestId = params['requestId'];
          if(requestId) {
            this.selectedRequestId = requestId;
            this.selectedRequestChange();
          } else {
            this.filteredRequests = structuredClone(this.submittedRequests);
          }
        })
        this.requests = data[1];
      })
    }
  }

  public submittedRequests: SubmittedRequest[] = [];
  public requests: FormRequest[] = [];
  public filteredRequests: SubmittedRequest[] = []

  public selectedRequestId: string | null = null;
  public selectedStatus: string | null = null;
  public statuses = ['Approved', 'Rejected', 'Submitted'];

  selectedRequestChange() {
    this.filteredRequests = [];
    this.submittedRequests.forEach((r: SubmittedRequest) => {
      if(r.formId == this.selectedRequestId) {
        this.filteredRequests.push(r);
      }
    })
  }
  selectedStatusChange() {
    this.filteredRequests = [];
    this.submittedRequests.forEach((r: SubmittedRequest) => {
      if(r.state == this.selectedStatus) {
        this.filteredRequests.push(r);
      }
    })
  }

  getStateClass(state: string): string {
    switch (state) {
      case 'Approved':
        return 'bg-green-100 text-green-800';
      case 'Submitted':
        return 'bg-yellow-100 text-yellow-800';
      case 'Rejected':
        return 'bg-red-100 text-red-800';
      default:
        return '';
    }
  }
  
  navigateToApproveRequest(submittedRequest: SubmittedRequest) {
    const dialogRef = this.dialog.open(ApproveRequestComponent, {
      panelClass: 'dialog-box',
      data: { viewOnly: false, submittedRequest: submittedRequest }
    })
    dialogRef.afterClosed().subscribe((data: string) => {
      if(data === "DONE") {
        this.initializeDataApprove();
      }
    })
  }

  navigateToEditRequest(submittedRequest: SubmittedRequest) {
    const dialogRef = this.dialog.open(SubmitRequestComponent, {
      panelClass: 'dialog-box',
      data: { draft: true, request: null, submittedRequest: submittedRequest }
    })
    dialogRef.afterClosed().subscribe((data: string) => {
      if(data === 'SUBMIT') {
        this.initializeDataMyRequests();
      }
    })
  }

  navigateToViewRequest(submittedRequest: SubmittedRequest) {
    const dialogRef = this.dialog.open(ApproveRequestComponent, {
      panelClass: 'dialog-box',
      data: { viewOnly: true, submittedRequest: submittedRequest }
    })
    dialogRef.afterClosed().subscribe((data: string) => {
      
    })
  }
}

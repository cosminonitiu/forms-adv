import { Injectable } from "@angular/core";
import { FormRequest } from "../../shared/models/form-request";
import { SubmittedRequest } from "../../shared/models/submitted-requests";
import { RequestsService } from "../entities/requests.service";
import { SubmittedRequestsService } from "../entities/submitted-requests.service";

export interface LoadingState {
    pinnedRequests: boolean;
    pinnedManagedRequests: boolean;
    formRequests: boolean;
    manageRequests: boolean;
    myRequests: boolean;
    requestsToApprove: boolean;
}

@Injectable({
    providedIn: 'root',
})
export class RequestsStore {
    constructor(
        private formRequestsService: RequestsService,
        private submittedRequestsService: SubmittedRequestsService
    ){}

    public loadingState: LoadingState = {
        pinnedRequests: true,
        pinnedManagedRequests: true,
        formRequests: true,
        manageRequests: true,
        myRequests: true,
        requestsToApprove: true
    }

    public pinnedRequests: FormRequest[] = [];
    public pinnedManagedRequests: FormRequest[] = [];
    public allRequest: FormRequest[] = [];
    public allManagedRequests: FormRequest[] = [];
    public submittedRequests: SubmittedRequest[] = [];
    public requestsToApprove: SubmittedRequest[] = [];

    public initializeRequests(accountId: any) {
      this.reloadFormRequests();
      this.reloadOwnedRequests();
      this.reloadApprovalRequests(accountId);   
      this.reloadSubmittedRequests(accountId);  
    }

    public reloadFormRequests() {
      this.formRequestsService.apiFormRequestsGet().subscribe((data: FormRequest[]) => {
        this.pinnedRequests = data.slice(0, 3);
        this.allRequest = data;
        this.loadingState.pinnedRequests = false;
        this.loadingState.formRequests = false;
      })
    }

    public reloadOwnedRequests() {
      this.formRequestsService.apiFormRequestsGetOwner().subscribe((data: FormRequest[]) => {
        this.pinnedManagedRequests = data.slice(0, 3);
        this.allManagedRequests = data;
        this.loadingState.pinnedManagedRequests = false;
        this.loadingState.manageRequests = false;
      })
    }

    public reloadApprovalRequests(accountId: any) {
      this.submittedRequestsService.apiApprovalRequestsGet(accountId).subscribe((data: SubmittedRequest[]) => {
        this.requestsToApprove = data;
        this.loadingState.requestsToApprove = false;
      })
    }

    public reloadSubmittedRequests(accountId: any) {
      this.submittedRequestsService.apiSubmittedRequestsGet(accountId).subscribe((data: SubmittedRequest[]) => {
        this.submittedRequests = data;
        this.loadingState.myRequests = false;
      })
    }
}
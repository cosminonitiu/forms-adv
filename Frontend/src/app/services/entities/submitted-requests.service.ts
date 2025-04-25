import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { SubmittedRequest, SubmittedRequestSaveDraft, SubmittedRequestUpdateModel } from "../../shared/models/submitted-requests";

@Injectable({
    providedIn: 'root'
})
export class SubmittedRequestsService {
    protected basePath = environment.apiUrl;
    public defaultHeaders = new HttpHeaders();

    constructor(private httpClient: HttpClient) {}

    public apiSubmtitedRequestGet(id: string, owner: string): Observable<SubmittedRequest | null> {
        const options = owner && id ?
            { params: new HttpParams().set('Owner', owner).set('Id', id) } : {};
        return this.httpClient.get<SubmittedRequest>(`${this.basePath}/SubmittedRequests/One`, options);
    }

    public apiSubmittedRequestsGet(owner: string): Observable<SubmittedRequest[]> {
        const options = owner ?
            { params: new HttpParams().set('Owner', owner) } : {};
        return this.httpClient.get<SubmittedRequest[]>(`${this.basePath}/SubmittedRequests`, options);
    }

    public apiApprovalRequestsGet(approver: string): Observable<SubmittedRequest[]> {
        const options = approver ?
            { params: new HttpParams().set('Approver', approver) } : {};
        return this.httpClient.get<SubmittedRequest[]>(`${this.basePath}/SubmittedRequests/ToApprove`, options);
    }

    public apiSaveDraftRequestsPost(request: SubmittedRequestSaveDraft): Observable<string> {
        return this.httpClient.post(`${this.basePath}/SubmittedRequests`, request, { responseType: 'text' });
    }

    public apiSubmittedRequestsUpdate( requestUpdate: SubmittedRequestUpdateModel): Observable<any> {
        return this.httpClient.put<any>(`${this.basePath}/SubmittedRequests/${requestUpdate.id}`,requestUpdate );
    }

    public apiSubmittedRequestsSubmit(requestId: string): Observable<any> {
        const model = {};
        return this.httpClient.put<any>(`${this.basePath}/SubmittedRequests/Submit/${requestId}`, model, {responseType: 'json'});
    }

    public apiSubmittedRequestsApprove(requestId: string, owner: string): Observable<any> {
        const model = {
            "owner": owner
        };
        return this.httpClient.put<any>(`${this.basePath}/SubmittedRequests/Approve/${requestId}`, model, {responseType: 'json'});
    }

    public apiSubmittedRequestsReject(requestId: string, owner: string): Observable<any> {
        const model = {
            "owner": owner
        };
        return this.httpClient.put<any>(`${this.basePath}/SubmittedRequests/Reject/${requestId}`, model, {responseType: 'json'});
    }
    // public apiSubmittedDelete(id: number): Observable<any> {
    //     return this.httpClient.delete<any>( `${this.basePath}/SubmittedRequests/${id}`);
    // }
}
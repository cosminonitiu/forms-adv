import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { FormRequest } from "../../shared/models/form-request";

@Injectable({
    providedIn: 'root'
})
export class RequestsService {
    protected basePath = environment.apiUrl;
    public defaultHeaders = new HttpHeaders();

    constructor(private httpClient: HttpClient) {}

    public apiFormRequestGet(id: string, owner: string): Observable<FormRequest | null> {
        const options = id && owner ? //TODO
            { params: new HttpParams().set('Id', id).set('Owner', owner) } : {};

        return this.httpClient.get<FormRequest>(`${this.basePath}/FormRequests/One`, options);
    }

    public apiFormRequestsGet(): Observable<FormRequest[]> {
        return this.httpClient.get<FormRequest[]>(`${this.basePath}/FormRequests`);
    }

    public apiFormRequestsGetOwner(): Observable<FormRequest[]> {
        return this.httpClient.get<FormRequest[]>(`${this.basePath}/FormRequests/Managed`);
    }

    public apiFormRequestsPut( requestUpdate: FormRequest): Observable<any> {
        return this.httpClient.put<string>(`${this.basePath}/FormRequests/${requestUpdate.id}`,requestUpdate );
    }

    // public apiFormRequestsDelete(id: number): Observable<any> {
    //     return this.httpClient.delete<any>( `${this.basePath}/FormRequests/${id}`);
    // }

    // public apiFormRequestsPost( request: FormRequest): Observable<string> {
    //     return this.httpClient.post<string>(`${this.basePath}/FormRequests`,request );
    // }
}
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { FormRequest } from "../../shared/models/form-request";
import { ADUser } from "../../shared/models/ad-user";

@Injectable({
    providedIn: 'root'
})
export class ADService {
    protected basePath = environment.apiUrl;
    public defaultHeaders = new HttpHeaders();

    constructor(private httpClient: HttpClient) {}

    public apiAllUsersGet(): Observable<ADUser[]> {
        return this.httpClient.get<ADUser[]>(`${this.basePath}/AD/AllUsers`);
    }
}
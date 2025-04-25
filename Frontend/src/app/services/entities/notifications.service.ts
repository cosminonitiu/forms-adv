import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Observable, of } from "rxjs";
import { Notification } from "../../shared/models/notification";

@Injectable({
    providedIn: 'root'
})
export class NotificationsService {
    protected basePath = environment.apiUrl;
    public defaultHeaders = new HttpHeaders();

    constructor(private httpClient: HttpClient) {}

    public apiNotificationGet(): Observable<Notification[]> {
        return this.httpClient.get<Notification[]>(`${this.basePath}/Notifications`);
    }

    public apiNotificationDelete(id: number): Observable<any> {
        return this.httpClient.delete<any>( `${this.basePath}/Notifications/${id}`);
    }
}
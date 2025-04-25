export interface Notification {
    id: number;
    owner: string;
    type: 'approve';
    formRequestName: string;
    submittedRequestId: string;
    requesterUID: string;
    requesterName: string;
}
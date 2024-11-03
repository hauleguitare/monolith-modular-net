export interface CompanyTaskResponse {
    id: number;
    companyId?: number;
    taskName?: string;
    description?: string;
    type?: string;
    companyName?: string;
    sessions: CompanyTaskSessionResponse[];
}

export interface CompanyTaskSessionResponse {
    email?: string;
    password?: string;
    companyTaskId?: number;
    process?: CompanyTaskSessionProcessResponse;
}

export interface CompanyTaskSessionProcessResponse {
    companyTaskSessionId?: number;
    statusId: number;
    statusName?: string;
    statusHexColor?: string;
    activatedAt?: string;
}

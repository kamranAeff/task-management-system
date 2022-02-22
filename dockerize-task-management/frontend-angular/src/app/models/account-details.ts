import { Organisation } from "./organisation";

export interface AccountInfo {
    id: number;
    name: string;
    surname: string;
    patronymic?: any;
    visibleName: string;
    email: string;
    userName: string;
    organisations: Organisation[];
    isSuperAdmin: boolean;
    isOrganisationAdmin: boolean;
    isUser: boolean;
}
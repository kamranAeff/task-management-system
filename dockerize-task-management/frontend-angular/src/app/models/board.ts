import { Organisation } from "./organisation";
import { Task } from "./task";

export interface Board {
    id: number;
    title: string;
    description: string;
    authorId: number;
    author: string;
    createdDate: string;
    organisationId: number;
    organisation: Organisation;
    tasks: Task[];
}

export interface BoardCreateModel {
    id: number;
    title: string;
    description: string;
}
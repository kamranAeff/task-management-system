import { UserChoose } from "./user-choose";

export interface Task {
    id: number;
    title: string;
    description: string;
    deadline: string;
    status: string;
    priority: string;
    authorId: number;
    author: string;
    createdDate: string;
    users: UserChoose[];
}

export interface TaskCreateModel {
    id: number;
    title: string;
    boardId: number;
    description: string;
    deadline: Date;
    priority: string;
    users: UserChoose[];
    mappedUserIds: number[];
}
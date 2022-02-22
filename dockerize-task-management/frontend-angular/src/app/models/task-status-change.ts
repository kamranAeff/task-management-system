export const statuses: string[] = ['New', 'Complated'];

export interface TaskStatusChange {
    id: number;
    status: string;
}
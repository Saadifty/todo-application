export interface Todo {
    id?: number;
    user_id: number;
    title: string;
    description?: string;
    priority: string;
    is_completed: boolean;
    created_at?: Date;
    completed_at?: Date;
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Todo } from '../model/todo';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TodoService {
  baseURL: string = "https://todo-saad-api.azurewebsites.net/api";
  constructor(private http: HttpClient) { }

  get authHeader(): string {
    return localStorage['headerValue']; // Retrieve token from localStorage
  }

  get userId(): number {
    return parseInt(localStorage.getItem('userId') || '0', 10);
  }
  
    // Fetch all todos by userId
    getTodos(userId: number): Observable<Todo[]> {
      const apiUrl = `${this.baseURL}/todo?userId=${this.userId}`;
      return this.http.get<Todo[]>(apiUrl, {
        headers: { Authorization: this.authHeader },
      });
    }
  
   // Fetch a single todo by ID
   getTodo(id: number): Observable<Todo> {
    const apiUrl = `${this.baseURL}/todo/${id}`;
    return this.http.get<Todo>(apiUrl, { headers: { Authorization: this.authHeader},
  });
  }

  // Create a new todo
  createTodo(todo: Todo): Observable<any> {
    const apiUrl = `${this.baseURL}/todo`;
    return this.http.post(apiUrl, todo, { headers: { Authorization: this.authHeader},
    });
    }

  // Delete a todo by ID
  deleteTodo(id: number): Observable<any> {
    const apiUrl = `${this.baseURL}/todo/${id}`;
    return this.http.delete(apiUrl, { headers: { Authorization: this.authHeader},
    });
    }

  // Edit an existing todo
  editTodo(todo: Todo): Observable<Todo> {
    const apiUrl = `${this.baseURL}/todo`;
    return this.http.put<Todo>(apiUrl, todo, { headers: { Authorization: this.authHeader},
    });
    }
}

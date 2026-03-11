import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseURL: string = "https://todo-saad-api.azurewebsites.net/api";

  constructor(private http: HttpClient) {}

  // Register a new user
  register(user: User): Observable<any> {
    return this.http.post(`${this.baseUrl}/user/register`, user);
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Login } from '../model/login';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  baseUrl: string = 'http://localhost:5269/api';

  // Observable to track the login state
  private isLoggedInSubject: BehaviorSubject<boolean>;
  private usernameSubject: BehaviorSubject<string>; // Add BehaviorSubject for username

  constructor(private http: HttpClient, private router: Router) {
    // Initialize the BehaviorSubjects
    const storedState = !!localStorage.getItem('headerValue');
    const storedUsername = localStorage.getItem('username') || '';

    this.isLoggedInSubject = new BehaviorSubject<boolean>(storedState);
    this.usernameSubject = new BehaviorSubject<string>(storedUsername);
  }

  // Observable for login state
  get isLoggedIn$(): Observable<boolean> {
    return this.isLoggedInSubject.asObservable();
  }

  // Observable for username
  get username$(): Observable<string> {
    return this.usernameSubject.asObservable();
  }

  // Directly get the current login state
  isLoggedIn(): boolean {
    return this.isLoggedInSubject.value;
  }

  // Authenticate user
  authenticate(username: string, password: string): Observable<Login> {
    const body = { username, password };
    return this.http.post<Login>(`${this.baseUrl}/login`, body);
  }

  // Handle successful login
  handleLoginSuccess(headerValue: string, userId: string, username: string): void {
    // Store authentication details in localStorage
    localStorage.setItem('headerValue', headerValue);
    localStorage.setItem('userId', userId);
    localStorage.setItem('username', username); // Save username

    // Update login state and username
    this.isLoggedInSubject.next(true);
    this.usernameSubject.next(username);
  }

  // Logout the user
  logout(): void {
    // Clear stored data
    localStorage.removeItem('headerValue');
    localStorage.removeItem('userId');
    localStorage.removeItem('username'); // Remove username

    // Update login state and clear username
    this.isLoggedInSubject.next(false);
    this.usernameSubject.next('');

    // Redirect to login page
    this.router.navigate(['/login']);
  }
}
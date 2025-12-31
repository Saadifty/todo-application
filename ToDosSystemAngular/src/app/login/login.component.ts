import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
  ],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent {
  username!: string; 
  password!: string; 
  authenticated = false;
  errorMessage: string = '';

  constructor(public auth: AuthService, private router: Router) {}

  login() {
    // Clear previous error message
    this.errorMessage = '';

    // Check for valid username and password
    if (!this.username || !this.password) {
      this.errorMessage = 'Username and password are required.';
      return;
    }

    // Call the AuthService to authenticate
    this.auth.authenticate(this.username, this.password).subscribe({
      next: (auth) => {
        if (auth) {
          // Save token and username to localStorage
          localStorage.setItem('headerValue', auth.headerValue);
          localStorage.setItem('userId', auth.userId.toString());
          localStorage.setItem('username', this.username); // Save username

          // Notify AuthService of successful login
          this.auth.handleLoginSuccess(auth.headerValue, auth.userId.toString(), this.username);

          // Redirect to tasks page
          this.router.navigate(['todolist']);
        }
      },
      error: (err) => {
        // Handle errors from the backend
        if (err.status === 401) {
          this.errorMessage = 'Invalid username or password.';
        } else {
          this.errorMessage = 'An error occurred. Please try again later.';
        }
      }
    });
  }
}
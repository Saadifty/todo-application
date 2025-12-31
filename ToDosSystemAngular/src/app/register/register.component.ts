import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule} from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule,CommonModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatCardModule, MatDialogModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username: string = '';
  email: string = '';
  password: string = '';
  errorMessage: string = '';
  successMessage: string = '';

  constructor(private userService: UserService, private router: Router) {}

  register(): void 
  {
    if (!this.username || !this.email || !this.password) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    this.userService.register({ username: this.username, email: this.email, password_hash: this.password })
      .subscribe({
        next: () => {
          this.successMessage = 'Registration successful. Redirecting to login...';
          setTimeout(() => this.router.navigate(['/login']), 2000); // Redirect after 2 seconds
        },
        error: (err) => {
          if (err.status === 409) {
            this.errorMessage = 'Username or email already exists.';
          } else {
            this.errorMessage = 'An error occurred. Please try again later.';
          }
        }
      });
  }

  cancel(): void {
    // Clear form fields
    this.username = '';
    this.email = '';
    this.password = '';
    this.errorMessage = '';
    this.successMessage = '';

    // Redirect to the login page
    this.router.navigate(['/login']);
  }
}

import { Component } from '@angular/core';
import { Todo } from '../model/todo';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatCardModule} from '@angular/material/card';
import { TodoService } from '../services/todo.service';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-todo',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatCardModule,
    MatDialogModule
  ],
  templateUrl: './add-todo.component.html',
  styleUrls: ['./add-todo.component.css'],
})

export class AddTodoComponent {
  // Define a new todo object
  newTodo: Todo = {
    id: 0,
    user_id: Number(localStorage.getItem('userId')), // Retrieve user ID from local storage
    title: '',
    description: '',
    priority: 'low', 
    is_completed: false, 
    created_at: new Date(), 
    completed_at: new Date(), 
  };

  constructor(
    private todoService: TodoService, // Inject TodoService for CRUD operations
    public dialogRef: MatDialogRef<AddTodoComponent>
  ) {}

  // Save the new todo using TodoService's createTodo method
  saveTodo(): void {
    // Validate user ID before proceeding
    if (!this.newTodo.user_id) {
      console.error('Invalid user ID. Please log in.');
      alert('Error: User ID is missing or invalid.');
      return;
    }

    this.todoService.createTodo(this.newTodo).subscribe({
      next: (createdTodo) => {
        this.dialogRef.close(createdTodo); // Pass the created todo back to the parent
      },
      error: (error) => {
        alert('Failed to add todo. Please try again.');
      },
    });
  }

  // Cancel the dialog
  cancel(): void {
    this.dialogRef.close(); 
  }
}
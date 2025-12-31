import { Component, OnInit } from '@angular/core';
import { Todo } from '../model/todo';
import { TodoService } from '../services/todo.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TodoComponent } from "../todo/todo.component";
import { MatDialogModule } from '@angular/material/dialog';
import { AddTodoComponent } from '../add-todo/add-todo.component';
import { MatDialog } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-todolist',
  standalone: true,
  imports: [CommonModule, FormsModule, TodoComponent, MatDialogModule, MatButtonModule, MatIconModule],
  templateUrl: './todolist.component.html',
  styleUrls: ['./todolist.component.css']
})
export class TodolistComponent implements OnInit {
  todos: Todo[] = []; // Holds the list of todos
  userName: string = '';

  constructor(private todoService: TodoService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.getTodos(); // Fetch todos when the component initializes
    this.userName = localStorage.getItem('username') || 'User'; // Fallback to 'User' if name is missing
  }

  // Fetch the list of todos from the service
  getTodos(): void 
  {
    const userId = Number(localStorage.getItem('userId')); // Fetch userId from localStorage
    if (!userId) {
      console.error('No userId found in localStorage.');
      return;
    }

    this.todoService.getTodos(userId).subscribe({
      next: (todos) => {
        this.todos = todos; // Update the local list
      },
      error: (error) => {
        console.error('Error fetching todos:', error);
      },
    });
  }
  
  // Open the AddTodo dialog
  openAddDialog(): void {
    const dialogRef = this.dialog.open(AddTodoComponent, {
      width: '400px',
    });
  
    dialogRef.afterClosed().subscribe((newTodo) => {
      console.log('Dialog closed with:', newTodo); // Log what is returned
        this.getTodos(); // Refresh the todo list
    });
  }
}
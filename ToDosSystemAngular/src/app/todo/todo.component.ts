import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Todo } from '../model/todo';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { EditTodoComponent } from '../edit-todo/edit-todo.component';
import { MatDialog } from '@angular/material/dialog';
import { TodoService } from '../services/todo.service';
import { MatIconModule } from '@angular/material/icon';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-todo',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule, MatCheckboxModule, FormsModule],
  templateUrl: './todo.component.html',
  styleUrl: './todo.component.css',
})
export class TodoComponent {
  @Input() todo!: Todo; 
  @Output() updated = new EventEmitter<void>(); 
  @Output() deleted = new EventEmitter<void>(); 

  constructor(private dialog: MatDialog, private todoService: TodoService) {}

  openEditDialog(): void 
  {
    const dialogRef = this.dialog.open(EditTodoComponent, {
      width: '400px',
      data: { ...this.todo }, // Pass a copy of the todo to the dialog
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.updated.emit(); 
      }
    });

  }
   deleteTodo(): void 
   {
    if (confirm('Are you sure you want to delete this task?')) {
      this.todoService.deleteTodo(this.todo.id!).subscribe({
        next: () => {
          this.deleted.emit(); // Notify parent to refresh the list
        },
        error: (error) => {
          console.error('Error deleting todo:', error);
          alert('Failed to delete todo. Please try again.');
        },
      });
    }
  }
  
  toggleCompletion(): void {
    this.todo.is_completed = !this.todo.is_completed;
  
    this.todoService.editTodo(this.todo).subscribe({
      next: () => {
        console.log('Todo updated successfully.');
      },
      error: (error) => {
        console.error('Error updating todo:', error);
      },
    });
  }
}
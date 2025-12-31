import { Component, Inject, Input, Output, EventEmitter } from '@angular/core';
import { MatDialogModule, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Todo } from '../model/todo';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { TodoService } from '../services/todo.service';

@Component({
  selector: 'app-edit-todo',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatDialogModule
  ],
  templateUrl: './edit-todo.component.html',
  styleUrls: ['./edit-todo.component.css'],
})

export class EditTodoComponent {
  @Input() todo!: Todo; // Receives the todo to edit
  @Output() todoUpdated = new EventEmitter<Todo>(); // Emit the updated todo
  @Output() canceled = new EventEmitter<void>(); // Emit a cancel event

  constructor(
    private todoService: TodoService,
    public dialogRef: MatDialogRef<EditTodoComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Todo
  ) {
    this.todo = { ...data }; // Initialize the todo with a copy of the input data
  }

  // Save the todo and close the dialog
  saveTodo(): void {
  this.todoService.editTodo(this.todo).subscribe(() => {
    this.todoUpdated.emit(this.todo); // Emit the updated todo
    this.dialogRef.close(this.todo); 
  });
}

// Cancel editing and close the dialog
  cancelEdit(): void {
  this.canceled.emit(); 
  this.dialogRef.close(); 
}
}
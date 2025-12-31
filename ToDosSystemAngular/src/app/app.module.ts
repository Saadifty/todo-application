import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 
import { routes } from './app.routes'; 
import { AppComponent } from './app.component';
import { TodolistComponent } from './todolist/todolist.component';
import { EditTodoComponent } from './edit-todo/edit-todo.component';
import { TodoService } from './services/todo.service';

@NgModule({
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes), // Use the routes here
    AppComponent,                    // Import standalone components
    TodolistComponent,
    EditTodoComponent
  ],
  providers: [TodoService]
})
export class AppModule { }
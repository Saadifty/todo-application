import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TodolistComponent } from './todolist/todolist.component';
import { EditTodoComponent } from './edit-todo/edit-todo.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { AddTodoComponent } from './add-todo/add-todo.component';
import { AuthGuard } from './auth.guard';

export const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent  },
  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route
  { path: 'todolist', component: TodolistComponent, canActivate: [AuthGuard] }, 
  { path: 'add-todo', component: AddTodoComponent, canActivate: [AuthGuard] },
  { path: 'edit-todo/:id', component: EditTodoComponent, canActivate: [AuthGuard] } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TodoService } from './todo.service';
import { Todo } from '../model/todo';

describe('TodoService', () => {
  let service: TodoService;
  let httpMock: HttpTestingController;

  const mockTodo: Todo = {
    id: 1,
    user_id: 1,
    title: 'Test Todo',
    description: 'Test Description',
    priority: 'low',
    is_completed: false,
    created_at: new Date(),
    completed_at: new Date()
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule], 
    });

    service = TestBed.inject(TodoService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    // Ensure there are no outstanding HTTP requests
    httpMock.verify();
  });

  describe('createTodo', () => {
    it('should create a new todo', () => {
      service.createTodo(mockTodo).subscribe(todo => {
        expect(todo).toEqual(mockTodo);
      });

      const req = httpMock.expectOne('http://localhost:5269/api/todo');
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockTodo);  // Verify the request body is the todo object
      req.flush(mockTodo);  // Simulate a successful response
    });

    
  });
});
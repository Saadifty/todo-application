namespace ToDosAdminSystem.Tests;
using ToDosAdminSystem.Model.Entities;
using ToDosAdminSystem.API.Controllers;
using ToDosAdminSystem.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

[TestClass]
    public class DeleteToDosTests
    {
        private Mock<TodoRepository> _mockRepository;
        private TodoController _controller;

        [TestInitialize]
        public void Setup()
        {
            // Arrange: Initialize the mock repository
            _mockRepository = new Mock<TodoRepository>(null);
            _controller = new TodoController(_mockRepository.Object);
        }

        [TestMethod]
        public void DeleteTodo_ShouldReturnNoContent_WhenTodoExists()
        {
            // Arrange: Set up the mock to return a todo when searching by ID
            var todoId = 1;
            _mockRepository.Setup(repo => repo.GetTodoById(todoId)).Returns(new Todo(todoId) { title = "Test Todo" });
            _mockRepository.Setup(repo => repo.DeleteToDo(todoId)).Returns(true);

            // Act: Call the controller method
            var result = _controller.DeleteTodo(todoId);

            // Assert: Verify the result is NoContent (HTTP 204)
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
        }

        [TestMethod]
        public void DeleteTodo_ShouldReturnNotFound_WhenTodoDoesNotExist()
        {
            // Arrange: Set up the mock to return null when searching for a todo
            var todoId = 999;  // Non-existing ID
            _mockRepository.Setup(repo => repo.GetTodoById(todoId)).Returns((Todo)null);

            // Act: Call the controller method
            var result = _controller.DeleteTodo(todoId);

            // Assert: Verify the result is NotFound (HTTP 404)
            var notFoundResult = result as NotFoundObjectResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual($"Todo with id {todoId} not found", notFoundResult.Value);
        }

        
    }


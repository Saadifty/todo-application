namespace ToDosAdminSystem.Tests;
using ToDosAdminSystem.Model.Entities;

[TestClass]
public class TodoEdgeCaseTests
{
    [TestMethod]
    public void CountCompletedTodos_ShouldReturnCorrectCount()
    {
        // Arrange
        var todos = new List<Todo>
        {
            new Todo(1) { is_completed = true },
            new Todo(2) { is_completed = false },
            new Todo(3) { is_completed = true }
        };

        // Act
        var completedCount = todos.Count(t => t.is_completed);

        // Assert
        Assert.AreEqual(2, completedCount);
    }

    [TestMethod]
    public void CountCompletedTodos_ShouldReturnZero_WhenNoTodosCompleted()
    {
        // Arrange
        var todos = new List<Todo>
        {
            new Todo(1) { is_completed = false },
            new Todo(2) { is_completed = false }
        };

        // Act
        var completedCount = todos.Count(t => t.is_completed);

        // Assert
        Assert.AreEqual(0, completedCount);
    }
}

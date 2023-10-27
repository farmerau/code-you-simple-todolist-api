namespace TodoListApi.Models
{
    public class TodoItemModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public bool IsComplete { get; set; } = false;
    }
}

namespace TodoListApi.DataAccess.Entities
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsComplete { get; set; } = false;
    }
}
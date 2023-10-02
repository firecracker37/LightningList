namespace LightningList.Models
{
    public class TodoList : BaseModel
    {
        // Using Guid as the primary key

        public string Name { get; set; } = "My List";

        // Navigation property for one-to-many relationship
        public List<TodoItem> Items { get; set; } = new List<TodoItem>();
    }
}

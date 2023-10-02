namespace LightningList.Models
{
    public class TodoItem : BaseModel
    {

        public string Task { get; set; } = "My Item";

        public bool IsComplete { get; set; } = false;

        // Foreign key property
        public Guid TodoListId { get; set; }

        // Navigation property
        public TodoList TodoList { get; set; }
    }

}

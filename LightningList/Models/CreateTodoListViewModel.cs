using System.ComponentModel.DataAnnotations;

namespace LightningList.Models
{
    public class CreateTodoListViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}

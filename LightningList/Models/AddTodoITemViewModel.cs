using System.ComponentModel.DataAnnotations;


namespace LightningList.Models
{
    public class AddTodoITemViewModel
    {
        [Required]
        public string Task { get; set; }

    }
}

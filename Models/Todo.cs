using System.ComponentModel.DataAnnotations;

namespace ToDoAPI.Models
{
	public class Todo
	{
		[Key]
		public int Id { get; set; }
		public string TodoName { get; set; }
	}
}

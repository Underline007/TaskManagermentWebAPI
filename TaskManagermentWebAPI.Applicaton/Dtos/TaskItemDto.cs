using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagermentWebAPI.Applicaton.Dtos
{
    public class TaskItemDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public bool IsCompleted { get; set; }
    }
}

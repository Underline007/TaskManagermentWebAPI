using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagermentWebAPI.Applicaton.Dtos;
using TaskManagermentWebAPI.Entities.Models;

namespace TaskManagermentWebAPI.Applicaton.Interfaces
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        TaskItem GetTaskById(Guid id);
        TaskItem CreateTask(TaskItemDto taskItem);
        void EditTask(Guid Id,TaskItemDto taskItem);
        void DeleteTask(TaskItem taskItem);
        void BulkAddMultipleTasks(List<TaskItemDto> listAddTask);
        void BulkDeleteMultipleTasks(List<Guid> listDeleteTask);
    }
}

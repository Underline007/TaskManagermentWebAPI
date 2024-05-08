using System;
using System.Collections.Generic;
using System.Linq;
using TaskManagermentWebAPI.Applicaton.Dtos;
using TaskManagermentWebAPI.Applicaton.Interfaces;
using TaskManagermentWebAPI.Entities.Models;

namespace TaskManagermentWebAPI.Applicaton.Services
{
    public class TaskService : ITaskService
    {
        private static List<TaskItem> TaskData = new List<TaskItem>
        {
            new TaskItem {Id = Guid.NewGuid(), Title = "Task 1", IsCompleted = true},
            new TaskItem {Id = Guid.NewGuid(), Title = "Task 2", IsCompleted = false},
            new TaskItem {Id = Guid.NewGuid(), Title = "Task 3", IsCompleted = true},
        };

        public List<TaskItem> GetAllTasks()
        {
            return TaskData;
        }

        public TaskItem GetTaskById(Guid id)
        {
            return TaskData.FirstOrDefault(x => x.Id == id);
        }

        public TaskItem CreateTask(TaskItemDto taskItem)
        {
            var newTaskItem = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = taskItem.Title,
                IsCompleted = taskItem.IsCompleted
            };
            TaskData.Add(newTaskItem);
            return newTaskItem;
        }

        public void EditTask(Guid Id, TaskItemDto taskItem)
        {
            var existingTaskItem = TaskData.FirstOrDefault(x => x.Id == Id);
            if (existingTaskItem != null)
            {
                existingTaskItem.Title = taskItem.Title;
                existingTaskItem.IsCompleted = taskItem.IsCompleted;
            }
        }

        public void DeleteTask(TaskItem taskItem)
        {
            var existingTaskItem = TaskData.FirstOrDefault(x => x.Id == taskItem.Id);
            if (existingTaskItem != null)
            {
                TaskData.Remove(existingTaskItem);
            }
        }

        public void BulkAddMultipleTasks(List<TaskItemDto> listAddTask)
        {
            var newTaskItems = listAddTask.Select(newTask => new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = newTask.Title,
                IsCompleted = newTask.IsCompleted
            });
            TaskData.AddRange(newTaskItems);
        }

        public void BulkDeleteMultipleTasks(List<Guid> listDeleteTask)
        {
            if (listDeleteTask == null || listDeleteTask.Count == 0)
            {
                throw new ArgumentException("List of task IDs to delete is null or empty.");
            }

            var existingTaskIds = TaskData.Select(task => task.Id).ToList();

            foreach (var taskId in listDeleteTask)
            {
                if (!existingTaskIds.Contains(taskId))
                {
                    throw new ArgumentException($"Task with ID {taskId} does not exist.");
                }
            }

            TaskData.RemoveAll(x => listDeleteTask.Contains(x.Id));
        }

    }
}

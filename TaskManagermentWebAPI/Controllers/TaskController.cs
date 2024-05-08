using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using TaskManagermentWebAPI.Applicaton.Dtos;
using TaskManagermentWebAPI.Applicaton.Interfaces;
using TaskManagermentWebAPI.Entities.Models;

namespace TaskManagermentWebAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/v1/tasks")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            try
            {
                var tasks = _taskService.GetAllTasks();
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all tasks");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById(Guid id)
        {
            try
            {
                var task = _taskService.GetTaskById(id);
                if (task == null)
                {
                    return NotFound($"Not found task with {id}");
                }
                return Ok(task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting task");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskItemDto taskItem)
        {
            try
            {
                if (taskItem != null)
                {
                    var createdTask = _taskService.CreateTask(taskItem);
                    return CreatedAtAction(nameof(GetTaskById), new { id = createdTask.Id }, createdTask);
                }
                return BadRequest("Task is null");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating task");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditTask(Guid id, [FromBody] TaskItemDto taskItemDto)
        {
            try
            {
                _taskService.EditTask(id, taskItemDto);
                return Ok("Task edited successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while editing task");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(Guid id)
        {
            try
            {
                var taskToDelete = _taskService.GetTaskById(id);
                if (taskToDelete == null)
                {
                    return NotFound($"Task with id {id} not found");
                }
                _taskService.DeleteTask(taskToDelete);
                return Ok("Task deleted successfully."); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting task");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("bulk")]
        public IActionResult BulkAddTasks([FromBody] List<TaskItemDto> listAddTask)
        {
            try
            {
                _taskService.BulkAddMultipleTasks(listAddTask);
                return CreatedAtAction(nameof(GetAllTasks), new { });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when adding tasks");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("bulk")]
        public IActionResult BulkDeleteTasks([FromBody] List<Guid> listDeleteTask)
        {
            try
            {
                if (listDeleteTask == null || listDeleteTask.Count == 0)
                {
                    return BadRequest("List task is null or empty.");
                }

                var existingTaskIds = _taskService.GetAllTasks().Select(task => task.Id).ToList();

                foreach (var taskId in listDeleteTask)
                {
                    if (!existingTaskIds.Contains(taskId))
                    {
                        return NotFound($"Task with ID {taskId} does not exist.");
                    }
                }

                _taskService.BulkDeleteMultipleTasks(listDeleteTask);
                return Ok("Tasks deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when deleting tasks");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}

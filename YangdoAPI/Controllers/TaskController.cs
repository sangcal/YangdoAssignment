using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YangdoDTO;
using YangdoService;

namespace YangdoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase, IController<Task>
    {
        /// <summary>
        /// Basically, this web api is operated following steps
        /// Ex) [YangdoAPI.TaskController] -> [YangdoService.TaskService] -> [YangdoDAO.Repository] -> DB
        /// All of DI objects should be registered in ConfigureServices method of Startup.cs
        /// </summary>

        // Declare DI Object
        private readonly IBasicService<Task> taskService;

        // Inject Dependent Object
        // This DI Object will play roles relevant to DB connected
        public TaskController(TaskService taskService)
        {
            this.taskService = taskService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var task = taskService.GetById(id);

                if (task != null)
                {
                    // Serialize : .Net Object -> other kind of object
                    // Json Serialize : .NET Object -> Json object
                    var jsonResult = JsonConvert.SerializeObject(
                    task,
                    Formatting.None,
                    new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });


                    return Ok(jsonResult);
                }
                else
                {
                    // status code : 404 (Not Found)
                    // The server can not find the requested resource. In the browser, this means the URL is not recognized.
                    return NotFound();
                }
            }
            catch (Exception Ex)
            {
                // status code : 500
                // The server has encountered a situation it doesn't know how to handle.
                return StatusCode(500, Ex.Message);
            }
        }

        [HttpGet("GetList")]
        public IActionResult GetList()
        {
            try
            {
                var tasks = taskService.GetList();

                if (tasks.Count() != 0)
                {
                    // Serialize : .Net Object -> other kind of object
                    // Json Serialize : .NET Object -> Json object
                    var jsonResult = JsonConvert.SerializeObject(
                        tasks,
                        Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

                    // status code : 200
                    // The request has succeeded. The meaning of the success depends on the HTTP method
                    return Ok(jsonResult);
                }
                else
                {
                    // status code : 404 (Not Found)
                    // The server can not find the requested resource. In the browser, this means the URL is not recognized.
                    return NotFound();
                }
            }
            catch (Exception Ex)
            {
                // status code : 500
                // The server has encountered a situation it doesn't know how to handle.
                return StatusCode(500, Ex.Message);
            }
        }

        // test value => jsonFIlters: {'taskname': 'create', 'taskdesc': 'report'}
        [HttpGet("GetListByFilters")]
        public IActionResult GetListByFilters(string jsonFilters)
        {
            // when filters are equal to not null, retrieve all data under the conditions handed over
            if (jsonFilters != null)
            {
                try
                {
                    // Deserialize : other kind of object -> .Net Object
                    // Json Deserialize : Json object -> .NET Object
                    Dictionary<string, string> filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFilters);

                    var tasks = taskService.GetListByFilters(filters);

                    if (tasks.Count() != 0)
                    {
                        // Serialize : .Net Object -> other kind of object
                        // Json Serialize : .NET Object -> Json object
                        var jsonResult = JsonConvert.SerializeObject(
                            tasks,
                            Formatting.None,
                            new JsonSerializerSettings()
                            {
                                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            });

                        // status code : 200
                        // The request has succeeded. The meaning of the success depends on the HTTP method
                        return Ok(jsonResult);
                    }
                    else
                    {
                        // status code : 404 (Not Found)
                        // The server can not find the requested resource. In the browser, this means the URL is not recognized.
                        return NotFound();
                    }
                }
                catch (Exception Ex)
                {
                    // status code : 500
                    // The server has encountered a situation it doesn't know how to handle.
                    return StatusCode(500, Ex.Message);
                }

            }
            // when filters are equal to null, retrieve all data
            else
            {
                return this.GetList();
            }

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var task = taskService.GetById(id);

                if (task != null)
                {
                    // status code : 200
                    // The request has succeeded. The meaning of the success depends on the HTTP method
                    taskService.Delete(task);
                    return Ok(task);

                }
                else
                {
                    // status code : 404 (Not Found)
                    // The server can not find the requested resource. In the browser, this means the URL is not recognized.
                    return NotFound();
                }
            }
            catch (Exception Ex)
            {
                // status code : 500
                // The server has encountered a situation it doesn't know how to handle.
                return StatusCode(500, Ex.Message);
            }
        }

        // Insert
        [HttpPost]
        public IActionResult Post(Task entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskService.Insert(entity);

                    // status code : 201 (Created)
                    // The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
                    return CreatedAtAction(nameof(GetById), new { id = entity.TaskId }, entity);
                }
                catch (Exception Ex)
                {
                    // status code : 500 (Internal Server Error)
                    // The server has encountered a situation it doesn't know how to handle.
                    return StatusCode(500, Ex.Message);
                }
            }
            else
            {
                // status code : 400 (Bad Request)
                // The server could not understand the request due to invalid syntax.
                return BadRequest(ModelState);
            }
        }

        // Update
        [HttpPut]
        public IActionResult Put(Task entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskService.Update(entity);

                    // status code : 201 (Created)
                    // The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
                    return CreatedAtAction(nameof(GetById), new { id = entity.TaskId }, entity);
                }
                catch (Exception Ex)
                {
                    // status code : 500 (Internal Server Error)
                    // The server has encountered a situation it doesn't know how to handle.
                    return StatusCode(500, Ex.Message);
                }
            }
            else
            {
                // status code : 400
                // The server could not understand the request due to invalid syntax.
                return BadRequest(ModelState);
            }
        }
    }
}
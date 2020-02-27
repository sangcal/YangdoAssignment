using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YangdoDTO;
using YangdoService;

namespace YangdoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase, IController<Person>
    {
        /// <summary>
        /// Basically, this web api is operated following steps
        /// Ex) [YangdoAPI-PersonController] -> [YangdoService-PersonService] -> [YangdoDAO-Repository] -> DB
        /// All of DI objects should be registered in ConfigureServices method of Startup.cs
        /// </summary>


        // Declare DI Object
        private readonly IBasicService<Person> personService;

        // Inject Dependent Object
        // This DI Object will play roles relevant to DB connected
        public PersonController(PersonService personService)
        {
            this.personService = personService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var person = personService.GetById(id);

                if (person != null)
                {
                    // Serialize : .Net Object -> other kind of object
                    // Json Serialize : .NET Object -> Json object
                    var jsonResult = JsonConvert.SerializeObject(
                    person,
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
                var people = personService.GetList();

                if (people.Count() != 0)
                {
                    // Serialize : .Net Object -> other kind of object
                    // Json Serialize : .NET Object -> Json object
                    var jsonResult = JsonConvert.SerializeObject(
                        people,
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

        // test value => jsonFIlters: {'email': 'woc21', 'dob': '14/02/1981', 'firstname': 'yang'}
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

                    var people = personService.GetListByFilters(filters);

                    if (people.Count() != 0)
                    {
                        // Serialize : .Net Object -> other kind of object
                        // Json Serialize : .NET Object -> Json object
                        var jsonResult = JsonConvert.SerializeObject(
                            people,
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

            //var people = personService.GetList();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var person = personService.GetById(id);

                if (person != null)
                {
                    // status code : 200
                    // The request has succeeded. The meaning of the success depends on the HTTP method
                    personService.Delete(person);
                    return Ok(person);

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
        public IActionResult Post(Person entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    personService.Insert(entity);

                    // status code : 201 (Created)
                    // The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
                    return CreatedAtAction(nameof(GetById), new { id = entity.PersonId }, entity);
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
        public IActionResult Put(Person entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    personService.Update(entity);

                    // status code : 201 (Created)
                    // The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
                    return CreatedAtAction(nameof(GetById), new { id = entity.PersonId }, entity);
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
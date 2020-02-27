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
    public class TimeSheetController : ControllerBase, IController<TimeSheet>
    {
        /// <summary>
        /// Basically, this web api is operated following steps
        /// Ex) [YangdoAPI-TimeSheetController] -> [YangdoService-TimeSheetService] -> [YangdoDAO-Repository] -> DB
        /// All of DI objects should be registered in ConfigureServices method of Startup.cs
        /// </summary>


        // Declare DI Object
        private readonly IExtendedService<TimeSheet> timeSheetService;
        //private readonly IExtendedService<TimeSheetHours> timeSheetHoursService;

        // Inject Dependent Object
        // This DI Object will play roles relevant to DB connected
        public TimeSheetController(TimeSheetService timeSheetService)
        {
            this.timeSheetService = timeSheetService;
            //this.timeSheetHoursService = timeSheetHoursService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var timesheet = timeSheetService.GetById(id);

                if (timesheet != null)
                {
                    // Serialize : .Net Object -> other kind of object
                    // Json Serialize : .NET Object -> Json object
                    var jsonResult = JsonConvert.SerializeObject(
                    timesheet,
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
                var timesheets = timeSheetService.GetList();

                if (timesheets.Count() != 0)
                {
                    // Serialize : .Net Object -> other kind of object
                    // Json Serialize : .NET Object -> Json object
                    var jsonResult = JsonConvert.SerializeObject(
                        timesheets,
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

        // {'timefrom': '25/02/2020 00:00:00', 'timeto': '28/02/2020 00:00:00'}
        // {'personid': '1004', 'timefrom': '01/01/2020 00:00:00', 'timeto': '28/02/2020 00:00:00'}
        // {'taskid': '1003', 'timefrom': '01/01/2020 00:00:00', 'timeto': '28/02/2020 00:00:00'}
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

                    var timesheets = timeSheetService.GetListByFilters(filters);

                    if (timesheets.Count() != 0)
                    {
                        // Serialize : .Net Object -> other kind of object
                        // Json Serialize : .NET Object -> Json object
                        var jsonResult = JsonConvert.SerializeObject(
                            timesheets,
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
                var timesheet = timeSheetService.GetById(id);

                if (timesheet != null)
                {
                    // status code : 200
                    // The request has succeeded. The meaning of the success depends on the HTTP method
                    timeSheetService.Delete(timesheet);
                    return Ok(timesheet);

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
        public IActionResult Post(TimeSheet entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    timeSheetService.Insert(entity);

                    // status code : 201 (Created)
                    // The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
                    return CreatedAtAction(nameof(GetById), new { id = entity.TimeSheetId }, entity);
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
        public IActionResult Put(TimeSheet entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    timeSheetService.Update(entity);

                    // status code : 201 (Created)
                    // The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
                    return CreatedAtAction(nameof(GetById), new { id = entity.TimeSheetId }, entity);
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


        // jsonFilters => timeFrom, timeTo
        // {'timefrom': '25/02/2020 00:00', 'timeto': '26/02/2020 00:00'}
        [HttpGet("GetHoursOfTimeRange")]
        public IActionResult GetHoursOfTimeRange(string jsonFilters)
        {
            // when filters are equal to not null, retrieve all data under the conditions handed over
            if (jsonFilters != null)
            {
                try
                {
                    // Deserialize : other kind of object -> .Net Object
                    // Json Deserialize : Json object -> .NET Object
                    Dictionary<string, string> filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFilters);

                    var timesheets = timeSheetService.GetHoursOfTimeRange(filters);

                    if (timesheets.Count() != 0)
                    {
                        var totalHours = timesheets.Sum(x => x.WorkedHours);
                        var result = timesheets.Take(1).Select(x => new { TotalHours = totalHours });

                        // Serialize : .Net Object -> other kind of object
                        // Json Serialize : .NET Object -> Json object
                        var jsonResult = JsonConvert.SerializeObject(
                            result,
                            Formatting.None);

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

        // jsonFilters => personid, timeFrom, timeTo
        // {'personid': '1', 'timefrom': '25/02/2020 00:00', 'timeto': '26/02/2020 00:00'}
        [HttpGet("GetHoursOfTimeRangeByPersonId")]
        public IActionResult GetHoursOfTimeRangeByPersonId(string jsonFilters)
        {
            // when filters are equal to not null, retrieve all data under the conditions handed over
            if (jsonFilters != null)
            {
                try
                {
                    // Deserialize : other kind of object -> .Net Object
                    // Json Deserialize : Json object -> .NET Object
                    Dictionary<string, string> filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFilters);

                    var timesheets = timeSheetService.GetHoursOfTimeRangeByPersonId(filters);

                    if (timesheets.Count() != 0)
                    {
                        var totalHoursByPersonId = timesheets.GroupBy(x => x.PersonId).Select(y => new { PersonId = y.Key, TotalHours = y.Sum(z => z.WorkedHours) });

                        // Serialize : .Net Object -> other kind of object
                        // Json Serialize : .NET Object -> Json object
                        var jsonResult = JsonConvert.SerializeObject(
                            totalHoursByPersonId,
                            Formatting.None);

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

        // jsonFilters => personid, timeFrom, timeTo
        // {'taskid': '1', 'timefrom': '25/02/2020 00:00', 'timeto': '26/02/2020 00:00'}
        [HttpGet("GetHoursOfTimeRangeByTaskId")]
        public IActionResult GetHoursOfTimeRangeByTaskId(string jsonFilters)
        {
            // when filters are equal to not null, retrieve all data under the conditions handed over
            if (jsonFilters != null)
            {
                try
                {
                    // Deserialize : other kind of object -> .Net Object
                    // Json Deserialize : Json object -> .NET Object
                    Dictionary<string, string> filters = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonFilters);

                    var timesheets = timeSheetService.GetHoursOfTimeRangeByTaskId(filters);

                    if (timesheets.Count() != 0)
                    {
                        var totalHoursByPersonId = timesheets.GroupBy(x => x.TaskId).Select(y => new { TaskId = y.Key, TotalHours = y.Sum(z => z.WorkedHours) });

                        // Serialize : .Net Object -> other kind of object
                        // Json Serialize : .NET Object -> Json object
                        var jsonResult = JsonConvert.SerializeObject(
                            totalHoursByPersonId,
                            Formatting.None);

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

    }
}
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Diagnostics;

namespace YangdoIntegrationTest
{
    public class TimeSheetApiIntegrationTest
    {
        [Fact]
        public async Task Test_GetById()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/timesheet/getbyid/2002");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetList()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/timesheet/getlist");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetListByFilters()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/timesheet/getlistbyfilters?jsonFilters={'personid': '1004', 'time': '26/02/2020 11:00'}");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_Post()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/timesheet", new StringContent(
                    JsonConvert.SerializeObject(
                        new YangdoDTO.TimeSheet() { PersonId = 1004, TaskId = 1003, TimeFrom = Convert.ToDateTime("2020-02-27 10:20:00"), TimeTo = Convert.ToDateTime("2020-02-27 12:37:00") }
                    )
                    , Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task Test_Put()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PutAsync("/api/timesheet", new StringContent(
                    JsonConvert.SerializeObject(
                        new YangdoDTO.TimeSheet() { TimeSheetId = 2002, PersonId = 1004, TaskId = 1003, TimeFrom = Convert.ToDateTime("2020-02-26 10:30:00"), TimeTo = Convert.ToDateTime("2020-02-26 12:30:00") }
                    )
                    , Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.Created);
            }
        }

        [Fact]
        public async Task Test_Delete()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.DeleteAsync("/api/timesheet/2002");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetHoursOfTimeRange()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/timesheet/GetHoursOfTimeRange?jsonFilters={'timefrom': '25/02/2020 00:00:00', 'timeto': '28/02/2020 00:00:00'}");

                response.EnsureSuccessStatusCode();
                
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetHoursOfTimeRangeByPersonId()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/timesheet/GetHoursOfTimeRangeByPersonId?jsonFilters={'personid': '1', 'timefrom': '25/02/2020 00:00:00', 'timeto': '26/02/2020 00:00:00'}");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetHoursOfTimeRangeByTaskId()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/timesheet/GetHoursOfTimeRangeByTaskId?jsonFilters={'taskid': '1', 'timefrom': '25/02/2020 00:00:00', 'timeto': '26/02/2020 00:00:00'}");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }


    }
}

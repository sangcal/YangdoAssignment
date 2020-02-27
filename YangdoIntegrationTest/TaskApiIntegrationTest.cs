using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace YangdoIntegrationTest
{
    public class TaskApiIntegrationTest
    {
        [Fact]
        public async Task Test_GetById()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/task/getbyid/1");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetList()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/task/getlist");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetListByFilters()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/task/getlistbyfilters?jsonFilters={'taskname': 'Design', 'taskdesc': 'Design'}");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_Post()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/task", new StringContent(
                    JsonConvert.SerializeObject(
                        new YangdoDTO.Task() { TaskName = "Design DB Schema", TaskDesc = "Design DB Schema - desc" }
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
                var response = await client.PutAsync("/api/task", new StringContent(
                    JsonConvert.SerializeObject(
                        new YangdoDTO.Task() { TaskId = 1002, TaskName = "Design DB Schema2", TaskDesc = "Design DB Schema2 - desc" }
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
                var response = await client.DeleteAsync("/api/task/1002");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}

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
    public class PersonApiIntegrationTest
    {
        [Fact]
        public async Task Test_GetById()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/person/getbyid/1");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
                //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async Task Test_GetList()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/person/getlist");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_GetListByFilters()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.GetAsync("/api/person/getlistbyfilters?jsonFilters={'email': 'woc21', 'dob': '14/02/1981', 'firstname': 'yang'}");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }

        [Fact]
        public async Task Test_Post()
        {
            using (var client = new TestClientProvider().Client)
            {
                var response = await client.PostAsync("/api/person", new StringContent(
                    JsonConvert.SerializeObject(
                        new YangdoDTO.Person() { FirstName = "KilDong", LastName="Hong", Email="abc@naver.com", Phone = "01012345678", DOB = Convert.ToDateTime("14/02/1981") }
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
                var response = await client.PutAsync("/api/person", new StringContent(
                    JsonConvert.SerializeObject(
                        new YangdoDTO.Person() { PersonId=1003, FirstName = "KilDong2", LastName = "Hong2", Email = "abc2@naver.com", Phone = "01052345678", DOB = Convert.ToDateTime("15/02/1981") }
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
                var response = await client.DeleteAsync("/api/person/1003");

                response.EnsureSuccessStatusCode();

                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}

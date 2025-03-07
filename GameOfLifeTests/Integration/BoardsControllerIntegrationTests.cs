using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using GameOfLifeApi.Models.DTO;
using Newtonsoft.Json;

namespace GameOfLifeTests.Integration
{
    public class BoardsControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public BoardsControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateBoardFromJson_ValidInput_ReturnsOkAndBoardId()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false, false },
                new bool[] { true, true, true },
                new bool[] { false, false, false }
            };

            var response = await _client.PostAsJsonAsync("api/boards", boardState);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDTO>();
            Assert.NotNull(result?.data);
        }

        [Fact]
        public async Task GetNextState_WithPrettyOutput_ReturnsStateAndAscii()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false, false },
                new bool[] { true, true, true },
                new bool[] { false, false, false }
            };

            var createResponse = await _client.PostAsJsonAsync("api/boards", boardState);
            createResponse.EnsureSuccessStatusCode();
            var createResult = await createResponse.Content.ReadFromJsonAsync<ApiResponseDTO>();
            object boardId = createResult.data;

            var nextResponse = await _client.PutAsync($"api/boards/{boardId}/next", null);

            nextResponse.EnsureSuccessStatusCode();
            var result = await nextResponse.Content.ReadFromJsonAsync<ApiResponseDTO>();
            Assert.NotNull(result?.data);
            Assert.NotNull(result?.asciiData);
        }

        [Fact]
        public async Task GetStateAfterSteps_ValidSteps_ReturnsUpdatedState()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { true, false, true },
                new bool[] { false, true, false },
                new bool[] { true, false, true }
            };

            var createResponse = await _client.PostAsJsonAsync("api/boards", boardState);
            createResponse.EnsureSuccessStatusCode();
            var createResult = await createResponse.Content.ReadFromJsonAsync<ApiResponseDTO>();
            object boardId = createResult.data;

            var nextResponse = await _client.PutAsync($"api/boards/{boardId}/advance/3", null);

            nextResponse.EnsureSuccessStatusCode();
            var result = await nextResponse.Content.ReadFromJsonAsync<ApiResponseDTO>();
            Assert.NotNull(result?.data);
            Assert.NotNull(result?.asciiData);
        }

        [Fact]
        public async Task GetFinalState_StableBoard_ReturnsFinalState()
        {
            bool[][] boardState = new bool[][]
            {
                new bool[] { false, false, false },
                new bool[] { false, false, false },
                new bool[] { false, false, false }
            };

            var createResponse = await _client.PostAsJsonAsync("api/boards", boardState);
            createResponse.EnsureSuccessStatusCode();
            var createResult = await createResponse.Content.ReadFromJsonAsync<ApiResponseDTO>();
            object boardId = createResult.data;

            var finalResponse = await _client.PutAsync($"api/boards/{boardId}/final", null);

            finalResponse.EnsureSuccessStatusCode();
            var result = await finalResponse.Content.ReadFromJsonAsync<ApiResponseDTO>();
            Assert.NotNull(result?.data);
            Assert.NotNull(result?.asciiData);
        }
    }
}

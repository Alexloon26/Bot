using System.Net.Http;
using System.Threading.Tasks;
using BrawlHelper.Models;
using Newtonsoft.Json;

namespace BrawlHelper.Model
{
    public class BrawlClient
    {
        private readonly string _address;
        private readonly string _apiKey;

        public BrawlClient()
        {
            _address = Constants.Address;
            _apiKey = Constants.ApiKey;
        }

        public async Task<Player> GetPlayerInfo(string playerId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.GetAsync($"{_address}/v1/players/%23{playerId}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var player = JsonConvert.DeserializeObject<Player>(jsonResponse);

            return player;
        }

        public async Task<Brawlers> GetBrawlers()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.GetAsync($"{_address}/v1/brawlers");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var brawlers = JsonConvert.DeserializeObject<Brawlers>(jsonResponse);

            return brawlers;
        }
        public async Task<Club> GetClubInfo(string clubId)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.GetAsync($"{_address}/v1/clubs/%23{clubId}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var club = JsonConvert.DeserializeObject<Club>(jsonResponse);

            return club;
        }
        public async Task<List<EventItem>> GetEvents()
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            client.DefaultRequestHeaders.Add("accept", "application/json");

            var response = await client.GetAsync($"{_address}/v1/events/rotation");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var events = JsonConvert.DeserializeObject<List<EventItem>>(jsonResponse);

            return events;
        }
    }
}
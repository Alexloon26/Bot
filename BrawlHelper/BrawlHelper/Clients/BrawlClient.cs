using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BrawlHelper.Models;
using Newtonsoft.Json;

namespace BrawlHelper.Model
{
    public class BrawlClient
    {
        private static string _address;
        private static string _apiKey;

        public BrawlClient()
        {
            _address = Constants.Address;
            _apiKey = Constants.ApiKey;
        }

        public async Task<Player> GetPlayerInfo(string playerId)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var response = await client.GetAsync($"{_address}/v1/players/{playerId}");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var player = JsonConvert.DeserializeObject<Player>(jsonResponse);

                return player;
            }
        }

        public async Task<Brawlers> GetBrawlers()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
                client.DefaultRequestHeaders.Add("accept", "application/json");

                var response = await client.GetAsync($"{_address}/v1/brawlers");
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var brawlers = JsonConvert.DeserializeObject<Brawlers>(jsonResponse);

                return brawlers;
            }
        }
    }
}
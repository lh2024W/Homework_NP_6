using Newtonsoft.Json;

namespace Homework_NP_6
{
   class Program
    {
        static HttpClient httpClient = new HttpClient();
        private const string BaseUrl = "https://api.random.org/json-rpc/2/invoke";

        static async Task Main()
        {
            var result = await RollDiceAsync(2);

            Console.WriteLine("Welcome to the Dice Game!");

            Console.WriteLine("Choose a node: ");
            Console.WriteLine("1. Player vs Player");
            Console.WriteLine("2. Player vs Computer");

            int node = int.Parse(Console.ReadLine());

            if (node == 1)
            {
                await PlayPlayerVsPlayer();
            }
            else if (node == 2)
            {
                await PlayPlayerVsComputer();
            }
            else
            {
                Console.WriteLine("Invalid node. Exiting...");
            }
        }

        static async Task PlayPlayerVsComputer()
        {
            Console.WriteLine("Player, press Enter to roll the dice.");
            Console.ReadLine();
            int playerRoll = await RollDiceAsync(6);
            Console.WriteLine($"Player rolled: {playerRoll}");

            Console.WriteLine("Computer is rolling the dice...");
            int computerRoll = await RollDiceAsync(6);
            Console.WriteLine($"Computer rolled: {computerRoll}");

            if (playerRoll > computerRoll)
            {
                Console.WriteLine("Player wins!");
            }
            else if (playerRoll < computerRoll)
            {
                Console.WriteLine("Computer wins!");
            }
            else
            {
                Console.WriteLine("It`s a tie!");
            }
        }

        static async Task PlayPlayerVsPlayer()
        {
            Console.WriteLine("Player 1, press Enter to roll the dice.");
            Console.ReadLine();
            int player1Roll = await RollDiceAsync(6);
            Console.WriteLine($"Player 1 rolled: {player1Roll}");

            Console.WriteLine("Player 2 is rolling the dice...");
            int player2Roll = await RollDiceAsync(6);
            Console.WriteLine($"Player rolled: {player2Roll}");

            if (player1Roll > player2Roll)
            {
                Console.WriteLine("Player 1 wins!");
            }
            else if (player1Roll < player2Roll)
            {
                Console.WriteLine("Player 2 wins!");
            }
            else
            {
                Console.WriteLine("It`s a tie!");
            }
        }

        static async Task<int> RollDiceAsync(int sides)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                string apiKey = "159b42dd-de30-48f6-88db-4b3009b4af2b"; 
               //мой ключ пока заблокирован string apiKey = "afd20b41-4a78-4216-835a-51633bf01572"; (попробовать позже)

                var content = new StringContent(
                    $"{{\"jsonrpc\":\"2.0\",\"method\":\"generateIntegers\",\"params\":{{\"apiKey\":\"{apiKey}\",\"n\":1,\"min\":1,\"max\":{sides},\"replacement\":true}},\"id\":1}}",
                    System.Text.Encoding.UTF8,
                    "application/json"
                    );

                var response = await client.PostAsync("", content);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(json);
                    return result.result.random.data[0];
                }
                else
                {
                    throw new Exception("Failed to generate random numder.");
                }
            }
        }
    }
}

namespace AdjacentCountriesApp
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Adjacent Countries Finder";

            Dictionary<string, List<string>> countryNeighbors = InitializeCountryData();

            Console.WriteLine("---- Adjacent Countries Finder ----\n");
            Console.WriteLine("Enter a 2-letter Country Code (Example: IN, US, NZ)");
            Console.WriteLine("Type EXIT to close the program.\n");

            while (true)
            {
                Console.Write("Enter Country Code: ");
                string inputCode = Console.ReadLine().Trim().ToUpper();

                if (inputCode == "EXIT")
                {
                    Console.WriteLine("\nThank you for using the system!");
                    break;
                }

                if (string.IsNullOrWhiteSpace(inputCode) || inputCode.Length != 2)
                {
                    Console.WriteLine("Invalid input! Please enter a valid 2-letter country code.\n");
                    continue;
                }

                if (countryNeighbors.TryGetValue(inputCode, out List<string> neighbors))
                {
                    Console.WriteLine($"\nAdjacent Countries for {inputCode}:");

                    foreach (var country in neighbors)
                    {
                        Console.WriteLine(" - " + country);
                    }

                    Console.WriteLine();  
                }
                else
                {
                    Console.WriteLine("No data found for the provided country code.\n");
                }
            }
        }



        static Dictionary<string, List<string>> InitializeCountryData()
        {
            return new Dictionary<string, List<string>>()
            {
                { "IN", new List<string> { "Pakistan", "China", "Nepal", "Bhutan", "Bangladesh", "Myanmar" } },
                { "US", new List<string> { "Canada", "Mexico" } },
                { "NZ", new List<string> { "None – New Zealand is an island nation with no land borders" } },
                { "CA", new List<string> { "United States" } },
                { "CN", new List<string> 
                    { 
                        "India", "Nepal", "Bhutan", "Myanmar", "Russia", 
                        "Mongolia", "Pakistan", "Afghanistan", "North Korea", 
                        "Kazakhstan", "Kyrgyzstan", "Laos", "Vietnam" 
                    } 
                },
                { "AU", new List<string> { "None – Australia has no land borders" } }
            };
        }
    }
}

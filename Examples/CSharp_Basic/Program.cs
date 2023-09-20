// Example using the MyCirrus API
using System.Text.Json;

// define url and api key. Replace <YOUR_API_KEY> below
const string Url = "https://api.mycirrus.cloud/v1"; // MyCirrus API URL
const string Key = "<YOUR_API_KEY>"; // Your API key

// create http client and set the API Key header for all requests
using HttpClient client = new HttpClient();
client.DefaultRequestHeaders.Add("X-Api-Key", Key);

try
{
    // First, get the list of instruments
    var response = await client.GetStringAsync($"{Url}/control/instruments");

    // print the list of instruments
    Console.WriteLine("Instruments:");
    Console.WriteLine(response);


    // Get the serial number of the first instrument
    var instruments = JsonSerializer.Deserialize<Instrument[]>(response, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    var serial = instruments[0].SerialNumber;

    // Now get the instrument status
    var statusResponse = await client.GetStringAsync($"{Url}/control/instruments/{serial}/status");

    // print the status
    Console.WriteLine($"Status of {serial}:");
    Console.WriteLine(statusResponse);


    // Get today's measurements for this instrument
    var startDateTime = DateTime.UtcNow.Date;
    var endDateTime = startDateTime.AddDays(1);

    var measurementsResponse = await client.GetStringAsync($"{Url}/data/noise/measurements?instruments={serial}&start={startDateTime:O}&end={endDateTime:O}&values=LAeq,LCPeak,LAFmax");

    // print the measurements
    Console.WriteLine($"Measurements for {serial}:");
    Console.WriteLine(measurementsResponse);


    // finished
    Console.WriteLine("Finished");

    // see the api documentation for details on other endpoints and their usage

}
catch (HttpRequestException ex)
{
    Console.WriteLine("Error:");
    Console.WriteLine(ex);
}

public class Instrument
{
    public string? Type { get; set; }
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    // other properties omitted
}

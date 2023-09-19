// Example using the MyCirrus API with WebSockets
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

// Create the websocket
//   Include the instrument serial number in the url. Replace <YOUR_SERIAL>
//   And include the api key in the header. Replace <YOUR_API_KEY>
//   Alternatively you can include it in the query string ?x-api-key=XXXXXX
const string Key = "<YOUR_API_KEY>";
const string SerialNumber = "<YOUR_SERIAL>";
const string Url = $"wss://api.mycirrus.cloud/v1/live/{SerialNumber}";
using ClientWebSocket ws = new ClientWebSocket();
ws.Options.SetRequestHeader("X-Api-Key", Key);

try
{
    // Connect to the websocket
    await ws.ConnectAsync(new Uri(Url), CancellationToken.None);
    Console.WriteLine("Connected");

    // Handle incoming messages
    _ = ReceiveMessages(ws);

    // Setup keepalive
    _ = KeepAplive(ws);

    // Start live data
    //   comment this to avoid spamming the console when testing other commands
    Console.WriteLine("Starting Live Data");
    await SendJson(ws, new { action = "StartLiveData", types = new[] { "LAeq", "LAF" } });

    // There are various other actions you can perform here
    // Uncomment one of these lines to try it out
    //await SendJson(ws, new { action = "Identify" });
    //await SendJson(ws, new { action = "Calibration" });
    //await SendJson(ws, new { action = "SIC" });

    // Stop live data after 4 minutes
    await Task.Delay(TimeSpan.FromMinutes(4));
    Console.WriteLine("Stopping Live Data");
    await SendJson(ws, new { action = "StopLiveData" });

    // Disconnect after 5 minutes
    await Task.Delay(TimeSpan.FromMinutes(1));
    ws.Abort();
}
catch (Exception ex)
{
    Console.WriteLine("Error: ");
    Console.WriteLine(ex);
}

async Task SendJson(ClientWebSocket ws, object data)
{
    var json = JsonSerializer.Serialize(data);
    var buffer = Encoding.UTF8.GetBytes(json);
    await ws.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
}

async Task ReceiveMessages(ClientWebSocket ws)
{
    while (ws.State == WebSocketState.Open)
    {
        var buffer = new byte[1024];
        var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

        if (result.MessageType == WebSocketMessageType.Text)
        {
            var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
            HandleIncomingMessage(json);
        }
    }

    // if server closes the websocket check the reason here
    if (ws.CloseStatus is not null)
    {
        Console.WriteLine("Disconnected: " + ws.CloseStatusDescription);

        Environment.Exit(0);
    }
}

async Task KeepAplive(ClientWebSocket ws)
{
    while (ws.State == WebSocketState.Open)
    {
        // Trigger every 30 seconds to keep the connection alive
        await Task.Delay(TimeSpan.FromSeconds(30));
        await SendJson(ws, new { action = "KeepAlive" });
    }
}

void HandleIncomingMessage(string json)
{
    var msg = JsonSerializer.Deserialize<Message>(json);

    if (msg.action == "KeepAlive")
    {
        // The server responded to our keepalive, nothing to do
        return;
    }

    switch (msg.type)
    {
        case "LiveData":
            // This is the live data
            Console.WriteLine("Live Data: " + msg.time);
            Console.WriteLine(msg.values);
            break;
        case "InstrumentStatus":
            // If the instrument status changes, this shows the new status
            // This is also sent when the connection is first made
            Console.WriteLine("Instrument Status: ");
            Console.WriteLine(json);
            break;
        case "ActionStatus":
            // This is the response to an Identify, Calibration, or SIC action
            // There may be multiple responses to a single action
            Console.WriteLine("Action Status: ");
            Console.WriteLine(json);
            break;
        default:
            // Unknown message type
            Console.WriteLine("Unknown Message Type: " + msg.type);
            Console.WriteLine(json);
            break;
    }
}

class Message
{
    public string? action { get; set; }
    public string? type { get; set; }
    public string? time { get; set; }
    public object? values { get; set; }
}

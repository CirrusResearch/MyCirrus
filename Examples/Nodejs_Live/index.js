// Example using the MyCirrus API with WebSockets
// This uses the ws library - https://github.com/websockets/ws
const WebSocket = require('ws');

// Create and connect the websocket
//   Include the instrument serial number in the url. Replace <YOUR_SERIAL>
//   And include the api key in the header. Replace <YOUR_API_KEY>
//   Alternatively you can include it in the query string ?x-api-key=XXXXXX
const ws = new WebSocket('wss://api.mycirrus.cloud/v1/live/<YOUR_SERIAL>', {
    headers: { 'X-Api-Key': '<YOUR_API_KEY>' }
});

// this runs once the connection is open
ws.on('open', function open() {
  console.log('Connected');

  // setup keepalive
  setInterval(function keepalive() {
    // Trigger every 30 seconds to keep the connection alive
    ws.send('{ "action":"KeepAlive" }');
  }, 30 * 1000);

  // start live data
  //   comment this to avoid spamming the console when testing other commands
  console.log('Starting Live Data');
  ws.send('{ "action":"StartLiveData", "types":["LAeq", "LAF"] }');

  // there are various other actions we can perform here
  //   uncomment one of these lines to try it out
  //ws.send('{ "action":"Identify" }');
  //ws.send('{ "action":"Calibration" }');
  //ws.send('{ "action":"SIC" }');

  // stop live data after 4 minutes
  setTimeout(function stoplive() {
    console.log('Stopping Live Data');
    ws.send('{ "action":"StopLiveData" }');
  }, 4 * 60 * 1000);

  // disconnect after 5 minutes
  setTimeout(function end() {
    ws.close();
  }, 5 * 60 * 1000);
});

// this runs once the connection is closed
ws.on('close', function close(code, reason) {
  console.log('Disconnected: ' + reason.toString());

  process.exit();
});

// this runs when a message is received
ws.on('message', function incoming(data) {
  let msg = JSON.parse(data);

  if (msg.action == 'KeepAlive') {
    // the server responded to our keepalive, nothing to do
    return;
  }

  switch (msg.type) {
    case 'LiveData':
      // this is the live data
      console.log("Live Data: " + msg.time);
      console.log(msg.values);
      break;
    case 'InstrumentStatus':
      // if the instrument status changes this shows the new status
      //   this is also sent when the connection is first made
      console.log("Instrument Status: ");
      console.log(msg);
      break;
    case 'ActionStatus':
      // this is the response to an Identify, Calibration or SIC action
      //   there may be multiple responses to a single action
      console.log("Action Status: ");
      console.log(msg);
      break;
    default:
      // unknown message type
      console.log("Unknown Message Type: " + msg.type);
      console.log(msg);
      break;
  }
});

// this runs if there is an error
ws.on('error', function error(err) {
  console.log('Error: ');
  console.log(err);
});

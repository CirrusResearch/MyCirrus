// Example using the MyCirrus API

// define url and api key. Replace <YOUR_API_KEY> below
const url = 'https://api.mycirrus.cloud/v1'; // MyCirrus API URL
const key = '<YOUR_API_KEY>' // Your API key

// run the example
runExample();

async function runExample() {
  try {
    // first get the list of instruments
    const response = await fetch(url + '/control/instruments', { headers: { 'X-Api-Key': key } });
    const instruments = await response.json();

    // print the list of instruments
    console.log('Instruments:');
    console.log(instruments);


    // get the serial number of the first instrument
    const serial = instruments[0].serialNumber;

    // now get the instrument status
    const response2 = await fetch(url + '/control/instruments/' + serial + '/status', { headers: { 'X-Api-Key': key } });
    const status = await response2.json();

    // print the status
    console.log('Status of ' + serial + ':');
    console.log(status);


    // get todays measurements for this instrument
    const queryParams = new URLSearchParams({
      instruments: serial,
      start: new Date(new Date().setHours(0, 0, 0, 0)).toISOString(),
      end: new Date(new Date().setHours(24, 0, 0, 0)).toISOString(),
      values: 'LAeq,LCPeak,LAFmax', // only return these values from each measurement
    });
    const response3 = await fetch(url + '/data/noise/measurements?' + queryParams, { headers: { 'X-Api-Key': key } });
    const measurements = await response3.json();

    // print the measurements
    console.log('Measurements for ' + serial + ':');
    console.log(measurements);

    // finished
    console.log('Finished');

    // see the api documentation for details on other endpoints and their usage

  } catch (error) {
    console.log('Error:');
    console.error(error);
  }
}
// Example using the MyCirrus API
// This uses the axios library - https://axios-http.com/docs/intro
const axios = require('axios');

// define url and api key. Replace <YOUR_API_KEY> below
const url = 'https://api.mycirrus.cloud/v1'; // MyCirrus API URL
const key = '<YOUR_API_KEY>' // Your API key

// run the example
runExample();

async function runExample() {
  try {
    // first get the list of instruments
    const response = await axios.get(url + '/control/instruments', { headers: { 'X-Api-Key': key } });

    // print the list of instruments
    console.log('Instruments:');
    console.log(response.data);


    // get the serial number of the first instrument
    const serial = response.data[0].serialNumber;

    // now get the instrument status
    const response2 = await axios.get(url + '/control/instruments/' + serial + '/status', { headers: { 'X-Api-Key': key } });

    // print the status
    console.log('Status of ' + serial + ':');
    console.log(response2.data);


    // get todays measurements for this instrument
    const response3 = await axios.get(url + '/data/noise/measurements', {
      params: {
        instruments: serial,
        start: new Date(new Date().setHours(0, 0, 0, 0)).toISOString(),
        end: new Date(new Date().setHours(24, 0, 0, 0)).toISOString(),
        values: 'LAeq,LCPeak,LAFmax', // only return these values from each measurement
      },
      headers: { 'X-Api-Key': key }
    });

    // print the measurements
    console.log('Measurements for ' + serial + ':');
    console.log(response3.data);

    // finished
    console.log('Finished');

    // see the api documentation for details on other endpoints and their usage

  } catch (error) {
    console.log('Error:');
    console.error(error);
  }
}
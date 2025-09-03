const axios = require("axios");
const {Javonet, WsConnectionData} = require('javonet-nodejs-sdk')

const iterations = 100;

async function run() {
  const url = "http://localhost:8086/getstring"; // your api URL
  const times = [];

  for (let i = 0; i < iterations; i++) {
    const start = Date.now();
    try {
      await axios.get(url);
    } catch (err) {
      console.error(`Request ${i + 1} failed:`, err.message);
      continue;
    }
    const elapsed = Date.now() - start;
    times.push(elapsed);
  }

  if (times.length > 0) {
    const avg = times.reduce((a, b) => a + b, 0) / times.length;
    console.log(`\nHTTP Average: ${avg.toFixed(3)} ms`);
  } else {
    console.log("No successful requests to calculate average.");
  }
}

async function pythonCall() {
  Javonet.activate("your-API-key")

  const ws = "ws://127.0.0.1:8090/ws";
  const times = [];

  try {
    const pythonRuntime = Javonet.webSocket(new WsConnectionData(ws)).python();
    const pyType = await pythonRuntime.getType("MyClass.MyClass");
    const pyInstance = await pyType.createInstance();

    for (let i = 0; i < iterations; i++) {
      const start = Date.now();
      try {
        const result = await pyInstance
          .invokeInstanceMethod("getstring")
          .execute();
        const elapsed = Date.now() - start;
        const value = await result.getValue();

        times.push(elapsed);
      } catch (err) {
        console.error(`Request ${i + 1} failed:`, err.message);
      }
    }

    if (times.length > 0) {
      const avg = times.reduce((a, b) => a + b, 0) / times.length;
      console.log(`\nJavonet Average: ${avg.toFixed(3)} ms`);
    } else {
      console.log("No successful requests to calculate average.");
    }
  } catch (err) {
    console.error("Initialization failed:", err);
  }
}

run();
pythonCall();

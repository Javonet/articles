# Javonet vs REST: The Fastest Path from JavaScript to Python

## Introduction
In modern software development, seamless interoperability between different programming languages is often a key requirement. Teams frequently face the challenge of connecting applications written in JavaScript with backend logic implemented in Python. The most common approach is to expose Python functionality over REST APIs, which provides flexibility but also introduces serialization overhead, network latency, and infrastructure complexity.

In this article, we present a benchmark comparison of Javonet vs REST for JavaScript-to-Python calls. Our goal is to evaluate performance differences, identify potential bottlenecks, and provide practical insights into when each approach may be the best fit.

[toc]

## Test Environment
OS: **Windows 11**
CPU/RAM: **Intel Core i7-1355U**/**32GB**
Docker: **28.3.3**
Node.js: **22.19**
Python: **3.12**

## Flask code

A simple Flask application that exposes endpoint `getstring`:
```python=
from flask import Flask, Response

app = Flask(__name__)

@app.get("/getstring")
def getstring():
    return Response("Tested!", mimetype="text/plain")

if __name__ == "__main__":
    # 0.0.0.0 potrzebne w kontenerze
    app.run(host="0.0.0.0", port=5000)
```

We create a Dockerfile based on a slim Python image, copy the application and requirements, install dependencies, and run the app:

```dockerfile=
FROM python:3.12-slim

WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY app.py .
EXPOSE 5000

CMD ["python", "app.py"]
```

Let's build docker image and start container:
```bash!
docker build -t flask .
docker run -p 8080:80 -p 8081:81 flask
```

We should create `requirements.txt`:
```
Flask
```

## Fast API

Same thing we can do with FlaskApi:
```python=
from fastapi import FastAPI
from fastapi.responses import PlainTextResponse

app = FastAPI()

@app.get("/getstring", response_class=PlainTextResponse)
def getstring():
    return "Tested!"
```
Dockerfile:
```dockerfile=
FROM python:3.12-slim

WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt

COPY main.py .
EXPOSE 8000

CMD ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]
```

And `requirements.txt`:
```
fastapi
uvicorn
```

## Python code called by Javonet
Now we will create a simple Python application that serves the same function as the previous API, but this time it will be invoked through Javonet code. Let's create `MyClass.py`:
```python=
class MyClass:
    __type__ = "MyClass"

    def getstring(self) -> str:
        return "Tested!"
```
We also need to download the `gg` file and place it in the same directory as MyClass.py and the `Dockerfile`.

Our `Dockerfile` should look like this:
```dockerfile=
FROM ubuntu:24.04

USER root

RUN apt-get update && apt-get install -y --no-install-recommends \
    python3 python3-venv python3-pip \
    libxml2-dev openssl libxmlsec1-dev libxmlsec1-openssl libjsoncpp25 libpython3-dev \
 && apt-get clean && rm -rf /var/lib/apt/lists/*

WORKDIR /usr/local/app

ENV LD_LIBRARY_PATH=/usr/local/app 

COPY gg ./gg
COPY MyClass.py ./

EXPOSE 8090

CMD ["./gg", "--licenseKey", "your-API-key", "--runtime", "python", "--modules", "./MyClass.py"]
```
And we can run it:
```bash!
docker build -t pythonclass .
# Use different ports than before.
docker run -p 8082:80 -p 8083:81 pythonclass
```

## Testing code
Now we can move on to creating the JavaScript code that will test the Docker containers we have prepared.
```javascript=
const axios = require("axios");
const {Javonet, WsConnectionData} = require('javonet-nodejs-sdk')

const iterations = 100; // set the number of iterations

async function run() {
  const url = "http://localhost:8080/getstring"; // your api URL
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
  Javonet.activate("your-API-key");

  const ws = "ws://127.0.0.1:8082/ws";
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

```
For sample tests on our hardware, we obtained the following results (ms):
```
100 calls:
Javonet Average: 2.420
FastApi Average: 2.690
Flask Average: 8.090

1000 calls:
Javonet Average: 1.968
FastApi Average: 2.113
Flask Average: 8.691

10000 calls:
Javonet Average: 2.157
FastApi Average: 2.329
Flask Average: 8.468
```

## Conclusion
The benchmarks show that Javonet consistently outperforms REST-based approaches when calling Python from JavaScript. Across 100, 1,000, and 10,000 calls, Javonet maintained the lowest average latency (around 2 ms), slightly ahead of FastAPI and far faster than Flask, which stayed above 8 ms. While REST remains valuable for distributed systems, Javonet offers a clear advantage when low latency and maximum performance are required.
from fastapi import FastAPI
from fastapi.responses import PlainTextResponse

app = FastAPI()

@app.get("/getstring", response_class=PlainTextResponse)
def getstring():
    return "Tested!"

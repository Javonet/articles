from flask import Flask, Response

app = Flask(__name__)

@app.get("/getstring")
def getstring():
    return Response("Tested!", mimetype="text/plain")

if __name__ == "__main__":
    # 0.0.0.0 potrzebne w kontenerze
    app.run(host="0.0.0.0", port=5000)

FROM ubuntu:24.04

USER root

# instalacja Pythona 3.12 (jest w repo 24.04) + dependencies dla gg
RUN apt-get update && apt-get install -y --no-install-recommends \
    python3 python3-venv python3-pip \
    libxml2-dev openssl libxmlsec1-dev libxmlsec1-openssl libjsoncpp25 libpython3-dev \
 && apt-get clean && rm -rf /var/lib/apt/lists/*

WORKDIR /usr/local/app

ENV LD_LIBRARY_PATH=/usr/local/app 

# kopiujemy binarkę gg
COPY gg ./gg
COPY MyClass.py ./

EXPOSE 8090

CMD ["./gg", "--licenseKey", "your-API-key", "--runtime", "python", "--modules", "./MyClass.py"]
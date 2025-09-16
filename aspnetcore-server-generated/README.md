# IO.Swagger - ASP.NET Core 2.0 Server

REST API that performs a math operation on two numbers via POST. The operation is provided in the `X-Operation` header (add, subtract, multiply, divide). Secured with Bearer JWT. Includes an Auth endpoint to mint a short-lived token for testing. 

## Run

Linux/OS X:

```
sh build.sh
```

Windows:

```
build.bat
```

## Run in Docker

```
cd src/IO.Swagger
docker build -t io.swagger .
docker run -p 5000:5000 io.swagger
```

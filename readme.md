# Selenium Load PoC

This is an example of how to build a simple Selenium test and execute it from inside a docker instance.


# Build and run docker
It will run against a simple website, bundled within this package. So before running the test, start the web server:
```shell
dotnet run -p src/SeleniumLoadPoc.TestApp
```

Then build the service:
```shell
docker build .
```

Then running the docker instance will run the test:
```shell
docker run -e TARGET_HOSTNAME=172.16.211.170 40df9915370b
```

# Run locally
To troubleshoot you can also build and run locally:

```
dotnet run -p src/SeleniumLoadPoc.TestApp
dotnet run -p src/SeleniumLoadPoc
```
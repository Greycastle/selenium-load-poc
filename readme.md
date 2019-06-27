# Selenium Load PoC

And this is how you run it 🎶
```bash
docker build .
docker run -e TARGET_HOSTNAME=172.16.211.170 40df9915370b
```

And this is how you load it
```bash
dotnet run -p src/SeleniumLoadPoc.Generator --clients 5 --docker-name 40df9915370b
```
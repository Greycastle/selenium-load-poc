# Debian dotnet core sdk
FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch

RUN apt-get update && apt-get install -y \
	apt-transport-https \
	ca-certificates \
	curl \
	gnupg \
	--no-install-recommends \
	&& curl -sSL https://dl.google.com/linux/linux_signing_key.pub | apt-key add - \
	&& echo "deb https://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google-chrome.list \
	&& apt-get update && apt-get install -y \
	google-chrome-stable \
	fontconfig \
	fonts-ipafont-gothic \
	fonts-wqy-zenhei \
	fonts-thai-tlwg \
	fonts-kacst \
	fonts-symbola \
	fonts-noto \
	ttf-freefont \
	--no-install-recommends \
	&& apt-get purge --auto-remove -y curl gnupg \
	&& rm -rf /var/lib/apt/lists/*

# Install chromedriver
RUN apt-get update && apt-get install -qq -y unzip xvfb libxi6 libgconf-2-4
RUN apt-get install -qq default-jdk 
RUN wget https://chromedriver.storage.googleapis.com/75.0.3770.90/chromedriver_linux64.zip \
    && unzip chromedriver_linux64.zip \
    && mv chromedriver /usr/bin/chromedriver

RUN mkdir /var/app

COPY ./src /var/app

WORKDIR /var/app/SeleniumLoadPoc

RUN dotnet build --configuration Release --runtime debian.9-x64

CMD bin/Release/netcoreapp2.2/debian.9-x64/SeleniumLoadPoc
# goto - World's best URL-shortener & bookmarking site

The repository contains the ASP.NET MVC code base for building and deploying the goto solution 
in your local intranet.

## Table of contents

- [Requirements](#requirements)
    - [Building](#building)
    - [Deploying](#deploying)
- [Installation](#installation)
- [Usage](#usage)
- [Links](#links)

## Requirements

###Building
1. Windows 7 or higher operating system
2. Visual Studio (any version will do including the free VS Community edition)

###Deploying
1. A server with Windows operating system
2. Access to IIS

## Installation
1. Clone or fork the entire code base onto your development machine (PC)
2. Build and run from within Visual Studio to verify the home page loads
3. To test or use the website without launching from Visual Studio directly:
    1. From within Visual Studio, right click on the UrlShortenerWeb project in solution explorer and select Properties
    2. In the left-hand nav options, click on Web
    3. In the Servers section, from the drop-down, select "Local IIS" and in the textbox below for Choose Url, pick any Url you like.  We recommend "http://localhost/goto".
    4. Save these changes, and rebuild
    5. After these steps are completed, you can use the goto app by directly navigating to http://localhost/goto without having launch it from Visual Studio every time.

## Usage

## Links

* [Web site](https://balki.io/goto)
* [Source code](https://github.com/balki-server/goto)

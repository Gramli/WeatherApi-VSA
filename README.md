 <img align="left" width="116" height="116" src=".\doc\img\weatherApi_icon.png" />

# Vertical Slice Architecture WeatherApi
[![.NET Build and Test](https://github.com/Gramli/WeatherApi-VSA/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Gramli/WeatherApi-VSA/actions/workflows/dotnet.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/748a25879e324dfca7232aae16c33eaa)](https://app.codacy.com/gh/Gramli/WeatherApi-VSA/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)  

REST API solution demonstrates how to create an API using Vertical Slice Architecture, minimal API and various design patterns.  

The example API allows users to retrieve actual/forecast weather data by location from [Weatherbit](https://www.weatherbit.io/) throught [RapidAPI](https://rapidapi.com). Additionally, it allows users to add/delete favorite locations into an [in memory database](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli) and then retrieve weather data by stored (favorite) locations.

## Menu
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Get Started](#get-started)
    - [Try it using .http file (VS2022)](#try-it-using-http-file-vs2022)
  - [Motivation](#motivation)
  - [Architecture](#architecture)
    - [Comparsion to Clean Architecture](#comparsion-to-clean-architecture)
  - [Technologies](#technologies)

## Prerequisites
* **.NET SDK 9.0.x**

## Installation

To install the project using Git Bash:

1. Clone the repository:
   ```bash
   git clone https://github.com/Gramli/WeatherApi-VSA.git
   ```
2. Navigate to the project directory:
   ```bash
   cd WeatherApi-VSA/src
   ```
3. Install the backend dependencies:
   ```bash
   dotnet restore
   ```

## Get Started
1. Register on [RapidAPI](https://rapidapi.com)
2. Subscribe [Weatherbit](https://rapidapi.com/weatherbit/api/weather) (its for free) and go to Endpoints tab
3. In API documentation copy (from Code Snippet) **X-RapidAPI-Key**, **X-RapidAPI-Host** and put them to appsettings.json file in WeatherAPI project
```json
  "Weatherbit": {
    "BaseUrl": "https://weatherbit-v1-mashape.p.rapidapi.com",
    "XRapidAPIKey": "value from code snippet",
    "XRapidAPIHost": "value from code snippet"
  }
```
4. Run Weather.API 

### Try it using .http file (VS2022)
 * Go to Tests/Debug folder and open **debug-tests.http** file (in VS2022)
 * Send request

## Motivation
The main motivation is to write a practical example of a simple API using Vertical Slice Architecture and then compare it to the same API designed by Clean Architecture.

## Architecture
Project folows **[Vertical Slice Architecture](https://www.jimmybogard.com/vertical-slice-architecture/)** which instead of separation of technical concerns it encapsulating business logic of specific feature into vertical slice. So for example against [Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture), one vertical slice is cross section of all layers like in picture below.

![Project Vertical Slice Architecture Diagram](./doc/img/chart.png)

As this project involves a simple API, I decided to represent each vertical slice (or feature) as a CRUD operation with some additional business logic. Vertical slices shouldn't reference each other, so shared code is placed in the Domain folder. For example, the WeatherService, which acts as an adapter to the Weatherbit.Client project, or the DbContext, resides in the Domain. 

Every feature (slice) folder contains a similar structure:
* request query or command - holds request parameters/data
* request handler - implementation of business logic
* specification holder - validation rules
* configuration - registering of classes into IoC container and minimal api endpoints.

Some slices also contains Dto objects, mapper profiles etc.., just all specific to the feature(slice).

### Comparsion to Clean Architecture
I wrote a short blog post comparing Clean Architecture and Vertical Slice Architecture. The comparison is based on real pull requests, and the post also explores a hypothesis about a hybrid approach. [Link to post](https://gramli.github.io//posts/architecture/clean-arch-vs-vertical-slice-arch)

## Technologies
* [ASP.NET Core 9](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-9.0)
* [Entity Framework Core InMemory](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli)
* [SmallApiToolkit](https://github.com/Gramli/SmallApiToolkit)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [FluentResuls](https://github.com/altmann/FluentResults)
* [Validot](https://github.com/bartoszlenar/Validot)
* [GuardClauses](https://github.com/ardalis/GuardClauses)
* [Moq](https://github.com/moq/moq4)
* [Xunit](https://github.com/xunit/xunit)
* [Scalar](https://scalar.com)

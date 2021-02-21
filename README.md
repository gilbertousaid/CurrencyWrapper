# Currency Wrapper for Banguat Exchange
## _The Wrapper for USD and Euro caller_

This solution has been createdfor invoking Banguat (Central bank of Guatemala) webservice and scrape results having the following endpoints:

- api/currencies/daterange
- api/currencies/date

## Endpoint description

- range:
  You will be able to retrieve the exchange rate of a currency (USD, EU)
  from the public site of Banguat sending parameter of date and desired
  currency in the URI (See examples bellow)

- daterange:
  You will be able to retrieve the exchange rate  of a currency (USD, EU)
  from the public site of Banguat but this endpoint will provide three
  results: Mean Exchange, Max Exchange and Min Exchange, for the given
  date range, the parameters will be provided via URI (See Examples bellow)

#### Endpoint Samples:

| Endpoint | sample call | result |
| ------ | ------ | ------ |
| date | https://localhost:44337/api/currencies/date/20210219/USD | The Rate Exchange for the currency USD at 20210219 is  7.73197 | 
| date | https://localhost:44337/api/currencies/date/20210219/EU | The Rate Exchange for the currency EU at 20210219 is   1.21250 , 1.21260 | 
| daterange | https://localhost:44337/api/currencies/daterange/20210201/20210220/USD | For the currency USD in date ranges 20210201 to 20210220 has the mean value of 7.757056 and the max of 7.78003 and the min of 7.73197|
| daterange | https://localhost:44337/api/currencies/daterange/20210201/20210220/EU | For the currency EU in date ranges 20210201 to 20210220 has the mean value of 1.207513 and the max of 1.2138 and the min of 1.1986|
###  C# Was used for building this app
[![N|Solid](https://user-images.githubusercontent.com/29004603/75462714-9dd79b80-59bf-11ea-8e6b-575765733340.png)](https://docs.microsoft.com/en-us/dotnet/csharp/)

### For Running the app, you will need:

> .Net Framework 4.5 or higher
> Nugget Package access
> C# Builder
> Raider or Visual Studio (Preffered)

## Installation
Install the dependencies contained in the packages.config.

```sh
dotnet run [-c|--configuration <CONFIGURATION>] [-f|--framework <FRAMEWORK>]
    [--force] [--interactive] [--launch-profile <NAME>] [--no-build]
    [--no-dependencies] [--no-launch-profile] [--no-restore]
    [-p|--project <PATH>] [-r|--runtime <RUNTIME_IDENTIFIER>]
    [-v|--verbosity <LEVEL>] [[--] [application arguments]]

dotnet run -h|--help
```
## Packages used

Must of the used packages are included in .Net Framework but there are
some others not included by default:

| Plugin | Documentation |
| ------ | ------ |
| HtmlAgilityPack | (https://www.c-sharpcorner.com/UploadFile/9b86d4/getting-started-with-html-agility-pack/) |
| Newtonsoft.Json | (https://www.newtonsoft.com/json) |
| WebGrease| (https://www.nuget.org/packages/WebGrease/) |
| Modernizr | (https://www.c-sharpcorner.com/UploadFile/0e28e5/how-to-use-modernizr/) |
| Antlr | (https://tomassetti.me/getting-started-with-antlr-in-csharp/) |

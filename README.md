# EMG Templates

This repository contains a set of templates to be used when creating .NET projects from the `dotnet new` command line interface.

You can use [this guide](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new) to understand the options available to you when generating a project using the templates

## Available templates

The current version of the package contains the following templates.

|Name|Short name||
|-|-|-|
|ASP.NET Core API                          |emg-web-api               |
|Batch JobProcessor                        |emg-batch-jobprocessor    |
|Lambda Local Runner                       |emg-lambda-local-runner   |
|Lambda Event Function                     |emg-lambda-event          |
|Lambda RequestResponse Function           |emg-lambda-requestresponse|
|Test Project                              |emg-test-lib              |
|Nybus Windows Service _obsolete_          |emg-nybus-service         |_obsolete_
|WCF Windows Service _obsolete_            |emg-wcf-service           |_obsolete_
|WCF Windows Service with Nybus            |emg-wcf-service-nybus     |_obsolete_
|Windows Service                           |emg-windows-service       |

### ASP.NET Core API

```
dotnet new emg-web-api
```

This template creates a REST API hosted by a ASP.NET Core application.

The following packages are added by default:
* `EMG.Extensions.AspNetCore.Authentication.JWT`
* `EMG.Extensions.AspNetCore.Filters.Error`
* `EMG.Extensions.Logging.Loggly`

You can customize the application by providing the following parameters
* `--configure-aws` adds the basic setup for AWS services
* `--nybus` adds support for sending messages with Nybus v1
* `--nybus-bridge` adds support for sending messages via the NybusBridge to Nybus v0 applications
* `--wcf-discovery` adds support for discovering WCF services

### Batch JobProcessor

```
dotnet new emg-batch-jobprocessor
```

This template creates a console application that can be used to process AWS Batch jobs.

### Lambda Local Runner

```
dotnet new emg-lambda-local-runner
```

This template creates a console application using the `LambdaRunner` to host locally a AWS Lambda function.

### Lambda Event Function

```
dotnet new emg-lambda-event
```

This template creates a AWS Lambda function that reacts to event and does not return a response.

### Lambda RequestResponse Function

```
dotnet new emg-lambda-requestresponse
```

This template creates a AWS Lambda function that reacts to event and then returns a response.

### Test Project

```
dotnet new emg-test-lib
```

This template creates a unit-test project using NUnit 3.x.

It also has references to
* `AutoFixture`
* `Moq`
* `AutoFixture.NUnit3`
* `AutoFixture.AutoMoq`
* `AutoFixture.Idioms`

The project will contain a specialization of `AutoDataAttribute` that configures the Moq glue library for AutoFixture.

### Nybus Windows Service

```
dotnet new emg-nybus-service
```

This template creates a Windows Service application hosting Nybus v0 handlers.

The project uses:
* TopShelf
* Castle Windsor
* Nybus v0

This template is obsolete. Please use the `emg-windows-service` with the `--add-nybus Legacy` parameter.

### WCF Windows Service

```
dotnet new emg-wcf-service
```

This template creates a Windows Service application hosting WCF services.

The project uses:
* TopShelf
* Castle Windsor
* WCF

This template is obsolete. Please use the `emg-windows-service` with the `--add-wcf` flag.

### WCF Windows Service with Nybus

```
dotnet new emg-wcf-service
```

This template creates a Windows Service application hosting WCF services and Nybus v0 handlers.

The project uses:
* TopShelf
* Castle Windsor
* Nybus v0
* WCF

This template is obsolete. Please use the `emg-windows-service` with the `--add-nybus Legacy` parameter and the `--add-wcf` flag.

### Windows Service

```
dotnet new emg-wcf-service
```

This template creates a Windows Service application.

The application can be customized with the following parameters:
* `--add-nybus Current` adds support for hosting Nybus v1.x handlers
* `--add-nybus Legacy` adds support for hosting Nybus v0.x handlers
* `--add-wcf` adds support for hosting WCF services
* `--add-aws` adds basic setup to AWS services

## Installation

You can install the templates in this package by running the following command:
```
dotnet new -i EMG.Templates
```

Since the package is hosted in the EMG private NuGet feed, make sure it is listed as a authenticated source.

Alternatively, you can check out the repository and install the package by running the following command in the folder where you checked out the repository:
```
dotnet new -i .\content\
```

## Update to the latest version

To update the templates to the latest version, simply reinstall the package as specified in the section above.


## Uninstall

To uninstall the templates, you can use the command suggested by the `dotnet new -u` command.

Example:
```
  C:\Development\EMG\Templates\content
    Templates:
      [EMG] Batch JobProcessor (emg-batch-jobprocessor) C#
      [EMG] Lambda Event Function (emg-lambda-event) C#
      [EMG] Lambda Local Runner (emg-lambda-local-runner) C#
      [EMG] Nybus Windows Service (emg-nybus-service) C#
      [EMG] Lambda RequestResponse Function (emg-lambda-requestresponse) C#
      [EMG] Test Project (emg-test-lib) C#
      [EMG] WCF Windows Service (emg-wcf-service) C#
      [EMG] WCF Windows Service with Nybus (emg-wcf-service-nybus) C#
      [EMG] ASP.NET Core API (emg-web-api) C#
      [EMG] Windows Service (emg-windows-service) C#
    Uninstall Command:
      dotnet new -u C:\Development\EMG\Templates\content
```
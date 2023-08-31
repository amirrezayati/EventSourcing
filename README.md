# EventSourcing
Application of Event Sourcing using asp.net core, DDD, EF, EventStoreDB and SQL Server
Event sourcing is a most important pattern to design a microservice based application. If you are working with multiple services in a microservice based application, you have to use event driven architecture. In this article I will discuss and apply event sourcing using asp.net core, DDD and EventStoreDB.

Domain Driven Design (DDD)_
Domain-driven design (DDD) is a major software design approach, focusing on modeling software to match a domain according to input from that domain’s experts. Under domain-driven design, the structure and language of software code (class names, class methods, class variables) should match the business domain. (Wikipedia)

Event Sourcing
Event sourcing is a technique to store all events of an Object to get all of its versions. Event sourcing pattern is used to implement microservice based application. Using this pattern, we can track the changes of an object in its lifecycle.

EventStoreDB EventStoreDB specially built for Event Sourcing. It is a NoSQL database. This is a one-way database – we can only insert data into database.

Implementation
Let’s implement Event Sourcing using DDD and EventStoreDB.

Tools and Technologies Used

Visual Studio 2022
.NET 6.0
ASP.NET Core Web API
Visual C#
DDD
EventStoreDB
SQL Server
Step 1: Install EventStoreDB

You can install EventStoreDB using EventStoreDB documentation. Visit the following link : https://developers.eventstore.com/server/v20.10/installation.html#quick-start

Or, you can run docker image of EventStoreDB as below.

docker run --name esdb-node -it -p 2113:2113 -p 1113:1113 eventstore/eventstore:latest --insecure --run-projections=All --enable-external-tcp --enable-atom-pub-over-http
Here I used docker images for EventStoreDB. After running the above command, browse EventStoreDB using the following link.
http://localhost:2113/web/index.html#/dashboard

Step 2: Create solution and projects

Create a solution name EventSourcing.
Add a new web api projects name - Catalog.API
Add three class library projects name – Application, Domain, Infrastructure
Step 3: Install nuget packages

Install following nuget packages in Catalog.API
PM> Install-Package Microsoft.EntityFrameworkCore.Tools
Install following nuget packages in Application
PM> Install-Package MediatR
PM> Install-Package MediatR.Extensions.Microsoft.DependencyInjection
PM> Install-Package Microsoft.Extensions.DependencyInjection.Abstractions
PM> Install-Package Newtonsoft.Json
Install following nuget packages in Infrastructure
PM> Install-Package EventStore.Client.Grpc.Streams
PM> Install-Package Microsoft.EntityFrameworkCore
PM> Install-Package Microsoft.Extensions.Configuration.Abstractions
PM> Install-Package Microsoft.Extensions.DependencyInjection.Abstractions
PM> Install-Package Newtonsoft.Json

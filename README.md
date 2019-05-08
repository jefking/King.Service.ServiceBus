# Task scheduling for .NET: Service Bus.
+ Initialize: Queues, Topics
+ Enqueue and dequeue
+ Enqueue for dequeue at specific time
+ Dequeue batches
+ Recieve events
+ Time sensitive events
+ Plugs into [King.Service](https://github.com/jefking/King.Service)

# Ready, Set, Go!
## [NuGet](https://www.nuget.org/packages/King.Service.ServiceBus)
```
PM> Install-Package King.Service.ServiceBus
```

## [Demo Container](https://hub.docker.com/r/jefking/king.service.servicebus.demo)
Create Azure Service Bus

### Pull
```
docker pull jefking/king.service.servicebus.demo
```

### Run
```
docker run -it jefking/king.service.servicebus.demo <YOUR CONNECTION STRING>
```

## CI
[![Build status](https://dev.azure.com/jefkin/oss/_apis/build/status/King.Service.ServiceBus)](https://dev.azure.com/jefkin/oss/_build/latest?definitionId=13)

## [Wiki](https://github.com/jefking/King.Service.ServiceBus/wiki)
View the wiki to learn how to use this.
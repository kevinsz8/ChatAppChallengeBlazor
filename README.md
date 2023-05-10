# Code Challege Chat App

This is an example on how we can implement SignalR with RabbitMQ to send messages between different users and having a bot that is going to be executed once one of the users send a command in the message textbox like "/stock=aappl.us".

## Installation

First run an docker container for the RabbitMQ

```c
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.11-management
```


## Run the application

Open the code and run it, you are going to see 2 projects:

- ChatCodeChallenge (is the ASP NET CORE WEB APP)
- RabbitMQConsumer (Console app)

### Make sure that the WebApp port matches with the one on the Program.cs file on the RabbitMQConsumer (by default we use localhost:7187)

You should be able to run both and then you are going to see a new WebBrowser with a screen with the User and message textbox. 


## License

[MIT](https://choosealicense.com/licenses/mit/)

### RabbitMq exception

###### When invalid host name  
- **BrockerUnreachableException** 
    - **ConnectionFailureException**
        - **SocketException** when SocketErrorCode == HostNotFound
###### When RabbitMq down 
- **BrockerUnreachableException** 
    - **ConnectionFailureException**
        - **SocketException** when SocketErrorCode == ConnectionRefused
###### When invalid user name or Password 
- **BrockerUnreachableException** 
    - **AuthenticationFailureException** Message == "ACCESS_REFUSED - Login was refused using authentication mechanism PLAIN. For details see the broker logfile."

###### When invalid connection string 
- fall with **ArgumentException** in factory

###### When DeclareExchangePassive with invalid exchange 
- **OperationInterruptedException** ShutdownReason.ReplayCode = 404
- *ModelShutdown* event before exception
- next operation in model - **AlreadyClosedException**
- channel dropped, connection work

###### When DeclareExchange with invalid type or parameters
- **OperationInterruptedException** ShutdownReason.ReplayCode = 406
- next operation in model - **AlreadyClosedException**
- channel dropped, connection work

###### When server shutdown
1. *ConnectionShutdown* event with Cause - EndOfStreamException
2. *ModelShutDown* event with same Cause
3. **AlreadyClosedException** when next operation
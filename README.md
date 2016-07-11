# OpcEsb
High performance ESB (enterprise service bus) with OPC UA: Masstransit, RabbitMQ and OPC UA. This is also a Demo OPC UA Pub/Sub and using MassTransit as relay gateway/bridge to receive message from OPC UA Client. 

1. Installing Masstransit & RabbitMQ
+ Install Erlang: http://www.erlang.org/download.html
+ Then download and install http://www.rabbitmq.com/download.html
+ Enabling the RabbitMQ Web Management Interface:
  In cmd, let's enter: rabbitmq-plugins enable rabbitmq_management
  then type: services.msc, finding RabbitMQ service to restart it.
+ Now go to: http://localhost:15672/ then you will see the RabbitMQ web interface.

2. OPC UA stuffs
+ OPC UA Server: https://github.com/OPCFoundation/UA-.NET/tree/master/SampleApplications/Samples/Server
+ OPC UA Publisher: https://github.com/OPCFoundation/UA-.NET/tree/master/SampleApplications/Samples/Publisher
In the publisher, it will publish the message to Azure Iot. In here, I made just a minor change in order to publish message to RabbitMQ thru MassTransit.
HOW TO RUN OPC UA SERVER/PUBLISHER: (refer from: https://github.com/OPCFoundation/UA-.UWP-Universal-Windows-Platform/blob/master/README.md)
- Get from OPC Misc Tool the Certificate generator tool. (build the solution and get the Opc.Ua.CerticateGenerator.exe)

- Open a command prompt

- Create the folder "%TEMP%\OPC Foundation\CertificateStores\MachineDefault" and use the hostname command to find out the {hostname} to be used below.

- Issue the following two commands:

Opc.Ua.CertificateGenerator.exe -cmd issue -sp "%TEMP%\OPC Foundation\CertificateStores\MachineDefault" -an "UA Sample Client" -dn {hostname} -sn "CN=UA Sample Client/DC={hostname}" -au "urn:localhost:OPCFoundation:SampleClient"

Opc.Ua.CertificateGenerator.exe -cmd issue -sp "%TEMP%\OPC Foundation\CertificateStores\MachineDefault" -an "UA Sample Server" -dn {hostname} -sn "CN=UA Sample Server/DC={hostname}" -au "urn:localhost:OPCFoundation:SampleServer"
Copy the "%TEMP%\OPC Foundation" folder into the "LocalState" folder of the path shown in the error message (ex: \Users\UserName\AppData\Local\Packages\xxxxx-xxxx-xxx-xxx\LocalState).

- Now acknowledge the certificate message in the Opc.Ua.Publisher app and close the application.

- Restart the Opc.Ua.Publisher application. Now you should not get a certificate missing message. If you get a message for a missconfigured domain, acknowledge with "Yes" to use the certificate.

- If you don't have any OPC UA server device to connect to you may clone the OPC Foundations .NET repository, open the solution "UA Sample Applications.sln" with VisualStudio 2015, build the solution and run the "UA Sample Server" (Note: you need to start this application with Administrator rights).

- Now you enter a connection URL into the connection URL list box of "Opc.Ua.Publisher" (Note: do a right click into the list box to put the focus on it).

- In our case we are entering the connection URL, which is shown in the "UA Sample Server" application as "Server Endpoint URLs" connection URL list box into the "Opc.Ua.Publisher".

- Press "Connect" button to connect to the "UA Sample Server".

- You should see a dialog, which allows you to select "Security Mode", "Security Policy" and other settings. Choose your settings and acknowledge with "Ok".

- Then you should see a dialog, to allow entering your username and password. For an anonymous session, you are good to enter nothing and just acknowledge the dialog with "Ok".

- On the left window in the application you see all existing sessions (you are able to connect to multiple OPC UA servers here) and on the right side you could browse the addresss space of the server.

- Choose a node in the right window by browsing to it and selecting it with the mouse (Note: choose a value, which changes frequently like Objects->Server->ServerStatus->CurrentTime).

- Press the "Publish" button and the application will start publishing the nodes JSON encoded data to RabbitMQ.


3. Subscriber Data
You have run OPC Server and OPC Publisher. Now it the time to subscriber data from RabbitMQ. Let’s start OpcEsb.Subscriber, then enter a subscriber name, so you will see the data from Opc server.
Enjoy it!



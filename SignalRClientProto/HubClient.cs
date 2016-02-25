using Microsoft.AspNet.SignalR.Client;
using System;
using System.Configuration;

namespace SignalRClientProto
{
    class HubClient
    {
        static void Main(string[] args)
        {
            var ip = ConfigurationManager.AppSettings["HostIP"];
            var port = ConfigurationManager.AppSettings["HostPort"];
            var connection = new HubConnection($"http://{ip}:{port}/");

            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("ProtoHub");
            
            myHub.On("newUserConnected", param => {
                Console.WriteLine(param);
            });

            myHub.On("broadcast", param => {
                Console.WriteLine(param);
            });

            //Start connection
            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();

            var quit = false;
            while (!quit)
            {
                var command = Console.ReadLine();


                switch (command)
                {
                    case "ping":
                        myHub.Invoke("Ping").ContinueWith(task => {
                            if (task.IsFaulted)
                            {
                                Console.WriteLine("There was an error calling send: {0}",
                                                  task.Exception.GetBaseException());
                            }
                            else
                            {
                                Console.WriteLine("Ping Sent");
                            }
                        });
                        break;

                    default:
                        break;
                }
               

            }
            
            connection.Stop();
        }
    }
}

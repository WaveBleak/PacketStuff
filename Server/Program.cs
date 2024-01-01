using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;

class Program
{
    private static int _port;
    static async Task Main(string[] args)
    {
        await AskForPort(); //sæt _port
        Console.Clear();
        UdpClient listener = new UdpClient(_port);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, _port); //Accepter connections hvorsomhelst fra så længe det er det rigtige port

        try
        {
            Console.WriteLine("Connection established!");
            while (true)
            {
                byte[] bytes = listener.Receive(ref groupEP); //Læs bytes fra connections
                Console.WriteLine(Encoding.ASCII.GetString(bytes)); //Converter bytes til en besked
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        finally
        {
            listener.Close();
        }

    }


    private static Task<bool> AskForPort()
    {
        var tcs = new TaskCompletionSource<bool>();
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    Console.Clear();
                    Console.WriteLine("Skriv hvilket port du vil lytte på");
                    String input = Console.ReadLine();
                    if (input == null) throw new Exception();

                    _port = int.Parse(input);
                    tcs.SetResult(true);
                    break;
                }
                catch (Exception)
                {
                    Console.WriteLine("Noget gik galt! Prøv igen.");
                    Console.ReadKey();
                }
            }
        });
        return tcs.Task;
    }
}
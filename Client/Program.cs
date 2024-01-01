using System.Net;
using System.Net.Sockets;
using System.Runtime.Loader;
using System.Text;

class Program
{
    private static int _port;
    private static readonly IPAddress broadcastAddress = IPAddress.Parse("127.0.0.1"); //Localhost addressen
    static async Task Main(string[] args)
    {
        await AskForPort(); //_port bliver sat
        UdpClient client = new UdpClient(); //UDP Client laves
        IPEndPoint endPoint = new IPEndPoint(broadcastAddress, _port); //Forbindelse til localhost med portet _port;

        try
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Skriv en besked som skal sendes til serveren!");
                string message = Console.ReadLine();
                byte[] data = Encoding.ASCII.GetBytes(message); //Lav en besked om til bytes som kan sendes over netværket
                int attempts = 0;
                await client.SendAsync(data, data.Length, endPoint); //Send bytes over netværk
            }
        }
        catch (Exception) //Noget gik galt
        {
            Console.WriteLine("Kan ikke finde en server på port: " + _port);
            Console.ReadKey();
            return;
        }finally
        {
            client.Close();
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
                    Console.WriteLine("Skriv hvilket port du vil skrive til");
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
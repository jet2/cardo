using System;
using System.IO;
using Contracts;
using ServiceWire.NamedPipes;

namespace consoleipc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int znumber = -1;
            string ynumber = "0000000";
            while (true)
            {
                string[] cards = File.ReadAllLines("cards.txt");
                for (int i = 0; i < cards.Length; i++)
                {
                    Console.WriteLine($"{i + 1}) {cards[i]} ");
                }
                Console.WriteLine($"Введите порядковый номер карты или 0 для завершения работы эмулятора");
                string xnumber = Console.ReadLine();
                if (xnumber == "0") { break; };
                try
                {
                    znumber = int.Parse(xnumber);
                    ynumber = cards[znumber - 1];
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Автовыбор :) '0000000' ");
                    ynumber = "0000000";
                }
                try
                {
                    var npClient = new NpClient<ISenderText>(new NpEndPoint("TEST0"));
                    // See that we get the INNER exception with the Fixed code
                    try
                    {

                        Console.WriteLine($"Посылаю {ynumber} ");
                        npClient.Proxy.SendText($"{{\"isstate\":0,\"statevalue\":0, \"card\":\"{ynumber}\"}}");
                        Console.WriteLine($"Долетело {ynumber} ");
                    }
                    catch (Exception ex)
                    {
                        // Should be the exception we raised
                        Console.WriteLine($"НЕ долетело {ynumber}: {ex.Message}");
                    }
                    if (npClient != null) { npClient.Dispose(); }
                    
                }
                catch (Exception ex) {
                    Console.WriteLine($"НЕ долетело {ynumber}: {ex.Message}");
                }


            }

        }
    }
}
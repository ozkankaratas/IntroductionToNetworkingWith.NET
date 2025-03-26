﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading; 
using System.Threading.Tasks; 

class Program
{
    static async Task Main(string[] args)
    {
        IPAddress ipAddress = IPAddress.Any;
        int port = 8080;
        TcpListener listener = null;

        try
        {
            listener = new TcpListener(ipAddress, port);
            listener.Start();
            Console.WriteLine($"Sunucu başlatıldı. Dinleniyor: {ipAddress}:{port}");
            Console.WriteLine($"Ana İş Parçacığı ID: {Thread.CurrentThread.ManagedThreadId}"); // Ana thread'i görelim

            while (true) // Sürekli yeni bağlantıları bekle
            {
                Console.WriteLine("\nİstemci bağlantısı bekleniyor...");
                // 1. Bir istemci bağlantısını kabul et
                TcpClient client = await listener.AcceptTcpClientAsync();

                // 2. Bağlantı kabul edildiğinde, bu istemciyle ilgilenmesi için
                //    yeni bir görev başlat ve bir sonraki bağlantı için ana döngüye hemen geri dön

                Console.WriteLine($"İstemci bağlandı ({((IPEndPoint)client.Client.RemoteEndPoint).Address}:{((IPEndPoint)client.Client.RemoteEndPoint).Port}). Yeni görev başlatılıyor...");

                // HandleClientAsync metodunu ayrı bir Task olarak çalıştır.
                // _ değişkeniyle görevin sonucunu beklemediğimizi belirtiyoruz (fire and forget).
                _ = HandleClientAsync(client);

            } // Ana döngü hemen bir sonraki AcceptTcpClientAsync'e döner.
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket Hatası: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Genel Hata: {ex.Message}");
        }
        finally
        {
            listener?.Stop();
            Console.WriteLine("Sunucu durduruldu.");
        }
    }

    // Bu metot, TEK BİR istemci bağlantısıyla ilgilenir.
    // Her istemci için ayrı bir görev (Task) olarak çalıştırılır.
    static async Task HandleClientAsync(TcpClient client)
    {
        // Hangi thread'in bu istemciyle ilgilendiğini görelim
        Console.WriteLine($"---> İstemci ({((IPEndPoint)client.Client.RemoteEndPoint).Address}:{((IPEndPoint)client.Client.RemoteEndPoint).Port}) için İş Parçacığı ID: {Thread.CurrentThread.ManagedThreadId}");

        using (client)
        using (NetworkStream stream = client.GetStream())
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            try
            {
                // İstemciden veri oku (bağlantı kapanana veya hata oluşana kadar)
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"---> [{((IPEndPoint)client.Client.RemoteEndPoint).Port}] Alınan: {receivedMessage}");

                    // Aynı mesajı istemciye geri gönder (echo)
                    byte[] echoBytes = Encoding.UTF8.GetBytes("Echo: " + receivedMessage);
                    await stream.WriteAsync(echoBytes, 0, echoBytes.Length);
                    Console.WriteLine($"---> [{((IPEndPoint)client.Client.RemoteEndPoint).Port}] Gönderilen: Echo: {receivedMessage}");
                }
            }
            catch (System.IO.IOException ioEx) when (ioEx.InnerException is SocketException)
            {
                // Genellikle istemci aniden bağlantıyı kapattığında oluşur.
                Console.WriteLine($"---> İstemci ({((IPEndPoint)client.Client.RemoteEndPoint).Port}) bağlantısı beklenmedik şekilde kapandı: {ioEx.Message}");
            }
            catch (Exception ex)
            {
                // Bu istemciyle ilgili diğer hatalar
                Console.WriteLine($"---> İstemci ({((IPEndPoint)client.Client.RemoteEndPoint).Port}) ile iletişimde hata: {ex.Message}");
            }
            finally
            {
                Console.WriteLine($"---> İstemci ({((IPEndPoint)client.Client.RemoteEndPoint).Port}) bağlantısı kapatılıyor.");
            }
        } // TcpClient ve NetworkStream using bloklarının sonu
    }
}
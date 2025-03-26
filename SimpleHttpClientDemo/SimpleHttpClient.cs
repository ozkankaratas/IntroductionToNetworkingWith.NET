class SimpleHttpClient
{
    static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = "https://www.google.com"; // hedef url
            Console.WriteLine($"İstek gönderiliyor: {url}");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url); // asenkron olarak get isteği gönderip yanıt bekliyoruz

                 // yanıt kodu gönder (200,4xx, 5xx)
                Console.WriteLine(response.EnsureSuccessStatusCode());

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"\nBir Hata Oluştu {e.Message}");
                if (e.StatusCode.HasValue)
                {
                    Console.WriteLine($"Durum Kodu: {e.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nBeklenmedik bir hata oluştu: {ex.Message}");
            }
        }
    }
}
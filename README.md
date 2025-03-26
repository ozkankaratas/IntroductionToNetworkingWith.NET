# IntroductionToNetworkingWith.NET 

(tr-TR) Bu proje C# programlama dili kullanarak ağ programlama (networking) üzerine bir dizi eğitim materyali içermektedir. Bu eğitimler, ağ programlamanın kendisinden ziyade C#'ın ağ yeteneklerini ve System.Net isim alanı içindeki API'lerini nasıl kullanılacağını öğretmeyi amaçlamaktadır.

Eğitim içeriğinde genel olarak şunlar bulunur:

Giriş ve Kurulum: Eğitimin amacı, nasıl kullanılacağı ve gerekli ortam (özellikle Mono kullanımı) hakkında bilgi verilir.

Temel Kavramlar: HttpClient gibi sınıflarla web sayfalarını indirme gibi basit ağ işlemleri anlatılır.

TCP Programlama: TcpClient ve TcpListener sınıfları kullanılarak istemci-sunucu mimarisinde basit bir sohbet (chat) uygulaması geliştirme örneği sunulur. Daha sonra çoklu iş parçacığı (multithreading) ve asenkron kodlama kullanılarak metin tabanlı oyun sunucusu örneği verilir.

UDP Programlama: UdpClient sınıfı kullanılarak UDP protokolü üzerinden dosya transferi ve internet üzerinden oynanabilen basit bir Pong oyunu (istemci ve sunucu olarak) geliştirme gibi örnekler bulunur.

Socket Sınıfı: Daha düşük seviyeli ve daha fazla kontrol sağlayan Socket sınıfının kullanımı, Berkeley/POSIX Socket API'sine benzerliği vurgulanarak anlatılır.

Kod Örnekleri: Her bölümde anlatılan konularla ilgili tam kaynak kodları sağlanır ve bu kodların genellikle komut satırında çalıştırılabilecek şekilde basit tutulmasına özen gösterilir. Kodları indirip deneyebilirsiniz :)

Özetle, bu proje C# ile ağ programlamaya yeni başlayan veya bu konudaki bilgisini geliştirmek isteyenler için pratik örnekler ve açıklamalar sunan bir eğitim kaynağıdır. Özellikle TcpClient, TcpListener, UdpClient, HttpClient ve Socket gibi temel C# ağ sınıflarının kullanımını öğretir.

#(en-EN) This project includes a series of training materials on network programming using the C# programming language. These trainings are aimed at teaching C#'s networking capabilities and how to use its APIs within the System.Net namespace rather than network programming itself.

The training content generally includes the following:

Introduction and Installation: Information is provided about the purpose of the training, how to use it, and the required environment (especially using Mono).

Basic Concepts: Simple network operations such as downloading web pages are explained with classes such as HttpClient.

TCP Programming: A simple chat application development example is presented in a client-server architecture using the TcpClient and TcpListener classes. Then, a text-based game server example is given using multithreading and asynchronous coding.

UDP Programming: Examples such as file transfer over the UDP protocol and development of a simple Pong game (as a client and server) that can be played over the internet using the UdpClient class.

Socket Class: The use of the lower-level and more control-rich Socket class is explained, emphasizing its similarity to the Berkeley/POSIX Socket API.

Code Examples: Full source codes are provided for the topics covered in each section, and care is taken to keep these codes simple enough to be run on the command line. You can download and try the codes :)

In short, this project is an educational resource that provides practical examples and explanations for those who are new to network programming with C# or want to improve their knowledge on this subject. It teaches the use of basic C# network classes, especially TcpClient, TcpListener, UdpClient, HttpClient and Socket.

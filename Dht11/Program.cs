using System;
using System.Device.Gpio;
using System.Threading;

using Iot.Device.DHTxx;

Console.WriteLine("--- Test Czujnika DHT11 (Cyfrowy) ---");

// Definicja pinu DATA (GPIO 4)
int pin = 4;

// Inicjalizacja czujnika DHT11
// Uwaga: DHT11 jest dość wybredny czasowo, .NET na Linuxie radzi sobie z tym świetnie.
using var dht = new Dht11(pin);

Console.WriteLine("Pobieranie danych... (DHT11 potrzebuje ok. 2s między odczytami)");

try
{
    while (true)
    {
        // DHT11 nie lubi zbyt częstych odczytów - 2 sekundy to minimum
        Thread.Sleep(5000);

        // Metody TryRead... zwracają `true` przy sukcesie. Należy sprawdzić ich wynik.
        // Odczytujemy temperaturę i wilgotność. Ważne jest, aby sprawdzić, czy obie operacje się powiodły.
        if (dht.TryReadTemperature(out var temp) && dht.TryReadHumidity(out var hum))
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] " +
                              $"Temperatura: {temp.DegreesCelsius:F1}°C | " +
                              $"Wilgotność: {hum.Percent:F1}%");
        }
        else
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] Błąd odczytu z czujnika. Spróbuję ponownie.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
}
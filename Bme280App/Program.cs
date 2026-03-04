using System;
using System.Device.I2c;
using System.Threading;
using Iot.Device.Bmxx80;

Console.WriteLine("--- Test Czujnika BME280 przez I2C Hub ---");

// Domyślna magistrala I2C w Raspberry Pi (piny GPIO 2 i 3) to "1"
const int busId = 1;

// Adresy BME280 to zazwyczaj 0x76 lub 0x77. 
// Biblioteka oferuje wbudowane stałe: Bme280.DefaultI2cAddress (0x77) lub Bme280.SecondaryI2cAddress (0x76)
// Z modułami Waveshare często trafia się 0x77.
byte deviceAddress = Bme280.DefaultI2cAddress;

try
{
    var i2cSettings = new I2cConnectionSettings(busId, deviceAddress);
    using var i2cDevice = I2cDevice.Create(i2cSettings);
    using var bme280 = new Bme280(i2cDevice);

    Console.WriteLine("Czujnik BME280 pomyślnie zainicjowany. Rozpoczynam odczyty...");
    Console.WriteLine("Naciśnij Ctrl+C, aby wyjść.");
    Console.WriteLine(new string('-', 40));

    while (true)
    {
        // Odczyt wszystkich wartości z czujnika za jednym zamachem
        var readResult = bme280.Read();

        // Opcjonalne sprawdzenie czy udało się odczytać dane 
        // (W przeciwieństwie do DHT11, BME280 komunikuje się niezawodnie i szybko po I2C)
        if (readResult.Temperature.HasValue && readResult.Humidity.HasValue && readResult.Pressure.HasValue)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}]");
            Console.WriteLine($"  Temperatura: {readResult.Temperature.Value.DegreesCelsius:F1} \u00B0C");
            Console.WriteLine($"  Wilgotność : {readResult.Humidity.Value.Percent:F1} %");
            Console.WriteLine($"  Ciśnienie  : {readResult.Pressure.Value.Hectopascals:F1} hPa");
            Console.WriteLine(new string('-', 40));
        }
        else
        {
            Console.WriteLine("Błąd podczas odczytu danych środowiskowych.");
        }

        // BME280 nie ma rygorystycznych ograniczeń jak DHT11 (który musiał czekać np. 5s),
        // ale odczyt co 2 sekundy jest dobrą praktyką
        Thread.Sleep(2000);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nWystąpił błąd inicjalizacji lub odczytu: {ex.Message}");
    Console.WriteLine("1. Sprawdź, czy Interfejs I2C jest włączony w systemie raspi-config.");
    Console.WriteLine("2. Upewnij się, czy przewody są poprawnie wpięte do huba.");
    Console.WriteLine("3. Sprawdź czy układ nie jest przypadkiem podłączony pod adresem 0x76 (zmień zmienną deviceAddress).");
}
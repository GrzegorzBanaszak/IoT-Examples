using System;
using System.Device.I2c;
using System.Threading;
using Iot.Device.Ads1115;
using UnitsNet; // Wymagane do obsługi jednostek (np. woltów)

Console.WriteLine("--- Test Czujnika Wilgotności Gleby przez ADS1115 ---");

// Magistrala I2C "1" dla Raspberry Pi
const int busId = 1;

// Adres I2C dla ADS1115 to 0x48 (kiedy pin ADDR jest połączony z GND)
// Poprawka: podajemy adres bezpośrednio jako int
const int deviceAddress = 0x48;

try
{
    var i2cSettings = new I2cConnectionSettings(busId, deviceAddress);
    using var i2cDevice = I2cDevice.Create(i2cSettings);

    // Inicjalizacja ADS1115: 
    // InputMultiplexer.AIN0 - czytamy z pinu A0
    // MeasuringRange.FS4096 - zakres do +/- 4.096V (idealny przy zasilaniu 3.3V)
    using var adc = new Ads1115(i2cDevice, InputMultiplexer.AIN0, MeasuringRange.FS4096);

    Console.WriteLine("Przetwornik ADS1115 zainicjowany poprawnie.");
    Console.WriteLine("Rozpoczynam pomiary na kanale A0...");
    Console.WriteLine(new string('-', 40));

    // Zmienne do kalibracji
    double voltageDry = 2.407; // Napięcie w suchym powietrzu
    double voltageWet = 1.125; // Napięcie w wodzie

    while (true)
    {
        // Odczyt napięcia z kanału A0
        // Poprawka: używamy 'var' (w tle przypisze się typ ElectricPotential z UnitsNet)
        var readResult = adc.ReadVoltage();
        double currentVoltage = readResult.Volts;

        // Przeliczenie na procenty
        double moisturePercent = 100 - ((currentVoltage - voltageWet) / (voltageDry - voltageWet) * 100);
        moisturePercent = Math.Clamp(moisturePercent, 0, 100);

        Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}]");
        Console.WriteLine($"  Napięcie   : {currentVoltage:F3} V");
        Console.WriteLine($"  Wilgotność : {moisturePercent:F0} %");
        Console.WriteLine(new string('-', 40));

        Thread.Sleep(1000);
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\nWystąpił błąd: {ex.Message}");
    Console.WriteLine("Upewnij się, że I2C jest włączone i ADS1115 jest poprawnie podłączony.");
}
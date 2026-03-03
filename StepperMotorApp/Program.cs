using System;
using System.Device.Gpio;
using System.Threading;
using Iot.Device.Uln2003;

Console.WriteLine("--- Test 28BYJ-48: Pełne koło i Reset ---");
Console.WriteLine("Naciśnij Ctrl+C, aby zatrzymać i wrócić do pozycji 0.");

int pin1 = 17, pin2 = 18, pin3 = 27, pin4 = 22;

// 4096 kroków to pełny obrót (360 stopni) dla silnika 28BYJ-48 w trybie 8-krokowym
const int FullRotation = 4096;
int totalStepsMoved = 0;
bool isRunning = true;

using var motor = new Uln2003(pin1, pin2, pin3, pin4, stepsToRotate: FullRotation);

// Obsługa zdarzenia Ctrl+C
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true; // Zapobiegamy natychmiastowemu zamknięciu
    isRunning = false;
};

try
{
    while (isRunning)
    {
        Console.WriteLine("Zataczam pełne koło...");

        // Robimy obrót w małych partiach, aby móc szybciej zareagować na Ctrl+C
        for (int i = 0; i < FullRotation; i++)
        {
            if (!isRunning) break;
            motor.Step(1);
            totalStepsMoved++;
        }

        if (isRunning)
        {
            Console.WriteLine("Obrót zakończony. Czekam 2 sekundy...");
            Thread.Sleep(2000);
        }
    }
}
finally
{
    // RESET DO POZYCJI WYJŚCIOWEJ
    if (totalStepsMoved != 0)
    {
        Console.WriteLine($"\nResetowanie pozycji... Powrót o {-totalStepsMoved} kroków.");
        // Wykonujemy ruch powrotny
        motor.Step(-totalStepsMoved);
        totalStepsMoved = 0;
    }

    // Wyłączenie cewek (bezpieczeństwo)
    motor.Step(0);
    Console.WriteLine("Silnik w pozycji wyjściowej. Koniec.");
}
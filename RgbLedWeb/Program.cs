using System.Device.Pwm;
using System.Device.Pwm.Drivers;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Włączenie obsługi plików statycznych (szuka w folderze wwwroot)
app.UseDefaultFiles(); // Pozwala serwować index.html przy wejściu na "/"
app.UseStaticFiles();

// GPIO Setup
int pinR = 16, pinG = 20, pinB = 21;
using var pwmR = new SoftwarePwmChannel(pinR, 400, 0);
using var pwmG = new SoftwarePwmChannel(pinG, 400, 0);
using var pwmB = new SoftwarePwmChannel(pinB, 400, 0);

pwmR.Start(); pwmG.Start(); pwmB.Start();

// API Endpoint
app.MapPost("/set-color", ([FromQuery] string hex) =>
{
    try
    {
        int r = int.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        int g = int.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        int b = int.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);

        pwmR.DutyCycle = r / 255.0;
        pwmG.DutyCycle = g / 255.0;
        pwmB.DutyCycle = b / 255.0;

        return Results.Ok();
    }
    catch { return Results.BadRequest(); }
});

app.Run("http://0.0.0.0:5000");
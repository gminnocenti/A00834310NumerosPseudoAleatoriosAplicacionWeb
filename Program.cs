using System.Formats.Asn1;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MathNet.Numerics.Distributions; //
using WebApplication1;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
var numerosGenerator = new numerospsedoaleatorios();

// Add services if needed
// app.Services.Add...

app.MapGet("/", (HttpContext context) =>
{
    // Display an HTML form to collect user input
    return context.Response.WriteAsync(
        "<html><head><title>Numeros Pseudo Aleatorios y su Prueba de Bondad</title>" +
        "<style>" +
        "body { font-family: Arial, sans-serif; background-color: #f0f0f0; }" +
        "h1 { color: #333; text-align: center; }" +
        "form { max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; box-shadow: 0 0 10px rgba(0,0,0,0.1); }" +
        "label { display: block; margin-bottom: 5px; font-weight: bold; }" +
        "input[type='text'] { width: 100%; padding: 8px; margin-bottom: 10px; border: 1px solid #ccc; }" +
        "input[type='submit'] { background-color: #333; color: #fff; border: none; padding: 10px 20px; cursor: pointer; }" +
        "input[type='submit']:hover { background-color: #555; }" +
        "</style></head><body>" +
        "<h1>Generacion de Numeros Pseudo Aleatorios</h1>" +
        "<form method='post' action='/calculate'>" +
        //semilla
        "<label for='semilla'>Ingrese una semilla o X0:</label> <input type='text' name='semilla' /><br />" +
        "<label for='cantidadNumeros'>Ingrese cantidad de numeros a generar:</label> <input type='text' name='cantidadNumeros' /><br />" +
        "<label for='a'>Ingrese valor parametro a:</label> <input type='text' name='a' /><br />" +
        "<label for='c'>Ingrese valor parametro c:</label> <input type='text' name='c' /><br />" +
        "<label for='m'>Ingrese valor parametro m:</label> <input type='text' name='m' /><br />" +
        "<h1>Parametros para la Prueba de Bondad de Chi Cuadrada</h1>" +
        "<label for='alfa'>Ingrese valor parametro alfa:</label> <input type='text' name='alfa' /><br />" +
        "<input type='submit' value='Calculate' />" +
        "</form></body></html>");
});
app.MapPost("/calculate", (HttpContext context) =>
{
    // Get user input values from the form
    var semillastr = context.Request.Form["semilla"];
    var cantidadNumerosstr = context.Request.Form["cantidadNumeros"];
    var astr = context.Request.Form["a"];
    var cstr = context.Request.Form["c"];
    var mstr = context.Request.Form["m"];
    var alfastr = context.Request.Form["alfa"];

    if (int.TryParse(semillastr, out int semilla) && int.TryParse(cantidadNumerosstr, out int cantidadNumeros)
                                                  && int.TryParse(astr, out int a)
                                                  && int.TryParse(cstr, out int c)
                                                  && int.TryParse(mstr, out int m)
                                                  && double.TryParse(mstr, out double alfa))
    {
        // Call your calculation method with the user's input
        double[,] numerosgenerados = numerosGenerator.CongruencialLineal(semilla, cantidadNumeros, a, c, m);



        string html = "<html><head><title>Numeros generados por medio de Congruencia Lineas</title>" +
                      "<style>" +
                      "body { font-family: Arial, sans-serif; background-color: #f0f0f0; }" +
                      "h1 { color: #333; text-align: center; }" +
                      ".container { max-width: 800px; margin: 0 auto; padding: 20px; background-color: #fff; box-shadow: 0 0 10px rgba(0,0,0,0.1); }" +
                      "table { border-collapse: collapse; width: 100%; margin: 20px 0; }" +
                      "th, td { border: 1px solid #333; padding: 8px; text-align: center; }" +
                      "th { background-color: #333; color: #fff; }" +
                      "p { margin: 10px 0; }" +
                      ".success { color: green; }" +
                      ".error { color: red; }" +
                      ".blue-text { color: blue; }" + 
                      ".value-box {\n    display: inline-block;\n    border: 2px solid #333;\n   " +
                      " padding: 5px 10px;\n    background-color: #f0f0f0;\n    " +
                      "font-weight: bold; /* Make the text bold within the box */\n    margin: 0;\n}" +
                      ".back-button { text-align: center; }" +
                      "</style></head><body>" +
                      "<h1>Numeros generados por medio de Congruencia Lineas</h1>" +
                      "<div class='container'>" +
                      "<table><tr><th>a</th><th>c</th><th>m</th><th>Numero Generado</th></tr>";

// Iterate through the numerosgenerados array and populate the table
for (int i = 0; i < cantidadNumeros; i++)
{
    html += "<tr>";
    for (int j = 0; j < 4; j++)
    {
        if (j == 3) // Check if it's the last column (Numero Generado)
        {
            html += $"<td class='blue-text'>{numerosgenerados[i, j]}</td>"; // Apply the blue-text class
        }
        else
        {
            html += $"<td>{numerosgenerados[i, j]}</td>";
        }
    }
    html += "</tr>";
}

html += "</table>";
 // Add a button to go back to the input page

html += "<div style='text-align: center; border: 2px solid #333; padding: 10px; margin: 10px auto; width: 50%; background-color: #f0f0f0;'>";

html += "<h1>Prueba de bondad CHi Cuadrada</h1>";
double[] resultados = numerosGenerator.chicuadradapruebabondad(numerosgenerados, alfa);



double valorchicuadrada = resultados[0];
double valorcritico = resultados[1];
html +=$"El valor de chi cuadrada para los datos es <span class='value-box'>{valorchicuadrada}</span></p>";
html += $"<p>El valor critico para los datos generados es <span class='value-box'>{valorcritico}</span></p>";

if (valorchicuadrada <= valorcritico)
{
    html += "<p class='success'>Los datos siguen la distribucion de probabilidad Chi cuadrada.</p>";
}
else
{
    html += "<p class='error'>Los datos no siguen la distribucion de probabilidad Chi cuadrada.</p>";
}
html += "</div>";
html += "<div class='back-button'>" +
        "<a href='/'>Back to Input Page</a>" +
        "</div>";
html += "</div>"; // Close the container div

html += "</body></html>";


        // Return the HTML page as the response
        return context.Response.WriteAsync(html);
    }
    else
    {
        return context.Response.WriteAsync("Invalid input. Please enter valid values.");
    }
});




app.Run();
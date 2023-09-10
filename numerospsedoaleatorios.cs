using MathNet.Numerics.Distributions; 

namespace WebApplication1;

public class numerospsedoaleatorios
{
    public double[,] CongruencialLineal(int semilla, int cantidadNumeros, int a, int c, int m)
{
    int x = semilla; // Inicialización con la semilla

    double[,] numerosgenerados = new double[cantidadNumeros, 4]; // 2D array with 4 columns
    int a1 = 0;
    int c1 = 0;
    int m1 = 0;
    for (int i = 0; i < cantidadNumeros; i++)
    {
        a1 = x;
        c1 = a * x + c;
        m1 = (a * x + c) % m;
        
        x = (a * x + c) % m;
        numerosgenerados[i, 0] = a1; // guardar valor de a
        numerosgenerados[i, 1] = c1; // guardar valor de c
        numerosgenerados[i, 2] = m1; // guardar valor de m
        numerosgenerados[i, 3] = x; // guardar valor de x
    }

    return numerosgenerados;
}


double calcvalorcriticochicuadrada(double alfa, double degreesOfFreedom) //metodo que encuentra el valor critico para un serie de valores de tamano n
        {
            // Utiliza la función InverseChiSquared en MathNet.Numerics para obtener el valor crítico
            //crear variable que llama los paquetes especificos
            // Check and adjust alpha if needed to avoid issues
            if (alfa >= 1.0)
            {
                alfa = 0.9999; // Set it to a smaller value if it's too close to 1.0
            }
            var chi = new ChiSquared(degreesOfFreedom);
            //importar paquete Math.Net.Numerics
            double valorcriticoprueba = chi.InverseCumulativeDistribution(1.0 - alfa);

            return valorcriticoprueba;
        }

double medianumerosgenerados(double[] numerosgenerados) //media para un set de datos
        {
            double suma = 0;
            foreach (var valor in numerosgenerados)
            {
                suma += valor;
            }
            double media=suma / numerosgenerados.Length;
            return media;
        }
double[] ExtractLastColumn(double[,] array)
{
    int rowCount = array.GetLength(0); // Get the number of rows
    double[] lastColumn = new double[rowCount];

    for (int i = 0; i < rowCount; i++)
    {
        int columnCount = array.GetLength(1); // Get the number of columns
        lastColumn[i] = array[i, columnCount - 1]; // Get the value from the last column
    }

    return lastColumn;
}

public double[] chicuadradapruebabondad(double[,] numerosgenerados,double alfa) //metodo que hace la prueba de bondad
        {
            double valorchicuadrada = 0;
            //valor esperado para el set de datos
            double[] numerosgenerados2 = ExtractLastColumn(numerosgenerados);
            
            double media = medianumerosgenerados(numerosgenerados2);

            for (int i = 0; i < numerosgenerados2.Length; i++)// este loop calcula el valor de chicuadrada para todo el set de datos
            {
                valorchicuadrada += Math.Pow(numerosgenerados2[i] - media, 2) / media;
            }
            double degreesOfFreedom = numerosgenerados2.Length - 1;
            //manda a llamar el valor critico para hacer comparacion
            double valorcriticoprueba = calcvalorcriticochicuadrada(alfa, degreesOfFreedom);


            double[] resultados = new double[2];
            resultados[0] = valorchicuadrada;
            resultados[1] = valorcriticoprueba;
            return  resultados;

        }
}
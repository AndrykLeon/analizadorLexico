using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int edo, cont, tam, car, ultimoEdo, preUltimoEdo, contadorLineas, contadorCadenas;
        string[] txt;
        int[,] tt = { {0, 0, 0, 1, 0, 0, 0},
                      {7, 0, 3, 1, 5, 2, 0},
                      {7, 0, 0, 7, 7, 7, 0},
                      {7, 7, 7, 4, 7, 7, 0},
                      {7, 0, 7, 4, 5, 2, 0},
                      {7, 0, 0, 6, 7, 7, 0},
                      {7, 0, 0, 6, 7, 7, 0},
                      {7, 0, 7, 7, 7, 7, 0}};
        string[] tipoCadena = { "Natural", "Porcentaje", "Real", "Exponencial"};

        string salida = "";

        List<int> estadosDeAceptacion = new List<int> { 1, 2, 4, 6};
        List<string> cadenaAMostrar = new List<string>();
        List<char> uno = new List<char> {';', '(', ')'};
        List<char> dos = new List<char> { ',', '.'};
        List<char> tres = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        List<char> seis = new List<char> { ' ', '\n' };


        public Form1()
        {
            InitializeComponent();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string cadenas = richTextBox1.Text;

            realizarAutomata(cadenas);
        }
        private void realizarAutomata(string cadenasPorLineas)
        {
            string palabra = "";
            char[] cadena = cadenasPorLineas.ToCharArray();
            edo = 0; cont = 0; ultimoEdo = 0; preUltimoEdo = 0; contadorLineas = 1; contadorCadenas = 1; tam = cadena.Length;

            while (cont < tam)
            {
                car = SacarCar(cadena[cont]);
                edo = tt[edo, car];

                if (edo != 0)
                    cadenaAMostrar.Add(cadena[cont].ToString());

                ImprimirFila(edo, palabra, cadena);

                cont++;
                preUltimoEdo = ultimoEdo;
                ultimoEdo = edo;
            }
        }

        private int SacarCar(char caracter)
        {
            int resultado = 999;

            if (uno.Contains(caracter))
                resultado = 1;

            if (dos.Contains(caracter))
                resultado = 2;

            if (tres.Contains(caracter))
                resultado = 3;

            if (caracter == 'E')
                resultado = 4;

            if (caracter == '%')
                resultado = 5;

            if (seis.Contains(caracter))
            {
                resultado = 6;
                if(caracter == '\n')
                    contadorLineas++;
            }

            if (resultado == 999)
                resultado = 0;

            return resultado;
        }
        private string SacarTipo(int estado)
        {
            string resultado = "Invalido";

            if(estadosDeAceptacion.Contains(estado))
            {
                switch(estado)
                {
                    case 1:
                        resultado = tipoCadena[0];
                        break;

                    case 2:
                        resultado = tipoCadena[1];
                        break;

                    case 4:
                        resultado = tipoCadena[2];
                        break;

                    case 6:
                        resultado = tipoCadena[3];
                        break;
                }
            }

            return resultado;
        }

        private void ImprimirFila (int edo, string palabra, char[] cadena)
        {
            if ((edo == 0) && cadenaAMostrar.Count > 0)
            {
                string tipo = "";
                palabra = "";

                foreach (var letra in cadenaAMostrar)
                {
                    palabra = palabra + letra;
                }

                if (cont > 0)
                {
                    if (dos.Contains(cadena[cont - 1]))
                    {
                        palabra = palabra.Remove(palabra.Length - 1);
                        tipo = SacarTipo(preUltimoEdo);
                    }
                    else
                        tipo = SacarTipo(ultimoEdo);
                }
                else
                    tipo = SacarTipo(ultimoEdo);

                dataGridView1.Rows.Add(contadorCadenas, contadorLineas, palabra, tipo);
                contadorCadenas++;
                cadenaAMostrar.Clear();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Projeto_Hospital
{
    internal class Fila
    {

        public Paciente Cabeca { get; set; }
        public Paciente Cauda { get; set; }
        public int Elementos { get; set; } 
        public string PathFile { get; set; }

        public Fila()
        {
            Cabeca = null;
            Cauda = null;
            Elementos = 0;
            PathFile = DateTime.Now.ToString("dd/MM/yyyy").Replace("/", "_");
        }



        public void Imprimir()
        {
            if (Elementos == 0)
            {
                Console.WriteLine("[ A fila está vazia ]");
                return;
            }

            Paciente paciente = Cabeca;

            do
            {
                Console.WriteLine(paciente.ToString());
                Console.WriteLine("\n********************************************");
                paciente = paciente.Proximo;

            } while (paciente != null);
        }

        public void Inserir(Paciente paciente)
        {
            if (Vazia())
            {
                Cabeca = paciente;
                Cauda = paciente;
            }
            else
            {
                Cauda.Proximo = paciente;
                paciente.Anterior = Cauda;
                Cauda = paciente;
            }

            Elementos++;
        }

        public Paciente Buscar(string cpf)
        {
            if (Elementos == 0) return null;

            Paciente paciente = Cabeca;

            do
            {
                if (cpf == paciente.CPF)
                    return paciente;

                paciente = paciente.Proximo;

            } while (paciente != null);

            return null;
        }

        public void Remover(string cpf)
        {
            if (Elementos == 0) return;

            if (Cabeca.CPF == cpf)
            {
                Cabeca = Cabeca.Proximo;
                Elementos--;
            }

            if (Cabeca == null)
                Cauda = null;
        }



        public void InserirDadosNoArquivo(Paciente paciente, string arquivo)
        {
            bool aguardandoNaFila = false;

            try
            {
                StreamReader sr = new StreamReader($"{PathFile}\\{arquivo}.txt");

                string line = sr.ReadLine();

                while (line != null)
                {
                    string[] dados = line.Split(";");

                    if (paciente.CPF == dados[0])
                    {
                        aguardandoNaFila = true;
                        sr.Close();
                        break;
                    }

                    line = sr.ReadLine();
                }

                sr.Close();

                if (!aguardandoNaFila)
                {
                    StreamWriter sw = new StreamWriter($"{PathFile}\\{arquivo}.txt", append: true);
                    sw.WriteLine($"{paciente.CPF};{paciente.Nome};{paciente.DataNasc.ToString("dd/MM/yyyy")};{paciente.Sexo};");
                    sw.Close();

                    Inserir(paciente);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }

        public void CarregarDadosDoArquivo(Fila fila, string arquivo)
        {
            bool arquivoFila = false;
            bool diretorioFila = false;


            try
            {
                if (Directory.Exists(PathFile))
                    diretorioFila = true;
                else
                {
                    Directory.CreateDirectory(PathFile);
                    diretorioFila = true;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }

            if (diretorioFila)
            {

                try
                {
                    if (File.Exists($"{PathFile}\\{arquivo}.txt"))
                        arquivoFila = true;
                    else
                        File.Create($"{PathFile}\\{arquivo}.txt").Close();

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.ToString());
                }


                if (arquivoFila)
                {
                    try
                    {
                        StreamReader sr = new StreamReader($"{PathFile}\\{arquivo}.txt");

                        string line = sr.ReadLine();

                        while (line != null)
                        {
                            string[] dados = line.Split(";");

                            Paciente paciente = new Paciente(dados[0], dados[1], DateTime.Parse(dados[2]), dados[3]);

                            fila.Inserir(paciente);

                            line = sr.ReadLine();
                        }

                        sr.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.ToString());
                    }
                }
            }


        }

        public void RemoverDadosDoArquivo(Fila fila, string arquivo)
        {
            try
            {
                StreamWriter sw = new StreamWriter($"{PathFile}\\{arquivo}.txt");

                fila.Remover(fila.Cabeca.CPF);

                for (Paciente paciente = Cabeca; paciente != null; paciente = paciente.Proximo)
                    sw.WriteLine($"{paciente.CPF};{paciente.Nome};{paciente.DataNasc.ToString("dd/MM/yyyy")};{paciente.Sexo};");

                sw.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        public bool Vazia()
        {
            return (Cabeca == null && Cauda == null) ? true : false;
        }
    }

}

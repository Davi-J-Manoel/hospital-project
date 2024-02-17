using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto_Hospital
{
    internal class Paciente
    {
        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime DataNasc { get; set; }
        public string Sexo { get; set; }

        public Paciente Proximo { get; set; }
        public Paciente Anterior { get; set; }


        public Paciente() { }

        public Paciente(string cPF, string nome, DateTime dataNasc, string sexo)
        {
            CPF = cPF;
            Nome = nome;
            DataNasc = dataNasc;
            Sexo = sexo;
            Proximo = null;
            Anterior = null;
        }


        public override string ToString()
        {
            return $"\nCPF: {CPF}\nNome: {Nome}\nDt. Nasc.: {DataNasc.ToString("dd/MM/yyyy")}\nSexo: {Sexo}";
        }

        public void SalvarInformacoesDoPacienteNoArquivo()
        {
            try
            {
                StreamWriter sw = new StreamWriter("Pacientes.txt", append: true);
                sw.WriteLine($"{CPF};{Nome};{DataNasc.ToString("dd/MM/yyyy")};{Sexo};");
                sw.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }

        public Paciente BuscarPacienteNoArquivo(string cpf)
        {
            Paciente paciente;

            try
            {
                if (!File.Exists("Pacientes.txt"))
                {
                    File.Create("Pacientes.txt").Close();
                    return null;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }

            try
            {
                StreamReader sr = new StreamReader("Pacientes.txt");
                string line = sr.ReadLine();

                while (line != null)
                {
                    string[] dados = line.Split(";");

                    if (cpf == dados[0])
                    {
                        paciente = new Paciente(dados[0], dados[1], DateTime.Parse(dados[2]), dados[3]);

                        sr.Close();

                        return paciente;
                    }

                    line = sr.ReadLine();
                }

                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }

            return null;
        }


        public void SalvarHistoricoDoPaciente(string cpf, string resultadoTeste, string[] sintomas, int dias, string[] comorbidades, string situacao)
        {
            bool historicoPaciente = false;

            try
            {
                if (Directory.Exists("Historico"))
                    historicoPaciente = true;
                else
                {
                    Directory.CreateDirectory("Historico");
                    historicoPaciente = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            if (historicoPaciente)
            {
                try
                {
                    string historico = "";

                    StreamWriter sw = new StreamWriter($"Historico\\{cpf}.txt", append: true);

                    historico += resultadoTeste + ";";

                    for (int i = 0; i < sintomas.Length; i++)
                        historico += sintomas[i] + ";";

                    historico += dias + ";";

                    for (int i = 0; i < comorbidades.Length; i++)
                        historico += comorbidades[i] + ";";

                    historico += situacao + ";";

                    sw.WriteLine(historico);

                    sw.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.ToString());
                }
            }

        }

        public void CarregarHistoricoDoPaciente(string cpf)
        {
            bool historico = false;
            bool historicoPaciente = false;

            try
            {
                if (Directory.Exists("Historico"))
                    historico = true;
                else
                {
                    Directory.CreateDirectory("Historico");
                    historico = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            if (historico)
            {
                string[] pacientes = Directory.GetFiles("Historico")
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToArray();

                foreach (string paciente in pacientes)
                    if (cpf == paciente)
                        historicoPaciente = true;


                if (historicoPaciente)
                {

                    try
                    {
                        string line;
                        int consultas = 1;

                        StreamReader sr = new StreamReader($"Historico\\{cpf}.txt");

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] dados = line.Split(";");

                            Console.WriteLine($"\n\nConsulta: #000{consultas} -----------------------------------------");

                            Console.WriteLine($"\nResultado teste de Covid: {dados[0]}");

                            Console.WriteLine("\n[Sintomas]");
                            Console.WriteLine($"Febre: {dados[1]} \n" +
                                $"Dor de Cabeça: {dados[2]}\n" +
                                $"Falta de Paladar: {dados[3]}\n" +
                                $"Falta de Olfato:  {dados[4]}");
                            Console.WriteLine($"\nQuantidade de dias com sintomas: {dados[5]}");


                            Console.Write("\n[Comorbidades] ");

                            if (dados[6] == null || dados[6] == "")
                                Console.WriteLine("Nenhuma");
                            else
                            {
                                Console.WriteLine();
                                for (int i = 6; i < 11; i++)
                                    if (dados[i] != null || dados[i] != "")
                                        Console.WriteLine(dados[i]);
                            }

                            Console.WriteLine($"\nSituação: {dados[11]}");

                            consultas++;
                        }

                        sr.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                }
            }
        }
    }
}

using System;

namespace Projeto_Hospital
{

    internal class Program
    {
        public static Fila fila_normal = new Fila();
        public static Fila fila_preferencial = new Fila();

        static void Main(string[] args)
        {
            fila_normal.CarregarDadosDoArquivo(fila_normal, "FilaNormal");
            fila_preferencial.CarregarDadosDoArquivo(fila_preferencial, "FilaPreferencial");

            string acao;
            int preferencial = 0;

            int senhas = 0;

            do
            {
                Console.Clear();

                switch (acao = Menu())
                {
                    case "0":
                        Console.Clear();
                        Console.WriteLine("Saindo...");
                        break;

                    case "1":
                        NovoPaciente();
                        senhas++;

                        break;

                    case "2":
                        if (fila_preferencial.Elementos > 0 && preferencial < 2)
                        {
                            Console.WriteLine(fila_preferencial.Cabeca.ToString());
                            preferencial++;

                            ChamarExame(fila_preferencial, "FilaPreferencial");
                        }
                        else if (fila_normal.Elementos > 0)
                        {
                            Console.WriteLine(fila_normal.Cabeca.ToString());
                            preferencial = 0;

                            ChamarExame(fila_normal, "FilaNormal");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("As filas estão vazias.");
                            preferencial = 0;
                        }
                        break;

                    case "3":
                        BuscarPacienteNaFila();
                        break;

                    case "4":
                        BuscarHistorico();
                        break;

                    case "5":
                        Console.Clear();
                        Console.WriteLine("******************** Fila Normal *******************");
                        fila_normal.Imprimir();
                        break;

                    case "6":
                        Console.Clear();
                        Console.WriteLine("******************** Fila Preferencial *******************");
                        fila_preferencial.Imprimir();
                        break;

                    default:
                        Console.Clear();
                        Console.WriteLine("Ação inválida");
                        break;
                }

                Console.ReadKey();

            } while (acao != "0");
        }

        private static string Menu()
        {
            Console.WriteLine("******************** Menu *******************");
            Console.WriteLine("[1] Chamar Próximo Paciente");
            Console.WriteLine("[2] Chamar Paciente para Exame");
            Console.WriteLine("[3] Buscar Paciente na Fila");
            Console.WriteLine("[4] Buscar Histórico do Paciente");
            Console.WriteLine("*********************************************");
            Console.WriteLine("[5] Visualizar Pacientes na Fila Normal");
            Console.WriteLine("[6] Visualizar Pacientes na Fila Preferencial");
            Console.WriteLine("*********************************************");
            Console.WriteLine("[0] Sair");
            Console.WriteLine("*********************************************");
            Console.WriteLine();
            Console.Write(":: ");

            return Console.ReadLine();
        }

        private static void NovoPaciente()
        {
            Console.Clear();

            Paciente paciente = new Paciente();

            Console.WriteLine("****************** Buscar Paciente ****************");
            Console.Write("\nInforme o CPF: ");
            string cpf = Console.ReadLine();
            Console.WriteLine("\n***************************************************");

            paciente = paciente.BuscarPacienteNoArquivo(cpf);

            if (paciente != null)
            {
                Console.Clear();

                Console.WriteLine("****************** Ficha do Paciente ****************");
                Console.WriteLine(paciente.ToString());
                Console.WriteLine("\n*****************************************************");
            }
            else
                paciente = CadastrarPaciente(cpf);

            if ((DateTime.Now.Year - paciente.DataNasc.Year) >= 60)
                fila_preferencial.InserirDadosNoArquivo(paciente, "FilaPreferencial");
            else
                fila_normal.InserirDadosNoArquivo(paciente, "FilaNormal");

        }

        private static Paciente CadastrarPaciente(string cpf)
        {
            Console.Clear();

            string sexo;

            Console.WriteLine("********** Paciente **********");

            Console.WriteLine($"\nInforme o CPF:\n{cpf}");

            Console.WriteLine("\nInforme o Nome:");
            string nome = Console.ReadLine().ToUpper();

            Console.WriteLine("\nInforme o Data Nasc.:");
            DateTime dataNasc = DateTime.Parse(Console.ReadLine());

            #region Verifica Sexo
            do
            {
                Console.WriteLine("\nInforme o Sexo: [M]asculino ou [F]eminino");
                sexo = Console.ReadLine().ToUpper();

                if (sexo != "M" && sexo != "F") Console.WriteLine("Opção inválida.");

            } while (sexo != "M" && sexo != "F");

            Console.WriteLine("\n******************************");

            sexo = (sexo == "M") ? "MASCULINO" : "FEMININO";
            #endregion

            Paciente paciente = new Paciente(cpf, nome, dataNasc, sexo);

            paciente.SalvarInformacoesDoPacienteNoArquivo();

            return paciente;
        }

        public static void ChamarExame(Fila fila, string arquivo)
        {
            Console.Clear();

            Console.WriteLine(fila.Cabeca.ToString());

            string opcao;

            do
            {
                Console.WriteLine("\nQual o resultado do teste de Covid-19?");
                Console.WriteLine("[1] Positivo");
                Console.WriteLine("[2] Negativo");
                Console.WriteLine("[3] Não Reagente");
                opcao = Console.ReadLine();

            } while (opcao != "1" && opcao != "2" && opcao != "3");

            string resultadoTeste = (opcao == "1")
                ? "Positivo"
                : (opcao == "2")
                ? "Negativo"
                : "Não Reagente";

            Console.WriteLine("\nQuantidade em dias com os sintomas: ");
            int dias = int.Parse(Console.ReadLine());


            string[] sintomas = Sintomas();

            string[] comorbidades = Comorbidades();


            Console.Clear();

            Console.WriteLine(fila.Cabeca.ToString());

            Console.WriteLine($"\nResultado teste de Covid: {resultadoTeste}");

            Console.WriteLine("\n[Sintomas]");
            Console.WriteLine($"Febre: {sintomas[0]} \n" +
                $"Dor de Cabeça: {sintomas[1]}\n" +
                $"Falta de Paladar: {sintomas[2]}\n" +
                $"Falta de Olfato:  {sintomas[3]}");
            Console.WriteLine($"\nQuantidade de dias com sintomas: {dias}");


            Console.Write("\n[Comorbidades] ");

            if (comorbidades[0] == null)
                Console.WriteLine("Nenhuma");
            else
            {
                Console.WriteLine();
                foreach (string comorbidade in comorbidades)
                    if (comorbidade != null)
                        Console.WriteLine(comorbidade);
            }

            string acao;

            do
            {
                Console.WriteLine($"\nO que deseja fazer com o paciente {fila.Cabeca.Nome}? ");
                Console.WriteLine("[1] Dar alta");
                Console.WriteLine("[2] Colocar em quarentena");
                Console.WriteLine("[3] Mandar para emergência");

                acao = Console.ReadLine();

                switch (acao)
                {
                    case "1":
                        fila.Cabeca.SalvarHistoricoDoPaciente(fila.Cabeca.CPF, resultadoTeste, sintomas, dias, comorbidades, "Liberado");
                        fila.RemoverDadosDoArquivo(fila, arquivo);
                        break;

                    case "2":
                        fila.Cabeca.SalvarHistoricoDoPaciente(fila.Cabeca.CPF, resultadoTeste, sintomas, dias, comorbidades, "Em Quarentena");
                        fila.RemoverDadosDoArquivo(fila, arquivo);
                        break;

                    case "3":
                        break;

                    default:
                        Console.WriteLine("Ação inválida");
                        break;
                }

            } while (acao != "1" && acao != "2" && acao != "3");
        }

        private static void BuscarPacienteNaFila()
        {
            Console.Clear();

            Paciente paciente = new Paciente();

            Console.WriteLine("****************** Buscar Paciente na Fila ****************");
            Console.Write("\nInforme o CPF: ");
            string cpf = Console.ReadLine();
            Console.WriteLine("\n*********************************************************");

            if ((paciente = fila_normal.Buscar(cpf)) != null)
            {
                Console.WriteLine(paciente.ToString());
                Console.WriteLine("Aguardando na fila normal");
            }
            else if ((paciente = fila_preferencial.Buscar(cpf)) != null)
            {
                Console.WriteLine(paciente.ToString());
                Console.WriteLine("Aguardando na fila preferencial");
            }
            else
            {
                Console.WriteLine("Paciente não está em nenhuma fila.");
            }
        }

        private static void BuscarHistorico()
        {
            Console.Clear();

            Paciente paciente = new Paciente();

            Console.WriteLine("****************** Buscar Paciente ****************");
            Console.Write("\nInforme o CPF: ");
            string cpf = Console.ReadLine();
            Console.WriteLine("\n***************************************************");

            paciente = paciente.BuscarPacienteNoArquivo(cpf);

            if (paciente != null)
            {
                Console.Clear();

                Console.WriteLine("******************** Ficha do Paciente ******************");
                Console.WriteLine(paciente.ToString());
                Console.WriteLine("\n****************** Histórico do Paciente ****************");
                paciente.CarregarHistoricoDoPaciente(cpf);
                Console.WriteLine("\n*********************************************************");
            }
            else
                Console.WriteLine("Histórico não encontrado");

        }

        private static string[] Sintomas()
        {
            string[] sintomas = new string[4] { "NÃO", "NÃO", "NÃO", "NÃO" };

            string sintoma;

            do
            {
                Console.WriteLine("\nEsta ou esteve com febre? [S - SIM] [N - NÃO]");
                sintoma = Console.ReadLine().ToUpper();
                if (sintoma == "S")
                {
                    sintomas[0] = "SIM";
                }
                else if (sintoma == "N")
                {
                    sintomas[0] = "NÃO";
                }
                else
                {
                    Console.WriteLine("Opção inválida!!!");
                }
            } while (sintoma != "S" && sintoma != "N");

            do
            {
                Console.WriteLine("\nEsta ou esteve com dor de cabeça? [S - SIM] [N - NÃO]");
                sintoma = Console.ReadLine().ToUpper();
                if (sintoma == "S")
                {
                    sintomas[1] = "SIM";
                }
                else if (sintoma == "N")
                {
                    sintomas[1] = "NÃO";
                }
                else
                {
                    Console.WriteLine("Opção inválida!!!");
                }
            } while (sintoma != "S" && sintoma != "N");

            do
            {
                Console.WriteLine("\nEsta ou esteve com falta de paladar [S - SIM] [N - NÃO]");
                sintoma = Console.ReadLine().ToUpper();
                if (sintoma == "S")
                {
                    sintomas[2] = "SIM";
                }
                else if (sintoma == "N")
                {
                    sintomas[2] = "NÃO";
                }
                else
                {
                    Console.WriteLine("Opção inválida!!!");
                }
            } while (sintoma != "S" && sintoma != "N");

            do
            {
                Console.WriteLine("\nEsta ou esteve com falta de olfato? [S - SIM] [N - NÃO]");
                sintoma = Console.ReadLine().ToUpper();
                if (sintoma == "S")
                {
                    sintomas[3] = "SIM";
                }
                else if (sintoma == "N")
                {
                    sintomas[3] = "NÃO";
                }
                else
                {
                    Console.WriteLine("Opção inválida!!!");
                }

            } while (sintoma != "S" && sintoma != "N");


            return sintomas;
        }

        public static string[] Comorbidades()
        {
            string[] comorbidadeArray = new string[5] { null, null, null, null, null };

            Console.WriteLine("\nPossui comorbidade? [S - SIM] [N - NÃO]");
            string comorbidade = Console.ReadLine().ToUpper();
            int i = 1, x = 0;

            do
            {
                if (comorbidade == "N")
                {
                    break;
                }

                do
                {
                    if (comorbidade != "S" && comorbidade != "N")
                    {
                        Console.WriteLine("Opção inválida, tente novamente!");
                        Console.WriteLine("Possui comorbidade? [S - SIM] [N - NÃO]");
                        comorbidade = Console.ReadLine().ToUpper();
                    }
                } while (comorbidade != "S" && comorbidade != "N");

                if (comorbidade == "S")
                {
                    Console.Write($"\nInforme a comorbidade {i}: ");
                    comorbidadeArray[x] = Console.ReadLine();
                    Console.WriteLine("Possui mais alguma comorbidade? [S - SIM] [N - NÃO]");
                    comorbidade = Console.ReadLine().ToUpper();
                    x++;
                    i++;
                }

            } while (comorbidade != "N");

            return comorbidadeArray;
        }
    }
}

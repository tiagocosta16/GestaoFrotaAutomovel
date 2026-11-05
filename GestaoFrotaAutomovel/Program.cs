using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFrotaAutomovel
{
    class Program
    {
        // Lista de veículos da empresa.
        static List<Veiculo> frota = new List<Veiculo>();

        static void Main(string[] args)
        {
            int opcao;

            do
            {
                Console.Clear();

                // Título do menu.
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("==============================================");
                Console.WriteLine("             SISTEMA DE GESTÃO DE FROTA");
                Console.WriteLine("==============================================");
                Console.ResetColor();

                Console.WriteLine("\n1 - Registar veículo");
                Console.WriteLine("2 - Consultar veículos");
                Console.WriteLine("3 - Abastecer veículo");
                Console.WriteLine("4 - Realizar viagem");
                Console.WriteLine("5 - Consultar dados de um veículo");
                Console.WriteLine("6 - Remover veículo");
                Console.WriteLine("7 - Sair");

                Console.Write("\nEscolha uma opção: ");

                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Opção inválida!");
                    Console.ResetColor();
                    Console.ReadKey();
                    continue;
                }

                // Destaca visualmente a opção escolhida.
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n>> OPÇÃO SELECIONADA: " + opcao);
                Console.ResetColor();

                switch (opcao)
                {
                    case 1: RegistarVeiculo(); break;
                    case 2: ConsultarVeiculos(); break;
                    case 3: AbastecerVeiculo(); break;
                    case 4: RealizarViagem(); break;
                    case 5: ConsultarDadosVeiculo(); break;
                    case 6: RemoverVeiculo(); break;
                    case 7: Console.WriteLine("A sair do sistema..."); break;
                    default:
                        Console.WriteLine("Opção inválida!");
                        break;
                }

                if (opcao != 7)
                {
                    Console.WriteLine("\nPrima uma tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcao != 7);
        }

        // Regista um novo veículo.
        static void RegistarVeiculo()
        {
            Console.WriteLine("\n=== REGISTAR VEÍCULO ===");
            Console.Write("Introduza a matrícula: ");
            string matricula = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(matricula))
            {
                Console.WriteLine("Erro: A matrícula não pode estar vazia.");
                return;
            }

            matricula = matricula.Trim();

            foreach (Veiculo v in frota)
            {
                if (v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Erro: Matrícula já registada.");
                    return;
                }
            }

            Console.Write("Capacidade do depósito (litros): ");

            if (!int.TryParse(Console.ReadLine(), out int capacidade) || capacidade <= 0)
            {
                Console.WriteLine("Erro: Capacidade inválida.");
                return;
            }

            try
            {
                Veiculo novo = new Veiculo(matricula, capacidade);

                // Liga o evento ao método de alerta.
                novo.ReservaAtingida += MostrarAlertaReserva;

                frota.Add(novo);

                Console.WriteLine("Veículo registado com sucesso!");
                Console.WriteLine("Matrícula: " + novo.Matricula);
                Console.WriteLine("Depósito inicial: " + novo.NivelCombustivel.ToString("F2") + " litros");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        // Apresenta todos os veículos numa tabela.
        static void ConsultarVeiculos()
        {
            Console.WriteLine("\n=== VEÍCULOS REGISTADOS ===");

            if (frota.Count == 0)
            {
                Console.WriteLine("Não existem veículos registados.");
                return;
            }

            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("| Matrícula  | Quilometragem | Combustível | Capacidade       |");
            Console.WriteLine("---------------------------------------------------------------");

            foreach (Veiculo v in frota)
            {
                Console.WriteLine("| {0,-10} | {1,13} | {2,11:F2} | {3,16} |",
                    v.Matricula,
                    v.Quilometragem,
                    v.NivelCombustivel,
                    v.CapacidadeDeposito);
            }

            Console.WriteLine("---------------------------------------------------------------");
        }

        // Abastece um veículo escolhido.
        static void AbastecerVeiculo()
        {
            Console.WriteLine("\n=== ABASTECER VEÍCULO ===");

            Veiculo v = ProcurarVeiculo();
            if (v == null) return;

            Console.Write("Quantidade de combustível (litros): ");

            if (!double.TryParse(Console.ReadLine(), out double litros))
            {
                Console.WriteLine("Erro: Quantidade inválida.");
                return;
            }

            v.Abastecer(litros);
        }

        // Realiza uma viagem com um veículo.
        static void RealizarViagem()
        {
            Console.WriteLine("\n=== REALIZAR VIAGEM ===");

            Veiculo v = ProcurarVeiculo();
            if (v == null) return;

            Console.Write("Distância da viagem (km): ");

            if (!int.TryParse(Console.ReadLine(), out int distancia))
            {
                Console.WriteLine("Erro: Distância inválida.");
                return;
            }

            if (!v.RealizarViagem(distancia))
                Console.WriteLine("Não foi possível realizar a viagem.");
        }

        // Consulta os dados de um veículo específico.
        static void ConsultarDadosVeiculo()
        {
            Console.WriteLine("\n=== CONSULTAR DADOS ===");

            Veiculo v = ProcurarVeiculo();
            if (v == null) return;

            double percentagem = (v.NivelCombustivel / v.CapacidadeDeposito) * 100;

            Console.WriteLine("\nMatrícula: " + v.Matricula);
            Console.WriteLine("Quilometragem: " + v.Quilometragem + " km");
            Console.WriteLine("Combustível: " + v.NivelCombustivel.ToString("F2") + " litros");
            Console.WriteLine("Capacidade: " + v.CapacidadeDeposito + " litros");
            Console.WriteLine("Combustível disponível: " + percentagem.ToString("F2") + "%");

            // Indica o estado do depósito.
            Console.Write("Estado do depósito: ");

            if (v.NivelCombustivel < v.CapacidadeDeposito * 0.10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Reserva");
            }
            else if (v.NivelCombustivel >= v.CapacidadeDeposito)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Depósito cheio");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Combustível suficiente");
            }

            Console.ResetColor();
        }

        // Remove um veículo através da matrícula.
        static void RemoverVeiculo()
        {
            Console.WriteLine("\n=== REMOVER VEÍCULO ===");

            Veiculo v = ProcurarVeiculo();
            if (v == null) return;

            Console.Write("Tem a certeza de que pretende remover este veículo? (S/N): ");
            string resposta = Console.ReadLine();

            if (resposta.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                frota.Remove(v);
                Console.WriteLine("Veículo removido com sucesso!");
            }
            else
            {
                Console.WriteLine("Operação cancelada.");
            }
        }

        // Procura um veículo através da matrícula.
        static Veiculo ProcurarVeiculo()
        {
            Console.Write("Introduza a matrícula: ");
            string matricula = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(matricula))
            {
                Console.WriteLine("Matrícula inválida.");
                return null;
            }

            foreach (Veiculo v in frota)
            {
                if (v.Matricula.Equals(matricula.Trim(), StringComparison.OrdinalIgnoreCase))
                    return v;
            }

            Console.WriteLine("Veículo não encontrado.");
            return null;
        }

        // Apresenta o alerta quando o veículo entra na reserva.
        static void MostrarAlertaReserva(object sender, EventArgs e)
        {
            Veiculo v = (Veiculo)sender;
            ConsoleColor corAnterior = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n======================================");
            Console.WriteLine("          ALERTA DE RESERVA!");
            Console.WriteLine("======================================");
            Console.WriteLine("Veículo: " + v.Matricula);
            Console.WriteLine("Combustível: " + v.NivelCombustivel.ToString("F2") + " litros");
            Console.WriteLine("É necessário abastecer o veículo!");
            Console.WriteLine("======================================");

            Console.ForegroundColor = corAnterior;
        }
    }
}

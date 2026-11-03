using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFrotaAutomovel
{
    class Program
    {
        // Lista que permite guardar vários veículos.
        static List<Veiculo> frota = new List<Veiculo>();

        static void Main(string[] args)
        {
            // Variável que controla o funcionamento do menu.
            int opcao = 0;

            // Ciclo que mantém o menu ativo até o utilizador sair.
            do
            {
                Console.Clear();

                // Apresentação do menu principal.
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("======================================");
                Console.WriteLine("     SISTEMA DE GESTÃO DE FROTA");
                Console.WriteLine("======================================");
                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine("1 - Registar veículo");
                Console.WriteLine("2 - Consultar veículos");
                Console.WriteLine("3 - Abastecer veículo");
                Console.WriteLine("4 - Realizar viagem");
                Console.WriteLine("5 - Consultar dados de um veículo");
                Console.WriteLine("6 - Sair");

                Console.WriteLine();
                Console.Write("Escolha uma opção: ");

                // Lê e valida a opção escolhida.
                if (!int.TryParse(Console.ReadLine(), out opcao))
                {
                    Console.WriteLine("Opção inválida!");
                    Console.ReadKey();
                    continue;
                }

                Console.WriteLine();

                // Executa a funcionalidade escolhida.
                switch (opcao)
                {
                    case 1:
                        RegistarVeiculo();
                        break;

                    case 2:
                        ConsultarVeiculos();
                        break;

                    case 3:
                        AbastecerVeiculo();
                        break;

                    case 4:
                        RealizarViagem();
                        break;

                    case 5:
                        ConsultarDadosVeiculo();
                        break;

                    case 6:
                        Console.WriteLine("A sair do sistema...");
                        break;

                    default:
                        Console.WriteLine("Opção inválida! Escolha uma opção entre 1 e 6.");
                        break;
                }

                // Aguarda que o utilizador pressione uma tecla.
                if (opcao != 6)
                {
                    Console.WriteLine();
                    Console.WriteLine("Prima uma tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcao != 6);
        }

        // Método para registar um novo veículo na frota.
        static void RegistarVeiculo()
        {
            Console.WriteLine("=== REGISTAR VEÍCULO ===");

            // Pede a matrícula do veículo.
            Console.Write("Introduza a matrícula: ");
            string matricula = Console.ReadLine();

            // Verifica se a matrícula já existe.
            foreach (Veiculo v in frota)
            {
                if (v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Erro: Já existe um veículo com esta matrícula.");
                    return;
                }
            }

            // Pede a capacidade do depósito.
            Console.Write("Introduza a capacidade do depósito (litros): ");

            int capacidade;

            // Valida a capacidade introduzida.
            if (!int.TryParse(Console.ReadLine(), out capacidade) || capacidade <= 0)
            {
                Console.WriteLine("Erro: Capacidade inválida.");
                return;
            }

            try
            {
                // Cria um novo objeto da classe Veiculo.
                Veiculo novoVeiculo = new Veiculo(matricula, capacidade);

                // Associa o evento de reserva ao método de alerta.
                novoVeiculo.ReservaAtingida += MostrarAlertaReserva;

                // Adiciona o veículo à lista da frota.
                frota.Add(novoVeiculo);

                Console.WriteLine("Veículo registado com sucesso!");
                Console.WriteLine("Matrícula: " + novoVeiculo.Matricula);
                Console.WriteLine("Capacidade: " + novoVeiculo.CapacidadeDeposito + " litros");
                Console.WriteLine("Depósito inicial: " + novoVeiculo.NivelCombustivel + " litros");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
        }

        // Método para apresentar todos os veículos registados.
        static void ConsultarVeiculos()
        {
            Console.WriteLine("=== VEÍCULOS REGISTADOS ===");

            // Verifica se existem veículos na lista.
            if (frota.Count == 0)
            {
                Console.WriteLine("Não existem veículos registados.");
                return;
            }

            // Percorre a lista e apresenta os veículos.
            foreach (Veiculo v in frota)
            {
                Console.WriteLine("----------------------------------");
                Console.WriteLine("Matrícula: " + v.Matricula);
                Console.WriteLine("Quilometragem: " + v.Quilometragem + " km");
                Console.WriteLine("Combustível: " + v.NivelCombustivel + " litros");
                Console.WriteLine("Capacidade: " + v.CapacidadeDeposito + " litros");
            }
        }

        // Método para abastecer um veículo.
        static void AbastecerVeiculo()
        {
            Console.WriteLine("=== ABASTECER VEÍCULO ===");

            // Procura o veículo através da matrícula.
            Veiculo veiculo = ProcurarVeiculo();

            if (veiculo == null)
            {
                Console.WriteLine("Veículo não encontrado.");
                return;
            }

            // Pede a quantidade de combustível.
            Console.Write("Quantidade de combustível a adicionar (litros): ");

            double litros;

            // Valida a quantidade introduzida.
            if (!double.TryParse(Console.ReadLine(), out litros))
            {
                Console.WriteLine("Erro: Quantidade inválida.");
                return;
            }

            // Chama o método de abastecimento da classe Veiculo.
            veiculo.Abastecer(litros);
        }

        // Método para realizar uma viagem.
        static void RealizarViagem()
        {
            Console.WriteLine("=== REALIZAR VIAGEM ===");

            // Procura o veículo através da matrícula.
            Veiculo veiculo = ProcurarVeiculo();

            if (veiculo == null)
            {
                Console.WriteLine("Veículo não encontrado.");
                return;
            }

            // Pede a distância da viagem.
            Console.Write("Introduza a distância da viagem (km): ");

            int distancia;

            // Valida a distância introduzida.
            if (!int.TryParse(Console.ReadLine(), out distancia))
            {
                Console.WriteLine("Erro: Distância inválida.");
                return;
            }

            // Executa a viagem.
            bool sucesso = veiculo.RealizarViagem(distancia);

            // Apresenta o resultado da operação.
            if (sucesso)
            {
                Console.WriteLine("Viagem concluída!");
            }
            else
            {
                Console.WriteLine("Não foi possível realizar a viagem.");
            }
        }

        // Método para consultar os dados de um veículo específico.
        static void ConsultarDadosVeiculo()
        {
            Console.WriteLine("=== CONSULTAR DADOS DO VEÍCULO ===");

            // Procura o veículo através da matrícula.
            Veiculo veiculo = ProcurarVeiculo();

            if (veiculo == null)
            {
                Console.WriteLine("Veículo não encontrado.");
                return;
            }

            // Apresenta todas as informações do veículo.
            Console.WriteLine();
            Console.WriteLine("Matrícula: " + veiculo.Matricula);
            Console.WriteLine("Quilometragem: " + veiculo.Quilometragem + " km");
            Console.WriteLine("Nível de combustível: " + veiculo.NivelCombustivel + " litros");
            Console.WriteLine("Capacidade do depósito: " + veiculo.CapacidadeDeposito + " litros");

            // Calcula a percentagem de combustível disponível.
            double percentagem = (veiculo.NivelCombustivel / veiculo.CapacidadeDeposito) * 100;

            Console.WriteLine("Combustível disponível: " + percentagem.ToString("F2") + "%");
        }

        // Método auxiliar para procurar um veículo pela matrícula.
        static Veiculo ProcurarVeiculo()
        {
            Console.Write("Introduza a matrícula do veículo: ");

            string matricula = Console.ReadLine();

            // Percorre a lista à procura da matrícula.
            foreach (Veiculo v in frota)
            {
                if (v.Matricula.Equals(matricula, StringComparison.OrdinalIgnoreCase))
                {
                    return v;
                }
            }

            // Devolve null se não encontrar o veículo.
            return null;
        }

        // Método executado automaticamente quando a reserva é atingida.
        static void MostrarAlertaReserva(object sender, EventArgs e)
        {
            // Converte o objeto que enviou o evento para Veiculo.
            Veiculo veiculo = (Veiculo)sender;

            // Guarda a cor atual da consola.
            ConsoleColor corAnterior = Console.ForegroundColor;

            // Apresenta o alerta a vermelho.
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("          ALERTA DE RESERVA!");
            Console.WriteLine("======================================");
            Console.WriteLine("ATENÇÃO: O veículo " + veiculo.Matricula + " está na reserva!");
            Console.WriteLine("Nível de combustível: " + veiculo.NivelCombustivel + " litros");
            Console.WriteLine("É necessário abastecer o veículo.");
            Console.WriteLine("======================================");

            // Recupera a cor anterior da consola.
            Console.ForegroundColor = corAnterior;
        }
    }
}

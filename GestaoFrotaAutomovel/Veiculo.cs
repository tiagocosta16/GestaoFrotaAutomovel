using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFrotaAutomovel
{
    public class Veiculo
    {
        // Campos privados: guardam os dados internos do veículo.
        private int _quilometragem;
        private double _nivelCombustivel;

        // Propriedade automática para guardar a matrícula.
        public string Matricula { get; set; }

        // Propriedade automática para guardar a capacidade do depósito.
        public int CapacidadeDeposito { get; set; }

        // Propriedade completa para controlar a quilometragem.
        public int Quilometragem
        {
            get { return _quilometragem; }

            set
            {
                // Impede valores negativos.
                if (value < 0)
                {
                    Console.WriteLine("Erro: A quilometragem não pode ser negativa.");
                }
                // Impede que a quilometragem diminua.
                else if (value < _quilometragem)
                {
                    Console.WriteLine("Erro: A quilometragem não pode diminuir.");
                }
                else
                {
                    _quilometragem = value;
                }
            }
        }

        // Propriedade completa para controlar o nível de combustível.
        public double NivelCombustivel
        {
            get { return _nivelCombustivel; }

            set
            {
                // Verifica se o combustível está dentro dos limites permitidos.
                if (value < 0 || value > CapacidadeDeposito)
                {
                    Console.WriteLine("Erro: Nível de combustível inválido.");
                }
                else
                {
                    _nivelCombustivel = value;

                    // Verifica se o combustível está abaixo de 10%.
                    if (_nivelCombustivel < CapacidadeDeposito * 0.10)
                    {
                        // Dispara o evento de alerta de reserva.
                        ReservaAtingida?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        // Declaração do evento de alerta de reserva.
        public event EventHandler ReservaAtingida;

        // Construtor que obriga a indicar a matrícula e a capacidade do depósito.
        public Veiculo(string matricula, int capacidadeDeposito)
        {
            // Valida a matrícula.
            if (string.IsNullOrWhiteSpace(matricula))
            {
                throw new ArgumentException("A matrícula é obrigatória.");
            }

            // Valida a capacidade do depósito.
            if (capacidadeDeposito <= 0)
            {
                throw new ArgumentException("A capacidade do depósito deve ser superior a zero.");
            }

            Matricula = matricula;
            CapacidadeDeposito = capacidadeDeposito;

            // Inicializa a quilometragem a zero.
            _quilometragem = 0;

            // Inicializa o depósito completamente cheio.
            _nivelCombustivel = capacidadeDeposito;
        }

        // Método para adicionar combustível ao depósito.
        public void Abastecer(double litros)
        {
            // Impede quantidades iguais ou inferiores a zero.
            if (litros <= 0)
            {
                Console.WriteLine("Erro: A quantidade de combustível deve ser superior a zero.");
                return;
            }

            // Calcula o combustível que ficaria no depósito.
            double novoNivel = NivelCombustivel + litros;

            // Verifica se ultrapassa a capacidade máxima.
            if (novoNivel > CapacidadeDeposito)
            {
                Console.WriteLine("Aviso: A quantidade ultrapassa a capacidade do depósito.");

                // Enche apenas até à capacidade máxima.
                NivelCombustivel = CapacidadeDeposito;
            }
            else
            {
                // Atualiza o nível de combustível.
                NivelCombustivel = novoNivel;
            }

            Console.WriteLine("Abastecimento realizado com sucesso!");
            Console.WriteLine("Combustível atual: " + NivelCombustivel + " litros.");
        }

        // Método para simular uma viagem.
        public bool RealizarViagem(int kmDistancia)
        {
            // Verifica se a distância é válida.
            if (kmDistancia <= 0)
            {
                Console.WriteLine("Erro: A distância deve ser superior a zero.");
                return false;
            }

            // Calcula o combustível necessário para a viagem.
            double combustivelNecessario = kmDistancia * 0.07;

            // Verifica se existe combustível suficiente.
            if (NivelCombustivel < combustivelNecessario)
            {
                Console.WriteLine("Viagem recusada: Combustível insuficiente.");
                return false;
            }

            // Atualiza a quilometragem.
            Quilometragem += kmDistancia;

            // Retira o combustível consumido.
            NivelCombustivel -= combustivelNecessario;

            Console.WriteLine("Viagem realizada com sucesso!");
            Console.WriteLine("Distância percorrida: " + kmDistancia + " km.");
            Console.WriteLine("Combustível consumido: " + combustivelNecessario + " litros.");

            return true;
        }
    }
}

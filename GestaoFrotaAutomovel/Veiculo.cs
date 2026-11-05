using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestaoFrotaAutomovel
{
    public class Veiculo
    {
        // Campos privados.
        private int _quilometragem;
        private double _nivelCombustivel;

        // Propriedades automáticas.
        public string Matricula { get; set; }
        public int CapacidadeDeposito { get; private set; }

        // Propriedade com validação da quilometragem.
        public int Quilometragem
        {
            get { return _quilometragem; }
            set
            {
                if (value < 0 || value < _quilometragem)
                    Console.WriteLine("Erro: A quilometragem não pode diminuir.");
                else
                    _quilometragem = value;
            }
        }

        // Propriedade com validação do combustível.
        public double NivelCombustivel
        {
            get { return _nivelCombustivel; }
            set
            {
                if (value < 0 || value > CapacidadeDeposito ||
                    double.IsNaN(value) || double.IsInfinity(value))
                {
                    Console.WriteLine("Erro: Nível de combustível inválido.");
                }
                else
                {
                    bool estavaEmReserva = _nivelCombustivel < CapacidadeDeposito * 0.10;

                    _nivelCombustivel = value;

                    // Ativa o evento quando o combustível entra na reserva.
                    if (!estavaEmReserva && value < CapacidadeDeposito * 0.10)
                        ReservaAtingida?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        // Evento de alerta de reserva.
        public event EventHandler ReservaAtingida;

        // Construtor do veículo.
        public Veiculo(string matricula, int capacidadeDeposito)
        {
            if (string.IsNullOrWhiteSpace(matricula))
                throw new ArgumentException("A matrícula não pode estar vazia.");

            if (capacidadeDeposito <= 0)
                throw new ArgumentException("A capacidade deve ser superior a zero.");

            Matricula = matricula.Trim();
            CapacidadeDeposito = capacidadeDeposito;
            _quilometragem = 0;
            _nivelCombustivel = capacidadeDeposito;
        }

        // Método para abastecer o veículo.
        public void Abastecer(double litros)
        {
            if (litros <= 0 || double.IsNaN(litros) || double.IsInfinity(litros))
            {
                Console.WriteLine("Erro: Quantidade de combustível inválida.");
                return;
            }

            if (NivelCombustivel + litros > CapacidadeDeposito)
            {
                Console.WriteLine("Aviso: Capacidade máxima ultrapassada.");
                NivelCombustivel = CapacidadeDeposito;
            }
            else
            {
                NivelCombustivel += litros;
            }

            Console.WriteLine("Abastecimento realizado com sucesso!");
            Console.WriteLine("Combustível atual: " + NivelCombustivel.ToString("F2") + " litros.");
        }

        // Método para realizar uma viagem.
        public bool RealizarViagem(int kmDistancia)
        {
            if (kmDistancia <= 0)
            {
                Console.WriteLine("Erro: Distância inválida.");
                return false;
            }

            // Consumo de 0,07 litros por quilómetro.
            double consumo = kmDistancia * 0.07;

            if (NivelCombustivel < consumo)
            {
                Console.WriteLine("Viagem recusada: Combustível insuficiente.");
                return false;
            }

            Quilometragem += kmDistancia;
            NivelCombustivel -= consumo;

            Console.WriteLine("Viagem realizada com sucesso!");
            Console.WriteLine("Distância: " + kmDistancia + " km.");
            Console.WriteLine("Combustível consumido: " + consumo.ToString("F2") + " litros.");

            return true;
        }
    }
}

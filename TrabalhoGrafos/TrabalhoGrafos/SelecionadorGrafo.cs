using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    public static class SelecionadorGrafo
    {
        public static double criterioProx = 0.5;

        /// <summary>
        /// Calcula a densidade de um grafo direcionado: A / (V * (V - 1)).
        /// </summary>
        private static double CalcularDensidade(int vertices, int arestas)
        {
            if (vertices <= 1)
                return 0;

            return (double)arestas / (vertices * (vertices - 1));
        }

        /// <summary>
        /// Escolhe a representação conforme o número de arestas e a densidade:
        /// - Se edges <= vertices: Matriz de Incidência.
        /// - Senão, compara densidade com Threshold: ListaAdjacencia ou MatrizAdjacencia.
        /// </summary>
        public static Representacao Choose(int vertices, int arestas)
        {
            if (arestas <= vertices)
                return Representacao.MatrizIncidencia;

            double densidade = CalcularDensidade(vertices, arestas);
            return densidade < criterioProx
                ? Representacao.ListaAdjacencia
                : Representacao.MatrizAdjacencia;
        }
    }

    public enum Representacao
    {
        ListaAdjacencia,
        MatrizAdjacencia,
        MatrizIncidencia
    }
}

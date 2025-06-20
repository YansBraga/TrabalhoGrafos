using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    public class Grafo
    {
        public int V { get; private set; }
        public List<Tuple<int, int>>[] Adj { get; private set; }

        public Grafo(int v)
        {
            V = v;
            Adj = new List<Tuple<int, int>>[V];
            for (int i = 0; i < V; i++)
            {
                Adj[i] = new List<Tuple<int, int>>();
            }
        }

        public Grafo(int v, int a)
        {
            TipoRepresentacao(v, a);
        }

        private void TipoRepresentacao(int v, int a)
        {
            
        }

        public void AdicionarAresta(int origem, int destino, int peso)
        {
            // Subtraímos 1 da origem porque os vértices no arquivo são 1-based (começam em 1),
            // mas os índices do nosso array em C# são 0-based (começam em 0).
            if (origem > 0 && origem <= V && destino > 0 && destino <= V)
            {
                Adj[origem - 1].Add(new Tuple<int, int>(destino, peso));
            }
        }

        public void ImprimirListaAdjacencia()
        {
            Console.WriteLine("\n--- Representação do Grafo (Lista de Adjacência) ---");
            for (int i = 0; i < V; i++)
            {
                Console.Write($"Vértice {i + 1}: -> ");
                foreach (var aresta in Adj[i])
                {
                    // aresta.Item1 é o destino, aresta.Item2 é o peso
                    Console.Write($"({aresta.Item1}, peso: {aresta.Item2}) ");
                }
                Console.WriteLine();
            }
        }

    }
}

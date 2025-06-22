using System;
using System.Collections.Generic;
using System.Text;

namespace TrabalhoGrafos
{
    public class GrafoMatrizAdjacencia : IGrafo
    {
        public int NumeroVertices { get; private set; }
        public int NumeroArestas { get; private set; }
        private int[,] _matrizAdjacencia;

        public GrafoMatrizAdjacencia(int numVertices)
        {
            NumeroVertices = numVertices;
            _matrizAdjacencia = new int[numVertices, numVertices];

            for (int i = 0; i < numVertices; i++)
            {
                for (int j = 0; j < numVertices; j++)
                {
                    _matrizAdjacencia[i, j] = -1; // Usando -1 para indicar que não há aresta
                }
            }
        }

        void IGrafo.AdicionarAresta(Vertice origem, Vertice destino, int peso)
        {
            if (origem.Id < 0 || origem.Id >= NumeroVertices || destino.Id < 0 || destino.Id >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Os índices dos vértices estão fora do intervalo.");
            }
            else if (_matrizAdjacencia[origem.Id, destino.Id] == -1) // Verifica se a aresta não existe
            {
                _matrizAdjacencia[origem.Id, destino.Id] = peso;
                NumeroArestas++;
            }
            else
            {
                throw new InvalidOperationException("Aresta já existente!");
            }
        }

        string IGrafo.Imprimir()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Número de Vértices: {NumeroVertices}, Número de Arestas: {NumeroArestas}");
            for (int i = 0; i < NumeroVertices; i++)
            {
                for (int j = 0; j < NumeroVertices; j++)
                {
                    sb.Append(_matrizAdjacencia[i, j] + " ");
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

        List<Vertice> IGrafo.VerticesAdjascentes(Vertice v)
        {
            List<Vertice> verticesAdjacentes = new List<Vertice>();

            for (int j = 0; j < NumeroVertices; j++) // Em um grafo direcionado, arestas entrantes e saintes sao vertices adjascentes
            {
                if (_matrizAdjacencia[v.Id, j] != -1 || _matrizAdjacencia[j, v.Id] != -1)
                {
                    Vertice ver = new Vertice(j);

                    if (_matrizAdjacencia[v.Id, j] != -1) //Arestas saintes
                    {
                        ver.AdicionarArestaSaindo(new Vertice(j), _matrizAdjacencia[v.Id, j]);
                    }

                    if (_matrizAdjacencia[j, v.Id] != -1) // Arestas entrantes
                    {
                        ver.AdicionarArestaEntrando(new Vertice(j), _matrizAdjacencia[v.Id, j]);
                    }

                    verticesAdjacentes.Add(ver);
                }
            }

            return verticesAdjacentes;
        }

        Vertice IGrafo.LocalizarVertice(int v)
        {
            if (v < 0 || v >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("O índice do vértice está fora do intervalo.");
            }
            return new Vertice(v);
        }
    }
}
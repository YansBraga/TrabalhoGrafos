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
            if (_matrizAdjacencia[origem.Id, destino.Id] == -1) // Verifica se a aresta não existe
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

        public override string ToString()
        {
            return "Matriz de Adjacência";
        }

        List<Aresta> IGrafo.ArestasIncidentes(Vertice v)
        {
            List<Aresta> arestas = new List<Aresta>();

            if (v.Id < 0 || v.Id >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("O índice do vértice está fora do intervalo.");
            }

            for (int i = 0; i < NumeroVertices; i++)
            {
                if (_matrizAdjacencia[i, v.Id] != -1) // Aresta incidindo em v
                {
                    arestas.Add(new Aresta(new Vertice(i), v, _matrizAdjacencia[i, v.Id]));
                }
            }

            return arestas;
        }

        List<Aresta> IGrafo.ArestasAdjascentes(Vertice v1, Vertice v2)
        {
            List<Aresta> arestas = new List<Aresta>();
            List<Aresta> aux = new List<Aresta>();

            arestas = ArestasIncidentes(v1);
            aux = ArestasIncidentes(v2);

            foreach (Aresta a in aux)
            {
                arestas.Add(a);
            }

            return arestas;
        }

        private List<Aresta> ArestasIncidentes(Vertice v1)
        {
            throw new NotImplementedException();
        }

        List<Vertice> IGrafo.VerticesIncidentes(Aresta aresta)
        {
            List<Vertice> vertices = new List<Vertice>();

            if (aresta.Origem.Id < 0 || aresta.Origem.Id >= NumeroVertices ||
                aresta.Destino.Id < 0 || aresta.Destino.Id >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Os índices dos vértices da aresta estão fora do intervalo.");
            }

            vertices.Add(aresta.Origem);
            vertices.Add(aresta.Destino);

            return vertices;
        }
        Aresta IGrafo.LocalizarAresta(Vertice origem, Vertice destino)
        {
            if (origem.Id < 0 || origem.Id >= NumeroVertices || destino.Id < 0 || destino.Id >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Um ou ambos os índices dos vértices estão fora do intervalo.");
            }

            if (_matrizAdjacencia[origem.Id, destino.Id] != -1)
            {
                return new Aresta(origem, destino, _matrizAdjacencia[origem.Id, destino.Id]);
            }

            throw new InvalidOperationException("A aresta não existe.");
        }

        int IGrafo.GrauVertice(Vertice v)
        {
            if (v.Id < 0 || v.Id >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("O índice do vértice está fora do intervalo.");
            }

            int grau = 0;

            for (int j = 0; j < NumeroVertices; j++)
            {
                if (_matrizAdjacencia[v.Id, j] != -1)
                {
                    grau++;
                }
            }

            // Grau de entrada
            for (int i = 0; i < NumeroVertices; i++)
            {
                if (_matrizAdjacencia[i, v.Id] != -1)
                {
                    grau++;
                }
            }

            return grau;
        }
        bool IGrafo.IsAdjascente(Vertice v1, Vertice v2)
        {
            if (v1.Id < 0 || v1.Id >= NumeroVertices ||
                v2.Id < 0 || v2.Id >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Um ou ambos os índices dos vértices estão fora do intervalo.");
            }

            return _matrizAdjacencia[v1.Id, v2.Id] != -1; // Retorna true se são adjacentes
        }

        public void SubstituirPeso(Aresta aresta, int peso)
        {
            throw new NotImplementedException();
        }

        public bool TrocarVertices(Vertice v1, Vertice v2)
        {
            throw new NotImplementedException();
        }

        //-----------------------------------Algoritmos-----------------------------------//
        public string Dijkstra(Vertice origem, Vertice destino)
        {
            int[] distancias = new int[NumeroVertices];
            Vertice[] predecessores = new Vertice[NumeroVertices];
            bool[] visitados = new bool[NumeroVertices];

            for (int i = 0; i < NumeroVertices; i++)
            {
                distancias[i] = int.MaxValue;
                predecessores[i] = null;
                visitados[i] = false;
            }

            distancias[origem.Id] = 0;

            PriorityQueue<int, int> fila = new PriorityQueue<int, int>();
            fila.Enqueue(origem.Id, 0);

            while (fila.Count > 0)
            {
                int u = fila.Dequeue();

                if (visitados[u] == false)
                {
                    visitados[u] = true;

                    for (int v = 0; v < NumeroVertices; v++)
                    {
                        int peso = _matrizAdjacencia[u, v];

                        if (peso != -1)
                        {
                            int novaDistancia = distancias[u] + peso;

                            if (novaDistancia < distancias[v])
                            {
                                distancias[v] = novaDistancia;
                                predecessores[v] = new Vertice(u);
                                fila.Enqueue(v, novaDistancia);
                            }
                        }
                    }
                }
            }

            List<Vertice> caminho = new List<Vertice>();
            Vertice atual = destino;

            while (atual != null)
            {
                caminho.Insert(0, atual);
                atual = predecessores[atual.Id];
            }

            if (distancias[destino.Id] == int.MaxValue)
            {
                return $"Não existe caminho entre {origem.Id} e {destino.Id}.";
            }

            StringBuilder resultado = new StringBuilder();
            resultado.AppendLine($"Caminho mínimo de {origem.Id} para {destino.Id}:");

            for (int i = 0; i < caminho.Count - 1; i++)
            {
                int peso = _matrizAdjacencia[caminho[i].Id, caminho[i + 1].Id];
                resultado.Append($"{caminho[i].Id} ({peso}) ");
            }

            resultado.Append($"{caminho[caminho.Count - 1].Id}");
            resultado.AppendLine($"\nDistância total: {distancias[destino.Id]}");

            return resultado.ToString();
        }

        public string FloydWarshall()
        {
            int quantidadeVertices = NumeroVertices;
            int[,] distancias = new int[quantidadeVertices, quantidadeVertices];

            for (int i = 0; i < quantidadeVertices; i++)
            {
                for (int j = 0; j < quantidadeVertices; j++)
                {
                    if (i == j)
                    {
                        distancias[i, j] = 0;
                    }
                    else if (_matrizAdjacencia[i, j] != -1)
                    {
                        distancias[i, j] = _matrizAdjacencia[i, j];
                    }
                    else
                    {
                        distancias[i, j] = int.MaxValue;
                    }
                }
            }

            for (int k = 0; k < quantidadeVertices; k++)
            {
                for (int i = 0; i < quantidadeVertices; i++)
                {
                    for (int j = 0; j < quantidadeVertices; j++)
                    {
                        if (distancias[i, k] != int.MaxValue && distancias[k, j] != int.MaxValue)
                        {
                            int novaDistancia = distancias[i, k] + distancias[k, j];

                            if (novaDistancia < distancias[i, j])
                            {
                                distancias[i, j] = novaDistancia;
                            }
                        }
                    }
                }
            }

            StringBuilder resultado = new StringBuilder();
            resultado.AppendLine("Matriz de distâncias mínimas entre todos os pares de vértices:");

            for (int i = 0; i < quantidadeVertices; i++)
            {
                for (int j = 0; j < quantidadeVertices; j++)
                {
                    if (distancias[i, j] == int.MaxValue)
                    {
                        resultado.Append("INF ");
                    }
                    else
                    {
                        resultado.Append(distancias[i, j]).Append(" ");
                    }
                }
                resultado.AppendLine();
            }

            return resultado.ToString();
        }
    }
}
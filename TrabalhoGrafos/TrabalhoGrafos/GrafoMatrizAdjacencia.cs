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
            _matrizAdjacencia = new int[numVertices + 1, numVertices + 1];

            for (int i = 0; i <= numVertices; i++)
            {
                for (int j = 0; j <= numVertices; j++)
                {
                    _matrizAdjacencia[i, j] = -1; // Usando -1 para indicar que não há aresta
                }
            }
        }

        public void AdicionarAresta(Vertice origem, Vertice destino, int peso)
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

        public string Imprimir()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Número de Vértices: {NumeroVertices}, Número de Arestas: {NumeroArestas}");
            for (int i = 1; i <= NumeroVertices; i++)
            {
                for (int j = 1; j <= NumeroVertices; j++)
                    sb.Append(_matrizAdjacencia[i, j]).Append(' ');
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public List<Vertice> VerticesAdjascentes(Vertice v)
        {
            List<Vertice> verticesAdjacentes = new List<Vertice>();

            for (int j = 1; j <= NumeroVertices; j++) // Em um grafo direcionado, arestas entrantes e saintes sao vertices adjascentes
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

        public Vertice LocalizarVertice(int v)
        {
            if (v < 1 || v > NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("O índice do vértice está fora do intervalo.");
            }
            return new Vertice(v);
        }

        public override string ToString()
        {
            return "Matriz de Adjacência";
        }

        public List<Aresta> ArestasIncidentes(Vertice v)
        {
            if (v.Id < 1 || v.Id > NumeroVertices)
                throw new ArgumentOutOfRangeException("O índice do vértice está fora do intervalo.");

            var arestas = new List<Aresta>();

            // 1) Entrantes: coluna fixa = v.Id
            for (int i = 1; i <= NumeroVertices; i++)
            {
                int peso = _matrizAdjacencia[i, v.Id];
                if (peso != -1)
                    arestas.Add(new Aresta(new Vertice(i), v, peso));
            }

            // 2) Saídas: linha fixa = v.Id
            for (int j = 1; j <= NumeroVertices; j++)
            {
                int peso = _matrizAdjacencia[v.Id, j];
                if (peso != -1)
                    arestas.Add(new Aresta(v, new Vertice(j), peso));
            }

            return arestas;
        }

        public List<Aresta> ArestasAdjascentes(Vertice v1, Vertice v2)
        {
            var lista = new List<Aresta>();
            if (_matrizAdjacencia[v1.Id, v2.Id] != -1)
                lista.Add(new Aresta(v1, v2, _matrizAdjacencia[v1.Id, v2.Id]));
            if (_matrizAdjacencia[v2.Id, v1.Id] != -1)
                lista.Add(new Aresta(v2, v1, _matrizAdjacencia[v2.Id, v1.Id]));
            return lista;
        }

        public List<Vertice> VerticesIncidentes(Aresta aresta)
        {
            List<Vertice> vertices = new List<Vertice>();

            if (aresta.Origem.Id < 1 || aresta.Origem.Id > NumeroVertices ||
                aresta.Destino.Id < 1 || aresta.Destino.Id > NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Os índices dos vértices da aresta estão fora do intervalo.");
            }

            vertices.Add(aresta.Origem);
            vertices.Add(aresta.Destino);

            return vertices;
        }
        public Aresta LocalizarAresta(Vertice origem, Vertice destino)
        {
            if (origem.Id < 1 || origem.Id > NumeroVertices || destino.Id < 1 || destino.Id > NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Um ou ambos os índices dos vértices estão fora do intervalo.");
            }

            if (_matrizAdjacencia[origem.Id, destino.Id] != -1)
            {
                return new Aresta(origem, destino, _matrizAdjacencia[origem.Id, destino.Id]);
            }

            throw new InvalidOperationException("A aresta não existe.");
        }

        public int GrauVertice(Vertice v)
        {
            if (v.Id < 1 || v.Id > NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("O índice do vértice está fora do intervalo.");
            }

            int grau = 0;

            for (int j = 1; j <= NumeroVertices; j++)
            {
                if (_matrizAdjacencia[v.Id, j] != -1)
                {
                    grau++;
                }
            }

            // Grau de entrada
            for (int i = 1; i <= NumeroVertices; i++)
            {
                if (_matrizAdjacencia[i, v.Id] != -1)
                {
                    grau++;
                }
            }

            return grau;
        }
        public bool IsAdjascente(Vertice v1, Vertice v2)
        {
            if (v1.Id < 1 || v1.Id > NumeroVertices ||
                v2.Id < 1 || v2.Id > NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Um ou ambos os índices dos vértices estão fora do intervalo.");
            }

            return _matrizAdjacencia[v1.Id, v2.Id] != -1; // Retorna true se são adjacentes
        }

        public void SubstituirPeso(Aresta aresta, int peso)
        {
            if (aresta.Origem.Id < 1 || aresta.Origem.Id > NumeroVertices ||
            aresta.Destino.Id < 1 || aresta.Destino.Id > NumeroVertices)
            throw new ArgumentOutOfRangeException("Não foi possível adicionar pois excede o tamanho da matriz");

            _matrizAdjacencia[aresta.Origem.Id, aresta.Destino.Id] = peso;
        }

        public bool TrocarVertices(Vertice v1, Vertice v2)
        {
            if (v1.Id < 1 || v1.Id > NumeroVertices ||
                v2.Id < 1 || v2.Id > NumeroVertices)
                throw new ArgumentOutOfRangeException("Um ou ambos os índices dos vértices estão fora do intervalo.");

            int i = v1.Id;
            int j = v2.Id;

            for (int col = 1; col <= NumeroVertices; col++)
            {
                int tmp = _matrizAdjacencia[i, col];
                _matrizAdjacencia[i, col] = _matrizAdjacencia[j, col];
                _matrizAdjacencia[j, col] = tmp;
            }

            for (int row = 1; row <= NumeroVertices; row++)
            {
                int tmp = _matrizAdjacencia[row, i];
                _matrizAdjacencia[row, i] = _matrizAdjacencia[row, j];
                _matrizAdjacencia[row, j] = tmp;
            }

            return true;
        }


        //-----------------------------------Algoritmos-----------------------------------//
        public string Dijkstra(Vertice origem, Vertice destino)
        {

            int[] distancias = new int[NumeroVertices + 1];
            Vertice[] predecessores = new Vertice[NumeroVertices + 1];
            bool[] visitados = new bool[NumeroVertices + 1];


            for (int i = 1; i <= NumeroVertices; i++)
            {
                distancias[i] = int.MaxValue;
                predecessores[i] = null;
                visitados[i] = false;
            }

            distancias[origem.Id] = 0;


            var fila = new PriorityQueue<int, int>();
            fila.Enqueue(origem.Id, 0);

            while (fila.Count > 0)
            {
                int u = fila.Dequeue();
                if (!visitados[u])
                {
                    visitados[u] = true;
                    
                    for (int v = 1; v <= NumeroVertices; v++)
                    {
                        int peso = _matrizAdjacencia[u, v];
                        if (peso != -1)
                        {
                            int novaDist = distancias[u] + peso;
                            if (novaDist < distancias[v])
                            {
                                distancias[v] = novaDist;
                                predecessores[v] = new Vertice(u);
                                fila.Enqueue(v, novaDist);
                            }
                        }
                    }
                }
            }
            
            var caminho = new List<Vertice>();
            for (Vertice atual = destino; atual != null; atual = predecessores[atual.Id])
                caminho.Insert(0, atual);

            if (distancias[destino.Id] == int.MaxValue)
                return $"Não existe caminho entre {origem.Id} e {destino.Id}.";

            // 6) Monta a string de saída
            var sb = new StringBuilder();
            sb.AppendLine($"Caminho mínimo de {origem.Id} para {destino.Id}:");
            for (int i = 0; i < caminho.Count - 1; i++)
            {
                int id1 = caminho[i].Id, id2 = caminho[i + 1].Id;
                int peso = _matrizAdjacencia[id1, id2];
                sb.Append($"{id1} ({peso}) ");
            }
            sb.AppendLine($"{caminho[^1].Id}")
              .AppendLine($"Distância total: {distancias[destino.Id]}");

            return sb.ToString();
        }


        public string FloydWarshall()
        {
            int n = NumeroVertices;
            int[,] dist = new int[n + 1, n + 1];

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (i == j)
                        dist[i, j] = 0;
                    else if (_matrizAdjacencia[i, j] != -1)
                        dist[i, j] = _matrizAdjacencia[i, j];
                    else
                        dist[i, j] = int.MaxValue;
                }
            }

            for (int k = 1; k <= n; k++)
            {
                for (int i = 1; i <= n; i++)
                {
                    if (dist[i, k] == int.MaxValue) continue;
                    for (int j = 1; j <= n; j++)
                    {
                        if (dist[k, j] == int.MaxValue) continue;
                        int viaK = dist[i, k] + dist[k, j];
                        if (viaK < dist[i, j])
                            dist[i, j] = viaK;
                    }
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine("Matriz de distâncias mínimas entre todos os pares de vértices:");
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (dist[i, j] == int.MaxValue)
                        sb.Append("INF ");
                    else
                        sb.Append(dist[i, j]).Append(' ');
                }
                sb.AppendLine();
            }

            return sb.ToString();
        }

    }
}
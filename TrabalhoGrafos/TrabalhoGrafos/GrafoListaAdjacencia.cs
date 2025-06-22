using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    public class GrafoListaAdjacencia : IGrafo
    {
        private Dictionary<string, Vertice> _vertices;
        public int NumeroVertices { get; }
        public int NumeroArestas { get; private set; }

        /// <summary>
        /// Inicializa um grafo com os rótulos de vértices fornecidos.
        /// </summary>
        public GrafoListaAdjacencia(List<Vertice> vertices, List<Aresta> arestas)
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            _vertices = vertices.ToDictionary(v => v.Nome, v => v);

            NumeroArestas = 0;
            NumeroVertices = vertices.Count;

            foreach (Aresta item in arestas)
            {
                AdicionarAresta(item.Origem, item.Destino, item.Peso);
            }
        }

        /// <summary>
        /// Adiciona uma aresta direcionada de <paramref name="origem"/> para <paramref name="destino"/> com <paramref name="peso"/>.
        /// </summary>
        public void AdicionarAresta(Vertice origem, Vertice destino, int peso)
        {
            bool added = LocalizarVertice(origem.Nome)?.AdicionarArestaSaindo(destino, peso) ?? false;
            if (added)
                NumeroArestas++;
            else
                Console.WriteLine($"Aresta de {origem} para {destino} já existe.");
        }

        /// <summary>
        /// Imprime a lista de adjacência do grafo em texto.
        /// </summary>
        public string Imprimir()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Vertice v in _vertices.Values)
            {
                sb.Append(v.Nome).Append(": ");
                sb.AppendLine(string.Join(", ", v.ArestasSaindo.Select(a => a.ToString())));
            }
            return sb.ToString();
        }

        List<Vertice> IGrafo.VerticesAdjascentes(Vertice v)
        {
            Vertice verticeLocalizado = LocalizarVertice(v.Nome);

            if (verticeLocalizado != null)
            {
                return verticeLocalizado.ArestasSaindo.Select(a => a.Destino).ToList();
            }

            return new List<Vertice>();
        }


        /// <summary>
        /// Retorna o vértice correspondente ao rótulo fornecido, ou null se não existir.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vertice LocalizarVertice(string v)
        {
            Vertice vertice = null;

            _vertices.TryGetValue(v, out vertice);

            return vertice;
        }

        Aresta IGrafo.LocalizarAresta(Vertice origem, Vertice destino)
        {
            var v = LocalizarVertice(origem.Nome);
            return v?.ArestasSaindo.FirstOrDefault(a => a.Destino.Nome == destino.Nome);
        }

        public override string ToString()
        {
            return "Lista de Adjacência";
        }

        List<Aresta> IGrafo.ArestasAdjascentes(Vertice v1, Vertice v2)
        {
            List<Aresta> arestas = new List<Aresta>();

            arestas.AddRange(LocalizarVertice(v1.Nome)?.ArestasSaindo);
            arestas.AddRange(LocalizarVertice(v2.Nome)?.ArestasSaindo);

            return arestas;
        }

        List<Aresta> IGrafo.ArestasIncidentes(Vertice v)
        {
            Vertice verticeLocalizado = null;
            List<Aresta> arestasIncidentes = new List<Aresta>();

            _vertices.TryGetValue(v.Nome, out verticeLocalizado);

            if (verticeLocalizado != null)
            {
                arestasIncidentes.AddRange(verticeLocalizado.ArestasEntrantes.Concat(verticeLocalizado.ArestasSaindo).ToList());
                arestasIncidentes.AddRange(verticeLocalizado.ArestasSaindo.Concat(verticeLocalizado.ArestasSaindo).ToList());
            }

            return arestasIncidentes;
        }

        List<Vertice> IGrafo.VerticesIncidentes(Aresta aresta)
        {
            List<Vertice> verticesIncidentes = new List<Vertice>();        

            Vertice origem = LocalizarVertice(aresta.Origem.Nome);
            Vertice destino = LocalizarVertice(aresta.Destino.Nome);

            if (origem != null && destino != null)
            {
                verticesIncidentes.Add(origem);
                verticesIncidentes.Add(destino);
            }

            return verticesIncidentes;
        }

        int IGrafo.GrauVertice(Vertice v)
        {
            Vertice verticeLocalizado = LocalizarVertice(v.Nome);
            int grau = verticeLocalizado.ArestasSaindo.Count + verticeLocalizado.ArestasEntrantes.Count;
            return grau;
        }

        bool IGrafo.IsAdjascente(Vertice v1, Vertice v2)
        {
            Vertice vertice1 = LocalizarVertice(v1.Nome);
            Vertice vertice2 = LocalizarVertice(v2.Nome);

            bool isAdjascente1 = vertice1?.ArestasSaindo.Any(a => a.Destino.Nome == vertice2.Nome) ?? false;
            bool isAdjascente2 = vertice2?.ArestasSaindo.Any(a => a.Destino.Nome == vertice1.Nome) ?? false;

            return isAdjascente1 || isAdjascente2;
        }

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

                    Vertice verticeAtual = LocalizarVertice(u);

                    if (verticeAtual != null)
                    {
                        List<Aresta> arestas = verticeAtual.ArestasSaindo;

                        for (int i = 0; i < arestas.Count; i++)
                        {
                            Aresta aresta = arestas[i];
                            int v = aresta.Destino.Id;
                            int peso = aresta.Peso;

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
                return $"Não existe caminho entre {origem.Nome} e {destino.Nome}.";
            }

            StringBuilder resultado = new StringBuilder();
            resultado.AppendLine($"Caminho mínimo de {origem.Nome} para {destino.Nome}:");

            for (int i = 0; i < caminho.Count - 1; i++)
            {
                Vertice origemAresta = caminho[i];
                Vertice destinoAresta = caminho[i + 1];

                Aresta aresta = LocalizarAresta(origemAresta, destinoAresta);
                int peso = aresta != null ? aresta.Peso : 0;

                resultado.Append($"{origemAresta.Nome} ({peso}) ");
            }

            resultado.Append($"{caminho[caminho.Count - 1].Nome}");
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
                    else
                    {
                        distancias[i, j] = int.MaxValue;
                    }
                }
            }

            for (int i = 0; i < _vertices.Count; i++)
            {
                Vertice verticeOrigem = _vertices[i];

                List<Aresta> arestasSaindo = verticeOrigem.ArestasSaindo;

                for (int j = 0; j < arestasSaindo.Count; j++)
                {
                    Aresta aresta = arestasSaindo[j];
                    int origemId = verticeOrigem.Id;
                    int destinoId = aresta.Destino.Id;

                    distancias[origemId, destinoId] = aresta.Peso;
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
        
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
        private Dictionary<int, Vertice> _vertices;
        public int NumeroVertices { get; }
        public int NumeroArestas { get; private set; }

        /// <summary>
        /// Inicializa um grafo com os rótulos de vértices fornecidos.
        /// </summary>
        public GrafoListaAdjacencia(List<Vertice> vertices, List<Aresta> arestas)
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            _vertices = vertices.ToDictionary(v => v.Id, v => v);

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
            bool added = LocalizarVertice(origem.Id)?.AdicionarArestaSaindo(destino, peso) ?? false;
                       
            if (added)
            {
                Vertice destinoGrafo = LocalizarVertice(destino.Id);
                destinoGrafo.AdicionarArestaEntrando(origem, peso);
                NumeroArestas++;
            }
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
                sb.Append(v.Id).Append(": ");
                var listaArestas = v.ArestasSaindo.Select(a => $"{v.Id} -> {a.Destino.Id} (Peso: {a.Peso})");
                sb.AppendLine(string.Join(", ", listaArestas));
            }
            return sb.ToString();

        }

        public List<Vertice> VerticesAdjascentes(Vertice v)
        {
            Vertice verticeLocalizado = LocalizarVertice(v.Id);

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
        public Vertice LocalizarVertice(int v)
        {
            Vertice vertice = null;

            _vertices.TryGetValue(v, out vertice);

            return vertice;
        }

        public Aresta LocalizarAresta(Vertice origem, Vertice destino)
        {
            var v = LocalizarVertice(origem.Id);
            return v?.ArestasSaindo.FirstOrDefault(a => a.Destino.Id == destino.Id);
        }

        public override string ToString()
        {
            return "Lista de Adjacência";
        }

        public List<Aresta> ArestasAdjascentes(Vertice v1, Vertice v2)
        {
            List<Aresta> arestas = new List<Aresta>();

            arestas.AddRange(LocalizarVertice(v1.Id)?.ArestasSaindo);
            arestas.AddRange(LocalizarVertice(v2.Id)?.ArestasSaindo);

            return arestas;
        }

        public List<Aresta> ArestasIncidentes(Vertice v)
        {
            Vertice verticeLocalizado = null;
            List<Aresta> arestasIncidentes = new List<Aresta>();

            _vertices.TryGetValue(v.Id, out verticeLocalizado);

            if (verticeLocalizado != null)
            {
                arestasIncidentes.AddRange(verticeLocalizado.ArestasEntrantes.Concat(verticeLocalizado.ArestasSaindo).ToList());
                arestasIncidentes.AddRange(verticeLocalizado.ArestasSaindo.Concat(verticeLocalizado.ArestasSaindo).ToList());

                arestasIncidentes = arestasIncidentes.Distinct().ToList();
            }

            return arestasIncidentes;
        }

        public List<Vertice> VerticesIncidentes(Aresta aresta)
        {
            List<Vertice> verticesIncidentes = new List<Vertice>();        

            Vertice origem = LocalizarVertice(aresta.Origem.Id);
            Vertice destino = LocalizarVertice(aresta.Destino.Id);

            if (origem != null && destino != null)
            {
                verticesIncidentes.Add(origem);
                verticesIncidentes.Add(destino);
            }

            return verticesIncidentes;
        }

        public int GrauVertice(Vertice v)
        {
            Vertice verticeLocalizado = LocalizarVertice(v.Id);
            int grau = verticeLocalizado.ArestasSaindo.Count + verticeLocalizado.ArestasEntrantes.Count;
            return grau;
        }

        public bool IsAdjascente(Vertice v1, Vertice v2)
        {
            Vertice vertice1 = LocalizarVertice(v1.Id);
            Vertice vertice2 = LocalizarVertice(v2.Id);

            bool isAdjascente1 = vertice1?.ArestasSaindo.Any(a => a.Destino.Id == vertice2.Id) ?? false;
            bool isAdjascente2 = vertice2?.ArestasSaindo.Any(a => a.Destino.Id == vertice1.Id) ?? false;

            return isAdjascente1 || isAdjascente2;
        }

        public void SubstituirPeso(Aresta aresta, int peso)
        {
            Aresta aresta1 = LocalizarAresta(aresta.Origem, aresta.Destino);
            aresta.Peso = peso;
        }

        public bool TrocarVertices(Vertice v1, Vertice v2)
        {
            
            var vert1 = LocalizarVertice(v1.Id);
            var vert2 = LocalizarVertice(v2.Id);
            if (vert1 == null || vert2 == null) return false;

            
            var tempSaida = vert1.ArestasSaindo;

            vert1.setArestasSaindo(vert2.ArestasSaindo);
            vert2.setArestasSaindo(tempSaida);

            
            foreach (var u in _vertices.Values)
            {
                foreach (var a in u.ArestasSaindo)
                {
                    if (a.Destino == vert1) a.Destino = vert2;
                    else if (a.Destino == vert2) a.Destino = vert1;
                }
            }

            
            foreach (var u in _vertices.Values)
                u.ArestasEntrantes.Clear();

            foreach (var u in _vertices.Values)
                foreach (var a in u.ArestasSaindo)
                    a.Destino.ArestasEntrantes.Add(a);

            return true;
        }

        public string Dijkstra(Vertice origem, Vertice destino)
        {
            Dictionary<int, int> distancias = new Dictionary<int, int>();
            Dictionary<int, Vertice> predecessores = new Dictionary<int, Vertice>();
            HashSet<int> visitados = new HashSet<int>();

            foreach (Vertice vertice in _vertices.Values)
            {
                distancias[vertice.Id] = int.MaxValue;
                predecessores[vertice.Id] = null;
            }

            distancias[origem.Id] = 0;

            PriorityQueue<int, int> fila = new PriorityQueue<int, int>();
            fila.Enqueue(origem.Id, 0);

            while (fila.Count > 0)
            {
                int u = fila.Dequeue();

                if (!visitados.Contains(u))
                {
                    visitados.Add(u);

                    Vertice verticeAtual = LocalizarVertice(u);

                    if (verticeAtual != null)
                    {
                        foreach (Aresta aresta in verticeAtual.ArestasSaindo)
                        {
                            int v = aresta.Destino.Id;
                            int peso = aresta.Peso;

                            if (distancias[u] != int.MaxValue)
                            {
                                int novaDistancia = distancias[u] + peso;

                                if (novaDistancia < distancias[v])
                                {
                                    distancias[v] = novaDistancia;
                                    predecessores[v] = verticeAtual;
                                    fila.Enqueue(v, novaDistancia);
                                }
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
                Vertice origemAresta = caminho[i];
                Vertice destinoAresta = caminho[i + 1];

                Aresta aresta = LocalizarAresta(origemAresta, destinoAresta);
                int peso = aresta != null ? aresta.Peso : 0;

                resultado.Append($"{origemAresta.Id} ({peso}) ");
            }

            resultado.Append($"{caminho[caminho.Count - 1].Id}");
            resultado.AppendLine($"\nDistância total: {distancias[destino.Id]}");

            return resultado.ToString();
        }

        public string FloydWarshall()
        {
            int quantidadeVertices = NumeroVertices;

            Dictionary<int, int> mapaIdParaIndice = new Dictionary<int, int>();
            Dictionary<int, int> mapaIndiceParaId = new Dictionary<int, int>();

            int indice = 0;
            foreach (Vertice vertice in _vertices.Values)
            {
                mapaIdParaIndice[vertice.Id] = indice;
                mapaIndiceParaId[indice] = vertice.Id;
                indice++;
            }

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

            foreach (Vertice vertice in _vertices.Values)
            {
                int origemIndice = mapaIdParaIndice[vertice.Id];

                foreach (Aresta aresta in vertice.ArestasSaindo)
                {
                    int destinoIndice = mapaIdParaIndice[aresta.Destino.Id];
                    distancias[origemIndice, destinoIndice] = aresta.Peso;
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
                    int origemId = mapaIndiceParaId[i];
                    int destinoId = mapaIndiceParaId[j];

                    if (distancias[i, j] == int.MaxValue)
                    {
                        resultado.Append($"({origemId}->{destinoId}) INF\t");
                    }
                    else
                    {
                        resultado.Append($"({origemId}->{destinoId}) {distancias[i, j]}\t");
                    }
                }
                resultado.AppendLine();
            }

            return resultado.ToString();
        }

    }
}
        
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
                sb.Append(v.Id).Append(": ");
                sb.AppendLine(string.Join(", ", v.ArestasSaindo.Select(a => a.ToString())));
            }
            return sb.ToString();
        }

        List<Vertice> IGrafo.VerticesAdjascentes(Vertice v)
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

        Aresta IGrafo.LocalizarAresta(Vertice origem, Vertice destino)
        {
            var v = LocalizarVertice(origem.Id);
            return v?.ArestasSaindo.FirstOrDefault(a => a.Destino.Id == destino.Id);
        }

        public override string ToString()
        {
            return "Lista de Adjacência";
        }

        List<Aresta> IGrafo.ArestasAdjascentes(Vertice v1, Vertice v2)
        {
            List<Aresta> arestas = new List<Aresta>();

            arestas.AddRange(LocalizarVertice(v1.Id)?.ArestasSaindo);
            arestas.AddRange(LocalizarVertice(v2.Id)?.ArestasSaindo);

            return arestas;
        }

        List<Aresta> IGrafo.ArestasIncidentes(Vertice v)
        {
            Vertice verticeLocalizado = null;
            List<Aresta> arestasIncidentes = new List<Aresta>();

            _vertices.TryGetValue(v.Id, out verticeLocalizado);

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

            Vertice origem = LocalizarVertice(aresta.Origem.Id);
            Vertice destino = LocalizarVertice(aresta.Destino.Id);

            if (origem != null && destino != null)
            {
                verticesIncidentes.Add(origem);
                verticesIncidentes.Add(destino);
            }

            return verticesIncidentes;
        }

        int IGrafo.GrauVertice(Vertice v)
        {
            Vertice verticeLocalizado = LocalizarVertice(v.Id);
            int grau = verticeLocalizado.ArestasSaindo.Count + verticeLocalizado.ArestasEntrantes.Count;
            return grau;
        }

        bool IGrafo.IsAdjascente(Vertice v1, Vertice v2)
        {
            Vertice vertice1 = LocalizarVertice(v1.Id);
            Vertice vertice2 = LocalizarVertice(v2.Id);

            bool isAdjascente1 = vertice1?.ArestasSaindo.Any(a => a.Destino.Id == vertice2.Id) ?? false;
            bool isAdjascente2 = vertice2?.ArestasSaindo.Any(a => a.Destino.Id == vertice1.Id) ?? false;

            return isAdjascente1 || isAdjascente2;
        }
    }
}
        
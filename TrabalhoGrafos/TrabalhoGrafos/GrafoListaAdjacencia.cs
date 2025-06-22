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
        private List<Vertice> _vertices;        
        public int NumeroVertices { get; }
        public int NumeroArestas { get; private set; }

       /// <summary>
        /// Inicializa um grafo com os rótulos de vértices fornecidos.
        /// </summary>
        public GrafoListaAdjacencia(List<Vertice> vertices, List<Aresta> arestas)
        {
            if (vertices == null)
                throw new ArgumentNullException(nameof(vertices));

            _vertices = vertices;

            NumeroArestas = arestas.Count;
            NumeroVertices = vertices.Count;

            foreach (var item in arestas)
            {
                AdicionarAresta(item.Origem, item.Destino, item.Peso);
            }
        }

        /// <summary>
        /// Adiciona uma aresta direcionada de <paramref name="origem"/> para <paramref name="destino"/> com <paramref name="peso"/>.
        /// </summary>
        public void AdicionarAresta(Vertice origem, Vertice destino, int peso)
        {            
            bool added = _vertices.FirstOrDefault(v => v.Equals(origem))?.AdicionarArestaSaindo(destino, peso) ?? false;
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
            var sb = new System.Text.StringBuilder();
            foreach (var v in _vertices)
            {
                sb.Append(v.Nome).Append(": ");
                sb.AppendLine(string.Join(", ", v.ArestasSaindo.Select(a => a.ToString())));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Retorna todas as arestas saindo do vértice <paramref name="indiceVertice"/>.
        /// </summary>
        public List<Aresta> GetArestasSaindo(int indiceVertice)
        {            
            return _vertices[indiceVertice].ArestasSaindo.ToList();
        }

        void IGrafo.Imprimir()
        {
            throw new NotImplementedException();
        }

        List<Vertice> IGrafo.VerticesAdjascentes(Vertice v)
        {
            throw new NotImplementedException();
        }

        Vertice IGrafo.LocalizarVertice(int v)
        {
            throw new NotImplementedException();
        }

        void IGrafo.AdicionarAresta(int origem, int destino, int peso)
        {
            throw new NotImplementedException();
        }
    }
}
        
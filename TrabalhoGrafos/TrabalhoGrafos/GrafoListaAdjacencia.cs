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
        public int NumeroVertices { get; }
        public int NumeroArestas { get; }

        public GrafoListaAdjacencia(int numVertices)
        {
            _vertices = new List<Vertice>();
            for (int i = 0; i < numVertices; i++)
            {
                _vertices.Add(new Vertice("V" + i));
            }
        }

        public void AdicionarAresta(int origem, int destino, int peso)
        {
            if (origem < 0 || origem >= NumeroVertices || destino < 0 || destino >= NumeroVertices)
            {
                throw new ArgumentOutOfRangeException("Os índices de origem ou destino estão fora do intervalo.");
            }
            if (!_vertices[origem].AdicionarArestaSaindo(_vertices[destino], peso))
            {
                Console.WriteLine("Aresta já existe entre os vértices.");
            }
            else
            {
                NumeroArestas++;
            }
        }

        public string Imprimir()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var vertice in _vertices)
            {
                sb.AppendLine(vertice.ToString());
            }
            return sb.ToString();
        }

        public List<Aresta> GetArestasSaindo(int numVertice)
    {
        return _vertices[numVertice].ArestasSaindo.ToList();
    }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    public interface IGrafo
    {
        int NumeroVertices { get; }
        int NumeroArestas { get; }
        void AdicionarAresta(Vertice origem, Vertice destino, int peso);
        string Imprimir();
        List<Vertice> VerticesAdjascentes(Vertice v);
        List<Aresta> ArestasAdjascentes(Vertice v1, Vertice v2);
        List<Aresta> ArestasIncidentes(Vertice v);
        List<Vertice> VerticesIncidentes(Aresta aresta);
        Vertice LocalizarVertice(string v);
        Aresta LocalizarAresta(Vertice origem, Vertice destino);        

        int GrauVertice(Vertice v);
        bool IsAdjascente(Vertice v1, Vertice v2);

        string Dijkstra(Vertice origem, Vertice destino);

        public string FloydWarshall();

    }
}

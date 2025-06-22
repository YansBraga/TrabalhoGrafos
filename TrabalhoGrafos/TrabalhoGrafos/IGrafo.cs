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
        Vertice LocalizarVertice(int v);
    }
}

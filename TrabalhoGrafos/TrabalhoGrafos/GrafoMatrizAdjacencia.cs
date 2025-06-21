using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    public class GrafoMatrizAdjacencia : IGrafo
    {
        public int NumeroVertices { get; }
        public int NumeroArestas { get; }

        public GrafoMatrizAdjacencia(int numVertices)
        {

        }

        void IGrafo.AdicionarAresta(int origem, int destino, int peso)
        {
            throw new NotImplementedException();
        }

        void IGrafo.Imprimir()
        {
            throw new NotImplementedException();
        }
    }
}

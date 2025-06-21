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

        public GrafoListaAdjacencia(int numVertices)
        {            
            //lista = new List<(int, int)>[numVertices];

            // Inicializa uma lista vazia para cada vértice.
            for (int i = 0; i < numVertices; i++)
            {
              //  lista[i] = new List<(int, int)>();
            }
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

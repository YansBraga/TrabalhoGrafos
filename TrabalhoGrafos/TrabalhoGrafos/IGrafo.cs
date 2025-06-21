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
        void AdicionarAresta(int origem, int destino, int peso);
        void Imprimir();
    }
}

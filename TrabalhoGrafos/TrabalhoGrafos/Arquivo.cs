using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrabalhoGrafos
{
    public static class Arquivo
    {   
        public static IGrafo ImportarArquivo()
        {
            try
            {
                List<Aresta> arestas = new List<Aresta>();
                List<Vertice> vertices = new List<Vertice>();

                // Obtém o diretório do projeto, assumindo que o arquivo está na raiz do projeto
                var projetoDir = Directory.GetParent(AppContext.BaseDirectory)      // ...\bin\Debug\net8.0
                         .Parent // ...\bin\Debug
                         .Parent // ...\bin
                         .Parent // ...\<pasta do projeto>
                         .FullName;

                string path = Path.Combine(projetoDir, "grafo.dimacs");
                string[] linhas = File.ReadAllLines(path);                
                
                string[] primeiraLinha = linhas[0].Split(' ');
                int numVertices = int.Parse(primeiraLinha[0]);
                int numArestas = int.Parse(primeiraLinha[1]);


                // Processa as linhas das arestas
                for (int i = 1; i < linhas.Length; i++)
                {
                    
                    string[] dadosAresta = linhas[i].Split(' ');
                    int origem = int.Parse(dadosAresta[0]);
                    int destino = int.Parse(dadosAresta[1]);
                    int peso = int.Parse(dadosAresta[2]);                    

                    arestas.Add(new Aresta(new Vertice(origem), new Vertice(destino), peso));
                }

                vertices = arestas
                    .Select(a => a.Origem)
                    .Concat(arestas.Select(a => a.Destino))
                    .GroupBy(v => v.Id)
                    .Select(g => g.First())
                    .ToList();

                Representacao representacao = SelecionadorTipoGrafo.Choose(numVertices, numArestas);                

                IGrafo grafo = representacao switch
                {
                    Representacao.ListaAdjacencia => new GrafoListaAdjacencia(vertices, arestas),
                    Representacao.MatrizAdjacencia => CriarMatrizAdjacencia(vertices.Count, arestas),
                    _ => throw new NotSupportedException("Representação de grafo não suportada.")
                };

                return grafo;
            }
            catch (FileNotFoundException)
            {                
                throw new Exception("ERRO: O arquivo não foi encontrado. Verifique o caminho e tente novamente.");
            }
            catch (Exception ex)
            {
                throw new Exception($"ERRO inesperado ao ler o arquivo: {ex.Message}");
            }
        }

        public static IGrafo CriarMatrizAdjacencia(int numVertices, List<Aresta> arestas)
        {
            var m = new GrafoMatrizAdjacencia(numVertices);
            foreach (var a in arestas)
                m.AdicionarAresta(a.Origem, a.Destino, a.Peso);
            return m;
        }
    }
}

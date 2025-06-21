using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrabalhoGrafos
{
    public class Arquivo
    {        
        private string caminho { get; set; }
        //public Grafo grafo { get; set; }

        
        public Arquivo(string caminho)
        {            
            //grafo = ImportarArquivo();
            this.caminho = caminho;
        }

        private string ImportarArquivo()
        {
            try
            {
                // Lê todas as linhas do arquivo de uma vez
                string[] linhas = File.ReadAllLines(caminho);
                
                string[] primeiraLinha = linhas[0].Split(' ');
                int numVertices = int.Parse(primeiraLinha[0]);

                // Cria o objeto Grafo com o número de vértices encontrado
                //Grafo grafo = new Grafo(numVertices);

                // Processa as linhas das arestas (pula a primeira linha, i=1)
                for (int i = 1; i < linhas.Length; i++)
                {                    
                    string[] dadosAresta = linhas[i].Split(' ');
                    int origem = int.Parse(dadosAresta[0]);
                    int destino = int.Parse(dadosAresta[1]);
                    int peso = int.Parse(dadosAresta[2]);

                    // Adiciona a aresta ao nosso objeto grafo
                    //grafo.AdicionarAresta(origem, destino, peso);
                }

                Console.WriteLine("Arquivo lido e grafo construído com sucesso!");
                return "";
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"ERRO: O arquivo não foi encontrado em '{caminho}'. Verifique o caminho e tente novamente.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO inesperado ao ler o arquivo: {ex.Message}");
                return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TrabalhoGrafos
{
    public static class Arquivo
    {   
        public static string ImportarArquivo()
        {
            try
            {
                // Obtém o diretório do projeto, assumindo que o arquivo está na raiz do projeto
                var projetoDir = Directory.GetParent(AppContext.BaseDirectory)      // ...\bin\Debug\net8.0
                         .Parent                                   // ...\bin\Debug
                         .Parent                                   // ...\bin
                         .Parent                                   // ...\<pasta do projeto>
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
                }

                Representacao representacao = SelecionadorTipoGrafo.Choose(numVertices, numArestas);

                return "Arquivo lido e grafo construído com sucesso!";
            }
            catch (FileNotFoundException)
            {                
                return "ERRO: O arquivo não foi encontrado. Verifique o caminho e tente novamente.";
            }
            catch (Exception ex)
            {                
                return $"ERRO inesperado ao ler o arquivo: {ex.Message}";
            }
        }
    }
}

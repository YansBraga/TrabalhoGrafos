﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    class Program
    {
        public static IGrafo grafo = null;

        public static void Main(string[] args)
        {
            bool sair = false;
            

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Yellow;

            while (!sair)
            {
                Console.Clear();

                if (grafo == null)
                {
                    ExibirMenuCriacao();
                    if (int.TryParse(Console.ReadLine(), out int opcaoCriacao))
                    {
                        switch (opcaoCriacao)
                        {
                            case 1:
                                Console.WriteLine("Importando grafo do arquivo DIMACS...");
                                grafo = Arquivo.ImportarArquivo();

                                if (grafo != null)
                                    Console.WriteLine($"Grafo importado com sucesso! \nRepresentação: {grafo.ToString()}");
                                else
                                    Console.WriteLine("Falha ao importar o grafo. Tente novamente.");

                                break;
                            case 2:
                                // 1) Lê quantos vértices e arestas
                                Console.Write("Insira a quantidade de vértices: ");
                                int numVertices = int.Parse(Console.ReadLine()!);
                                Console.Write("Insira a quantidade de arestas: ");
                                int numArestas = int.Parse(Console.ReadLine()!);

                                // 2) Lê cada aresta e monta a lista de arestas
                                var arestas_ = new List<Aresta>();
                                for (int i = 1; i <= numArestas; i++)
                                {
                                    Console.Write($"Aresta #{i} (origem destino peso): ");
                                    var partes = Console.ReadLine()!
                                        .Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                    int o = int.Parse(partes[0]);
                                    int d = int.Parse(partes[1]);
                                    int p = int.Parse(partes[2]);
                                    arestas_.Add(new Aresta(new Vertice(o), new Vertice(d), p));
                                }

                                // 3) Garante que temos a lista de todos os vértices (1..n)
                                var vertices_ = Enumerable
                                    .Range(1, numVertices)
                                    .Select(id => new Vertice(id))
                                    .ToList();

                                // 4) Escolhe a representação com base na densidade
                                var representacao = SelecionadorTipoGrafo.Choose(numVertices, numArestas);
                                grafo = representacao switch
                                {
                                    Representacao.ListaAdjacencia =>
                                        new GrafoListaAdjacencia(vertices_, arestas_),
                                    Representacao.MatrizAdjacencia =>
                                        // helper que cria e preenche a matriz:
                                        Arquivo.CriarMatrizAdjacencia(numVertices, arestas_),
                                    _ => throw new NotSupportedException("Representação não suportada.")
                                };

                                // 5) Exibe qual foi escolhida e imprime
                                Console.WriteLine($"\nUsando `{grafo}`\n");

                                if (grafo != null)
                                    Console.WriteLine($"Grafo salvo com sucesso! \nRepresentação: {grafo.ToString()}");
                                else
                                    Console.WriteLine("Falha ao salvar o grafo. Tente novamente.");

                                break;
                            case 3:


                                break;
                            default:
                                Console.WriteLine("Opção inválida. Tente novamente.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Por favor, insira um número.");
                    }
                }
                // Se o grafo JÁ EXISTE, mostra o menu de operações
                else
                {
                    ExibirMenuPrincipal();
                    if (int.TryParse(Console.ReadLine(), out int opcaoPrincipal))
                    {                        
                        switch (opcaoPrincipal)
                        {
                            case 1: //  Leitura e impressão de um grafo já pronto. Nesse caso, vocês deverão ler um grafo de entrada no formato DIMACS 1. O grafo a ser lido, deve ser representado em uma Lista de Adjacência, Matriz de Adjacência ou Matriz de Incidência.                                
                                Console.WriteLine(grafo.Imprimir());
                                break;

                            case 2: //  Imprimir todas as arestas adjacentes a uma aresta a, informada pelo usuário.
                                Aresta a = LerAresta();

                                if (a != null)
                                {
                                    Console.WriteLine($"Arestas adjacentes à aresta {a}:");
                                    grafo.ArestasAdjascentes(a.Origem, a.Destino).ForEach(v => Console.WriteLine(v));
                                }
                                else
                                    Console.WriteLine("Aresta não encontrada.");

                                break;

                            case 3: //  Imprimir todos os vértices adjacentes a um vértice v, informado pelo usuário.                    
                                Console.WriteLine($"Vértices adjascentes:");

                                List<Vertice> vertices_ = grafo.VerticesAdjascentes(LerVertice());

                                if (vertices_.Any())
                                    vertices_.ForEach(v => Console.WriteLine(v));
                                else
                                    Console.WriteLine("Não existem vértices adjascentes para este vértice");
                                break;

                            case 4: //  Imprimir todas as arestas incidentes a um vértice v, informado pelo usuário.
                                Console.WriteLine($"Arestas incidentes:");
                                grafo.ArestasIncidentes(LerVertice()).ForEach(a => Console.WriteLine(a));
                                break;

                            case 5: //  Imprimir todos os vértices incidentes a uma aresta a, informada pelo usuário.
                                Console.WriteLine($"Vértices incidentes:");
                                grafo.VerticesIncidentes(LerAresta()).ForEach(v => Console.WriteLine(v));
                                break;

                            case 6: //  Imprimir o grau do vértice v, informado pelo usuário.                                
                                Console.WriteLine($"Grau do vértice: {grafo.GrauVertice(LerVertice())}");
                                break;

                            case 7: //  Determinar se dois vértices são adjacentes
                                if (grafo.IsAdjascente(LerVertice(), LerVertice()))
                                    Console.WriteLine("Os vértices são adjacentes.");
                                else
                                    Console.WriteLine("Os vértices não são adjacentes.");
                                break;

                            case 8: //  Substituir o peso de uma aresta a, informada pelo usuário
                                Console.WriteLine("Insira o novo peso da aresta:");
                                if (int.TryParse(Console.ReadLine(), out int novoPeso))
                                {
                                    grafo.SubstituirPeso(LerAresta(), novoPeso);
                                    Console.WriteLine("Peso da aresta atualizado com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Entrada inválida. Por favor, insira um número.");
                                }
                                break;
                            case 9: //  Trocar dois vértices
                                Vertice v1 = LerVertice();
                                Vertice v2 = LerVertice();

                                if (grafo.TrocarVertices(v1, v2))
                                    Console.WriteLine($"Vértices {v1} e {v2} trocados com sucesso.");
                                else
                                    Console.WriteLine("Falha ao trocar os vértices. Verifique se ambos existem no grafo.");
                                break;

                            case 10: // Busca em grafos (Busca em Largura)
                                BuscaLargura(LerVertice());
                                break;

                            case 11: //  Busca em grafos (Busca em Profundidade)
                                BuscaProfundidade(LerVertice());
                                break;

                            case 12:
                                Console.WriteLine("Digite o nome do vértice de origem:");
                                Vertice verticeOrigemDijkstra = LerVertice();

                                if (verticeOrigemDijkstra != null)
                                {
                                    Console.WriteLine("Digite o nome do vértice de destino:");
                                    Vertice verticeDestinoDijkstra = LerVertice();

                                    if (verticeDestinoDijkstra != null)
                                    {
                                        string resultadoDijkstra = grafo.Dijkstra(verticeOrigemDijkstra, verticeDestinoDijkstra);
                                        Console.WriteLine(resultadoDijkstra);
                                    }
                                    else
                                    {
                                        Console.WriteLine("O vértice de destino não foi encontrado.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("O vértice de origem não foi encontrado.");
                                }

                                break;

                            case 13:
                                string resultadoFloyd = grafo.FloydWarshall();
                                Console.WriteLine(resultadoFloyd);
                                break;

                            case 16: // Opção para voltar ao menu anterior
                                grafo = null; // Descarta o grafo atual
                                Console.WriteLine("Retornando ao menu de criação de grafo.");
                                break;
                            case 0:
                                sair = true; // Define a flag para sair do loop
                                break;
                            default:
                                Console.WriteLine("Opção inválida. Tente novamente.");
                                break;
                        }

                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Entrada inválida. Por favor, insira um número.");
                    }
                }

            }

            Console.WriteLine("\nObrigado por usar o sistema. Programa encerrado.");
        }


        public static void ExibirMenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine($"--- Menu Principal (Grafo Carregado - {grafo.ToString()}) ---");
            Console.WriteLine("Escolha uma operação para realizar no grafo:");
            Console.WriteLine("1. Imprimir representação do grafo");
            Console.WriteLine("2. Imprimir todas as arestas adjacentes a uma aresta");
            Console.WriteLine("3. Imprimir todos os vértices adjacentes a um vértice");
            Console.WriteLine("4. Imprimir todas as arestas incidentes a um vértice");
            Console.WriteLine("5. Imprimir todos os vértices incidentes a uma aresta");
            Console.WriteLine("6. Imprimir o grau de um vértice");
            Console.WriteLine("7. Determinar se dois vértices são adjacentes");
            Console.WriteLine("8. Substituir o peso de uma aresta");
            Console.WriteLine("9. Trocar dois vértices");
            Console.WriteLine("10. Busca em Largura");
            Console.WriteLine("11. Busca em Profundidade");
            Console.WriteLine("12. Dijkstra");
            Console.WriteLine("13. Floyd Warshall");
            Console.WriteLine("16 - Descartar grafo e voltar");
            Console.WriteLine("0 - Sair do Programa");
            Console.Write("\nEscolha uma opção: ");
        }


        public static void ExibirMenuCriacao()
        {
            Console.WriteLine("--- Menu de Criação de Grafo ---");
            Console.WriteLine("Nenhum grafo carregado. Por favor, escolha uma opção:");
            Console.WriteLine("1 - Importar Grafo de arquivo DIMACS");
            Console.WriteLine("2 - Preencher manualmente");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha uma opção: ");
        }

        public static Vertice LerVertice()
        {
            Console.WriteLine("Qual a vertice desejado?");
            int verticeDesejado = int.Parse(Console.ReadLine());

            Vertice v = grafo.LocalizarVertice(verticeDesejado);

            if (v != null)
            {
                return v;
            }
            else
            {
                Console.WriteLine("Vértice não encontrado. Por favor, tente novamente.");
                return LerVertice();
            }
        }

        public static Aresta LerAresta()
        {
            Console.WriteLine("Insira os vértices referente a aresta escolhida?");
            Vertice vertice1 = LerVertice();
            Vertice vertice2 = LerVertice();

            Aresta a = grafo.LocalizarAresta(vertice1, vertice2);


            if (a != null)
            {
                return a;
            }
            else
            {
                Console.WriteLine("Aresta não encontrada. Por favor, tente novamente.");
                return LerAresta();
            }

        }

        public static void BuscaLargura(Vertice v)
        {
            int t = 0;
            Queue<Vertice> fila = new Queue<Vertice>();
            List<int> L = new List<int>();
            List<int> nivel = new List<int>();
            List<Vertice> pai = new List<Vertice>();

            for (int i = 0; i < grafo.NumeroVertices; i++)
            {
                L.Add(0);
                L[i] = 0;
                nivel.Add(0);
                nivel[i] = 0;
                pai.Add(null);
                pai[i] = null;
            }

            fila.Enqueue(v);
            t++;
            L[v.Id-1] = t;
            nivel[v.Id-1] = 0;

            while (fila.Count > 0)
            {
                Vertice atual = fila.Dequeue();

                try
                {
                    foreach (Vertice vert in grafo.VerticesAdjascentes(atual).OrderBy(adj => adj.Id-1))
                    {
                        if (L[vert.Id-1] == 0)
                        {
                            t++;
                            L[vert.Id-1] = t;
                            nivel[vert.Id-1] = nivel[atual.Id-1] + 1;
                            pai[vert.Id-1] = atual;
                            fila.Enqueue(vert);
                        }
                    }
                }
                catch (System.Exception)
                {
                    Console.WriteLine("Vertice " + atual + " nao localizado");
                }
            }

            Console.WriteLine("Resultado da Busca em Largura:");
            for (int i = 0; i < grafo.NumeroVertices; i++)
            {
                if (L[i] != 0)
                {
                    Console.WriteLine($"Vértice: {i+1}, Nível: {nivel[i]}, Pai: {(pai[i] != null ? pai[i].Id.ToString() : "null")}");
                }
                
            }
        }


        static int t = 0;
        public static void BuscaProfundidade(Vertice v)
        {
            t = 0;
            List<int> td = new List<int>(new int[grafo.NumeroVertices]);
            List<int> tt = new List<int>(new int[grafo.NumeroVertices]);
            List<Vertice> pai = new List<Vertice>(new Vertice[grafo.NumeroVertices]);

            for (int i = 0; i < grafo.NumeroVertices; i++)
            {
                td[i] = 0;
                tt[i] = 0;
                pai[i] = null;
            }

            BuscandoProfundidade(v, td, tt, pai);

            for (int i = 0; i < grafo.NumeroVertices; i++)
            {
                if (td[i] == 0)
                {
                    BuscandoProfundidade(v, td, tt, pai);
                }
            }
        }

        private static void BuscandoProfundidade(Vertice v, List<int> td, List<int> tt, List<Vertice> pai)
        {
            t++;
            td[v.Id] = t;

            foreach (Vertice w in grafo.VerticesAdjascentes(v).OrderBy(adj => adj.Id))
            {
                if (td[w.Id] == 0)
                {
                    pai[w.Id] = v;
                    BuscandoProfundidade(w, td, tt, pai);
                }
            }

            t++;
            tt[v.Id] = t;
        }
    }
}

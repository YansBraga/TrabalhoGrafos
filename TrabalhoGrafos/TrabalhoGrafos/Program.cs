using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrabalhoGrafos
{
    class Program
    {
        public static IGrafo grafo = null;

        static void Main(string[] args)
        {
            bool sair = false;
            int arestas = 0;
            int vertices = 0;

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
                                Console.WriteLine("Insira a quantidade de vértices: ");
                                vertices = int.Parse(Console.ReadLine());

                                Console.WriteLine("Insira a quantidade de arestas: ");
                                arestas = int.Parse(Console.ReadLine());

                                List<Aresta> a = new List<Aresta>();
                                List<Vertice> v = new List<Vertice>();

                                for (int i = 1; i < arestas; i++)
                                {

                                    Console.WriteLine("Digite a origem da aresta: ");
                                    int origem = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Digite o destino da aresta: ");
                                    int destino = int.Parse(Console.ReadLine());
                                    Console.WriteLine("Digite o peso da aresta: ");
                                    int peso = int.Parse(Console.ReadLine());

                                    a.Add(new Aresta(new Vertice(origem), new Vertice(destino), peso));
                                }

                                vertices = a.Select(a => a.Origem).Distinct().ToList();

                                Representacao representacao = SelecionadorTipoGrafo.Choose(vertices, arestas);

                                IGrafo grafo = representacao switch
                                {
                                    Representacao.ListaAdjacencia => new GrafoListaAdjacencia(v, a),
                                    //Representacao.MatrizAdjacencia => new GrafoMatrizAdjacencia(vertices, arestas),           
                                    _ => throw new NotSupportedException("Representação de grafo não suportada.")
                                };

                                if (grafo != null)
                                    Console.WriteLine($"Grafo criado com sucesso! \nRepresentação: {grafo.ToString()}");
                                else
                                    Console.WriteLine("Falha ao importar o grafo. Tente novamente.");
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
                        // Aqui você implementará o switch para todas as 15 operações do trabalho
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
                                grafo.VerticesAdjascentes(LerVertice()).ForEach(v => Console.WriteLine(v));
                                break;

                            case 4: //  Imprimir todas as arestas incidentes a um vértice v, informado pelo usuário.
                                grafo.ArestasIncidentes(LerVertice()).ForEach(a => Console.WriteLine(a));
                                break;

                            case 5: //  Imprimir todos os vértices incidentes a uma aresta a, informada pelo usuário.
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
                                Vertice verticeInicial = LerVertice();
                                BuscaLargura(verticeInicial);
                                break;

                            case 11: //  Busca em grafos (Busca em Profundidade)
                                LerAresta();
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
            Console.WriteLine("--- Menu Principal (Grafo Carregado) ---");
            Console.WriteLine("Escolha uma operação para realizar no grafo:");
            Console.WriteLine("1. Imprimir representação do grafo");
            Console.WriteLine("2. Imprimir todas as arestas adjacentes a uma aresta");
            Console.WriteLine("3. Imprimir todos os vértices adjacentes a um vértice");
            Console.WriteLine("4. Imprimir todas as arestas incidentes a um vértice");
            Console.WriteLine("5. Imprimir todos os vértices incidentes a uma aresta");
            Console.WriteLine("6. Imprimir o grau de um vértice");
            Console.WriteLine("7. Determinar se dois vértices são adjacentes");
            Console.WriteLine("8. Substituir o peso de uma aresta");
            Console.WriteLine("9. Trocar dois vértices"); /* Como exemplo, considere um vértice v1 que se conecta aos
                                                            vértices v3, v5 e v7, ao passo que o vértice v2 se conecta aos vértices v4 e v6. 
                                                            A troca dos vértices v1 e v2 implicaria que o vértice v1 estaria conectado aos
                                                            vértices v4 e v6. Por sua vez, o vértice v2 estaria conectado aos vértices v3, v5
                                                            e v7. O usuário deverá informar qual são os dois vértices a serem trocados.
                                                          */
            Console.WriteLine("1 - Imprimir representação do grafo");
            Console.WriteLine("1 - Imprimir representação do grafo");
            Console.WriteLine("1 - Imprimir representação do grafo");
            Console.WriteLine("1 - Imprimir representação do grafo");
            // Adicione as outras 14 opções do trabalho aqui
            /*
        10. Busca em grafos (Busca em Largura): O vértice inicial será dado pelo usuário e
        a respectiva árvore de busca deve ser gerada assim como o nível de cada vértice
        na árvore (nível da raiz é zero), além de apresentar os predecessores. Use a
        ordem numérica crescente para escolher entre os vértices adjacentes.
        11. Busca em grafos (Busca em Profundidade): O vértice inicial será dado pelo
        usuário e a respectiva árvore de busca deve ser gerada assim como a distância
        de descoberta e de finalização de cada vértice na árvore (nível da raiz é zero).
        Use a ordem numérica crescente para escolher entre os vértices adjacentes.
        12. Implementar o Algoritmo de Dijkstra. Este algoritmo, a partir de um vértice origem
        o e um vértice destino d, encontra o caminho mínimo entre eles. Deverá ser
        impresso a rota utilizada, ou seja, os vértices utilizados no caminho mínimo entre
        o e d, com os respectivos pesos de cada aresta do caminho.
        13. Implementar o Algoritmo de Floyd Warshall. Este algoritmo, a partir de um
        vértice origem o, encontra o caminho mínimo entre o vértice o e todos os demais
        vértices do grafo.
        14. Criação de um menu onde o usuário poderá interagir com a aplicação
            */
            // ...
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
            string verticeDesejado = int.Parse(Console.ReadLine());

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
            List<int> L = new List<int>(new int[grafo.NumeroVertices]);
            List<int> nivel = new List<int>(new int[grafo.NumeroVertices]);
            List<Vertice> pai = new List<Vertice>(new Vertice[grafo.NumeroVertices]);

            for (int i = 0; i < grafo.NumeroVertices; i++)
            {
                L[i] = 0;
                nivel[i] = 0;
                pai[i] = null;
            }

            fila.Enqueue(v);
            t++;
            L[v.Id] = t;
            nivel[v.Id] = 0;

            while (fila.Count > 0)
            {
                Vertice atual = fila.Dequeue();

                foreach (Vertice vert in grafo.VerticesAdjascentes(atual).OrderBy(adj => adj.Id))
                {
                    if (L[vert.Id] == 0)
                    {
                        t++;
                        L[vert.Id] = t;
                        nivel[vert.Id] = nivel[atual.Id] + 1;
                        pai[vert.Id] = atual;
                        fila.Enqueue(vert);
                    }
                }
            }

            Console.WriteLine("Resultado da Busca em Largura:");
            for (int i = 0; i < grafo.NumeroVertices; i++)
            {
                if (L[i] != 0)
                {
                    Console.WriteLine($"Vértice: {i}, Nível: {nivel[i]}, Pai: {(pai[i] != null ? pai[i].Id.ToString() : "null")}");
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

        public static bool verticeNaoDescoberto(List<int> td)
        {
            return td.Any(aux => aux == 0); 
        }
    }
}

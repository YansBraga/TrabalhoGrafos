using System.Text;


namespace TrabalhoGrafos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string? grafo = null; 
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
                                Console.Write("Digite o caminho do arquivo DIMACS: ");
                                string caminhoArquivo = Console.ReadLine();                                
                                //grafo = new Grafo(0); // Inicializa o grafo com 0 vértices
                                break;
                            case 2:
                                Console.WriteLine("Insira a quantidade de vértices e arestas respectivamente: ");
                                vertices = int.Parse(Console.ReadLine());
                                arestas = int.Parse(Console.ReadLine());



                                //grafo = new Grafo(vertices, arestas);

                                ExibirMenuPrincipal();
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
                            case 1:
                                // grafo.Imprimir();
                                Console.WriteLine("Funcionalidade de impressão a ser implementada.");
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

        private static void ExibirMenuPrincipal()
        {
            Console.WriteLine("--- Menu Principal (Grafo Carregado) ---");
            Console.WriteLine("Escolha uma operação para realizar no grafo:");
            Console.WriteLine("1 - Imprimir representação do grafo");
            // Adicione as outras 14 opções do trabalho aqui
            // ...
            Console.WriteLine("16 - Descartar grafo e voltar");
            Console.WriteLine("0 - Sair do Programa");
            Console.Write("\nEscolha uma opção: ");
        }

        private static void ExibirMenuCriacao()
        {
            Console.WriteLine("--- Menu de Criação de Grafo ---");
            Console.WriteLine("Nenhum grafo carregado. Por favor, escolha uma opção:");
            Console.WriteLine("1 - Importar Grafo de arquivo DIMACS");
            Console.WriteLine("2 - Preencher manualmente");
            Console.WriteLine("3 - Desafio");
            Console.WriteLine("4 - Sair");
            Console.Write("\nEscolha uma opção: ");
        }
            
    }        
}
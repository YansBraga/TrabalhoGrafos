namespace TrabalhoGrafos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Inicializando um grafo");

            Grafo grafo = new Grafo();

            Vertice v1 = new Vertice("A");
            Vertice v2 = new Vertice("B");
            Vertice v3 = new Vertice("C");

            v1.AdicionarAresta(v2, 5);
            v2.AdicionarAresta(v3, 2);
            v3.AdicionarAresta(v1, 1);


            Console.WriteLine("Grafo criado com os seguintes vértices e arestas:");

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(v1.ToString());

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(v2.ToString());

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine(v3.ToString());

            Console.ReadKey();
        }
    }
}

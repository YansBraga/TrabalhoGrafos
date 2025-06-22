public class Aresta
{
    public Vertice Origem { get; set; }
    public Vertice Destino { get; set; }
    public int Peso { get; set; }
    public Aresta(Vertice origem, Vertice destino, int peso)
    {
        Origem = origem;
        Destino = destino;
        Peso = peso;
    }
    public override string ToString()
    {
        return $"{Origem.Id} -> {Destino.Id} (Peso: {Peso})";
    }
}
using System.Text;
using TrabalhoGrafos;

public class Vertice
{
    private List<Aresta> _arestas;
    private string _nome;

    public Vertice(string nome)
    {
        _nome = nome;
        _arestas = new List<Aresta>();
    }

    public string Nome => _nome;

    public bool AdicionarAresta(Vertice destino, int peso)
    {
        Aresta novaAresta = new Aresta(this, destino, peso);

        if (_arestas.Any(a => a.Destino == destino && a.Peso == peso))
        {
            return false; // Aresta já existe
        }
        _arestas.Add(novaAresta);

        return true;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (Aresta aresta in _arestas)
        {
            sb.AppendLine(aresta.ToString());
        }

        return _nome + "\n" + sb.ToString();
    }
}
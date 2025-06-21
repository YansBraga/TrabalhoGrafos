using System.Text;
using TrabalhoGrafos;

public class Vertice
{
    private List<Aresta> _arestasEntrantes;
    private List<Aresta> _arestasSaintes;
    private string _nome;

    public Vertice(string nome)
    {
        _nome = nome;
        _arestasEntrantes = new List<Aresta>();
        _arestasSaintes = new List<Aresta>();
    }

    public string Nome => _nome;

    public bool AdicionarArestaEntrando(Vertice origem, int peso)
    {
        Aresta novaAresta = new Aresta(origem, this, peso);

        if (_arestasEntrantes.Any(a => a.Origem == origem && a.Peso == peso))
        {
            return false; // Aresta já existe
        }
        _arestasEntrantes.Add(novaAresta);

        return true;
    }

    public bool AdicionarArestaSaindo(Vertice destino, int peso)
    {
        Aresta novaAresta = new Aresta(this, destino, peso);

        if (_arestasSaintes.Any(a => a.Destino == destino && a.Peso == peso))
        {
            return false; // Aresta já existe
        }
        _arestasSaintes.Add(novaAresta);

        return true;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (Aresta aresta in _arestasSaintes)
        {
            sb.AppendLine(aresta.ToString());
        }

        return _nome + "\n" + sb.ToString();
    }
}
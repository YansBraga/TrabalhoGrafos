using System.Text;
using TrabalhoGrafos;

public class Vertice
{
    private List<Aresta> _arestasEntrantes;
    private List<Aresta> _arestasSaindo;
    private string _nome;

    public Vertice(string nome)
    {
        _nome = nome;
        _arestasEntrantes = new List<Aresta>();
        _arestasSaindo = new List<Aresta>();
    }

    public List<Aresta> ArestasEntrantes => _arestasEntrantes;
    public List<Aresta> ArestasSaindo => _arestasSaindo;

    public void setArestasEntrantes(List<Aresta> arestas)
    {
        _arestasEntrantes = arestas;
    }

    public void setArestasSaindo(List<Aresta> arestas)
    {
        _arestasSaindo = arestas;
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

        if (_arestasSaindo.Any(a => a.Destino == destino && a.Peso == peso))
        {
            return false; // Aresta já existe
        }
        _arestasSaindo.Add(novaAresta);

        return true;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        foreach (Aresta aresta in _arestasSaindo)
        {
            sb.AppendLine(aresta.ToString());
        }

        return _nome + "\n" + sb.ToString();
    }
}
using System.Text;
using TrabalhoGrafos;

public class Vertice
{
    private List<Aresta> _arestasEntrantes;
    private List<Aresta> _arestasSaindo;
    private int _id;

    public Vertice(int id)
    {
        _id = id;
        _arestasEntrantes = new List<Aresta>();
        _arestasSaindo = new List<Aresta>();
    }

    public IReadOnlyList<Aresta> ArestasEntrantes => _arestasEntrantes.AsReadOnly();
    public IReadOnlyList<Aresta> ArestasSaindo => _arestasSaindo.AsReadOnly();

    public int Id => _id;

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

        return _id + "\n" + sb.ToString();
    }
}
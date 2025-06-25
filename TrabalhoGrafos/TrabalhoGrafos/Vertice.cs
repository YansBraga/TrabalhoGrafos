using System.Text;
using TrabalhoGrafos;

public class Vertice
{
    private List<Aresta> _arestasEntrantes;
    private List<Aresta> _arestasSaindo;
    private int _id;

    public Vertice(int nome)
    {
        _id = nome;
        _arestasEntrantes = new List<Aresta>();
        _arestasSaindo = new List<Aresta>();
    }

    public List<Aresta> ArestasEntrantes => _arestasEntrantes;
    public List<Aresta> ArestasSaindo => _arestasSaindo;

    public void MudarValor(int novo)
    {
        _id = novo;
    }

    public void setArestasEntrantes(List<Aresta> arestas, Vertice destino)
    {
        _arestasEntrantes = arestas;

        _arestasSaindo.ForEach(i => i.Destino = destino);
    }

    public void setArestasSaindo(List<Aresta> arestas, Vertice origem)
    {
        _arestasSaindo = arestas;

        _arestasSaindo.ForEach(i => i.Origem = origem);
    }

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

    public bool Equals(Vertice other)
    {
        if (other is null) return false;
        return Id == other.Id;
    }

    public override bool Equals(object obj)
        => obj is Vertice v && Equals(v);
}
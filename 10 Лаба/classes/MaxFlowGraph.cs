namespace classes;

public class MaxFlowGraph
{
    private class Edge
    {
        public int To { get; set; }
        public int Capacity { get; set; }
        public int Flow { get; set; }
        public int ReverseIndex { get; set; }
    }

    private readonly List<List<Edge>> _graph;
    private readonly int _n;
    private readonly List<Edge> _allEdges = [];

    public MaxFlowGraph(int n)
    {
        _n = n;
        _graph = new List<List<Edge>>(n);
        for (int i = 0; i < n; i++)
        {
            _graph.Add([]);
        }
    }

    public void AddEdge(int from, int to, int capacity)
    {
        Edge e1 = new Edge { To = to, Capacity = capacity, Flow = 0 };
        Edge e2 = new Edge { To = from, Capacity = 0, Flow = 0 };
        e1.ReverseIndex = _graph[to].Count;
        e2.ReverseIndex = _graph[from].Count;
        _graph[from].Add(e1);
        _graph[to].Add(e2);
        _allEdges.Add(e1);
    }

    public int MaxFlow(int source, int sink)
    {
        int totalFlow = 0;
        int[] parent = new int[_n];
        Edge[] parentEdge = new Edge[_n];

        while (true)
        {
            for (int i = 0; i < _n; i++)
            {
                parent[i] = -1;
                parentEdge[i] = null!;
            }

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            parent[source] = source;

            while (queue.Count > 0)
            {
                int u = queue.Dequeue();

                for (int i = 0; i < _graph[u].Count; i++)
                {
                    Edge e = _graph[u][i];
                    if (parent[e.To] == -1 && e.Capacity > e.Flow)
                    {
                        parent[e.To] = u;
                        parentEdge[e.To] = e;
                        queue.Enqueue(e.To);
                    }
                }
            }

            if (parent[sink] == -1)
                break;

            int augmentFlow = int.MaxValue;
            for (int v = sink; v != source; v = parent[v])
            {
                augmentFlow = Math.Min(augmentFlow, parentEdge[v].Capacity - parentEdge[v].Flow);
            }

            for (int v = sink; v != source; v = parent[v])
            {
                Edge e = parentEdge[v];
                e.Flow += augmentFlow;
                Edge reverse = _graph[e.To][e.ReverseIndex];
                reverse.Flow -= augmentFlow;
            }

            totalFlow += augmentFlow;
        }

        return totalFlow;
    }

    public IEnumerable<(int From, int To, int Flow)> GetEdges()
    {
        for (int from = 0; from < _n; from++)
        {
            foreach (var edge in _graph[from])
            {
                if (edge.Flow > 0)
                {
                    yield return (from, edge.To, edge.Flow);
                }
            }
        }
    }
}
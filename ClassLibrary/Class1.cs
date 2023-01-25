using System;
using System.Collections.Generic;

public class Graph
{
	public int size;
	public int max_size;
	public string[] names;
	public List<List<int>> list = new List<List<int>>();
}

public class Subset
{
	public int size;
	public int max_cut;
	public string[] names;
	public List<List<int>> list = new List<List<int>>();
}

public static class Globals
{
	public static readonly int g_nNumberOfThreads = 4;
	public static Graph graph = new Graph();
	public static Subset subset = new Subset();

	public static List<int> null_vertex = new List<int>();

	//Считывает граф из файла
	public static Graph load_graph(string path)
	{
		string buf = "";
		ifstream fin = new ifstream(path);
		getline(fin, buf, '\n');
		graph.size = Convert.ToInt32(buf);

		// Считываем список смежности
		List<int> buffer = new List<int>();
		int ch = 0;
		int i = 0;
		while ((ch = fin.get()) != EOF)
		{
			if ((char)ch != '\n')
			{
				buffer.Add(ch - 48);
			}
			else
			{
				graph.list.Add(buffer);
				buffer.Clear();
			}
		}
		return new Graph(graph);
	}

	//Выводит граф в консоль
	public static void print_graph(in Graph graph)
	{
		for (int i = 0; i < graph.size; i++)
		{
			Console.Write(" ");

			for (int j = 0; j < graph.list[i].Count; j++)
			{
				Console.Write(graph.list[i][j]);
				Console.Write(" ");
			}

			Console.Write("\n");
		}
	}

	//Проверяет, есть ли вершина в подмножестве отсекаемых вершин
	public static bool is_in_subset(int vertex)
	{
		if (subset.list[vertex] != null_vertex)
		{
			return true;
		}
		return false;
	}

	//Ищет смежную вершину с наибольшим количеством рёбер
	public static int find_max_edges(List<int> vertex)
	{
		int max_edges = 0;
		int max_vertex = 0;

		int curr_vertex = 0;
		int edges = 0;

		for (int i = 0; i < vertex.Count; i++)
		{
			curr_vertex = 0;
			edges = 0;
			curr_vertex = vertex[i];
			edges = graph.list[curr_vertex - 1].Count;

			if (edges > max_edges)
			{
				max_vertex = curr_vertex;
				max_edges = edges;
			}
		}

		return max_vertex - 1;
	}

	//Возвращает число разрезов
	public static int cut_number(List<List<int>> list)
	{
		int result = 0;

		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] != null_vertex)
			{

				result += list[i].Count;
			}
		}

		return result;
	}

	// является ли связным
	public static bool is_related(int vertex)
	{
		for (int i = 0; i < subset.list.Count; i++)
		{
			if (subset.list[i] != null_vertex && (vertex - 1) == i)
			{
				return true;
			}
		}
		return false;
	}

	// удалить связь
	public static void delete_relation()
	{
		for (int i = 0; i < subset.list.Count; i++)
		{
			if (subset.list[i] != null_vertex)
			{
				for (int j = 0; j < subset.list[i].Count; j++)
				{

					if (is_related(new List<List<int>>(subset.list[i][j])))
					{
						int buf = subset.list[i][j] - 1;
						subset.list[i].erase(remove(subset.list[i].GetEnumerator(), subset.list[i].end(), subset.list[i][j]), subset.list[i].end());
						subset.list[buf].erase(remove(subset.list[buf].GetEnumerator(), subset.list[buf].end(), i + 1), subset.list[buf].end());
					}
				}
			}
		}
	}

	// находится ли в вершине
	public static bool is_in_vertex(int vertex, List<int> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i] == vertex + 1)
			{
				return true;
			}
		}
		return false;
	}

	// восстановить связь
	public static void restore_relation(int vertex)
	{

		for (int i = 0; i < subset.list.Count; i++)
		{
			if (subset.list[i] != null_vertex)
			{
				if (is_in_vertex(vertex, new List<List<int>>(graph.list[i])) && !is_in_vertex(vertex, new List<List<int>>(subset.list[i])))
				{
					subset.list[i].Add(vertex + 1);
				}
			}
		}
	}

	//Заносит вершину в подмножесто отсекаемых вершин
	public static void vertex_to_subset(int vertex)
	{
		subset.list[vertex] = new List<int>(graph.list[vertex]);
		delete_relation();
		subset.max_cut = cut_number(new List<List<int>>(subset.list));
	}


	public static bool is_increase(int vertex, List<List<int>> subset_list)
	{
		List<int> graph_vertex = graph.list[vertex];

		subset_list[vertex] = new List<int>(graph_vertex);
		for (int i = 0; i < subset_list.Count; i++)
		{
			if (subset_list[i] != null_vertex)
			{
				for (int j = 0; j < subset_list[i].Count; j++)
				{
					if (is_related(new List<List<int>>(subset_list[i][j])))
					{
						int buf = subset_list[i][j] - 1;
						subset_list[i].erase(remove(subset_list[i].GetEnumerator(), subset_list[i].end(), subset_list[i][j]), subset_list[i].end());
						subset_list[buf].erase(remove(subset_list[buf].GetEnumerator(), subset_list[buf].end(), i + 1), subset_list[buf].end());
					}
				}
			}
		}
		int increased_cuts = cut_number(new List<List<int>>(subset_list));

		if (increased_cuts > subset.max_cut)
		{
			return true;
		}
		return false;
	}
}
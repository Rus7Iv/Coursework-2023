#pragma once
#include <iostream>
#include <fstream>
#include <vector>
#include <list>
#include <locale.h>
#include <string>

using namespace std;

struct Graph
{
	int size;
	int max_size;
	char** names;
	vector<vector<int>> list;
}graph;

struct Subset
{
	int size;
	int max_cut;
	char** names;
	vector<vector<int>> list;
}subset;

vector<int> null_vertex;

//Считывает граф из файла
Graph load_graph(const char* path);

//Выводит граф в консоль
//void print_graph(const Graph graph);

// Выводит подмножество
//void print_subset(vector<vector<int>> list);

//Проверяет, есть ли вершина в подмножестве отсекаемых вершин
bool is_in_subset(int vertex);

//Ищет смежную вершину с наибольшим количеством рёбер
int find_max_edges(vector<int> vertex);

//Возвращает число разрезов
int cut_number(vector<vector<int>> list);

// является ли связным
bool is_related(int vertex);

// удалить связь
void delete_relation();

// находится ли в вершине
bool is_in_vertex(int vertex, vector<int> list);

// восстановить связь
void restore_relation(int vertex);

//Заносит вершину в подмножесто отсекаемых вершин
void vertex_to_subset(int vertex);


bool is_increase(int vertex, vector<vector<int>> subset_list);

bool delete_vertex(int vertex);

//Ищет максимальный разрез графа
vector<vector<int>> find_max_cut();
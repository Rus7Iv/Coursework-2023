#pragma once
//#ifdef LIBRARY_EXPORTS

#include <iostream>
#include <fstream>
#include <vector> // list
#include <list> //link&list
#include <locale.h>
#include <string>

//using namespace std;
//
//struct Graph
//{
//	int size;
//	int max_size;
//	char** names;
//	vector<vector<int>> list;
//}graph;
//
//struct Subset
//{
//	int size;
//	int max_cut;
//	char** names;
//	vector<vector<int>> list;
//}subset;

vector<int> null_vertex;

//Считывает граф из файла
//extern "C" __declspec(dllexport) Graph load_graph(const char* path);

//Проверяет, есть ли вершина в подмножестве отсекаемых вершин
bool is_in_subset(int vertex);

//Ищет смежную вершину с наибольшим количеством рёбер
extern "C" __declspec(dllexport) int find_max_edges(vector<int> vertex);

//Возвращает число разрезов
extern "C" __declspec(dllexport) int cut_number(vector<vector<int>> list);

// является ли связным
extern "C" __declspec(dllexport) bool is_related(int vertex);

// удалить связь
extern "C" __declspec(dllexport) void delete_relation();

// находится ли в вершине
extern "C" __declspec(dllexport) bool is_in_vertex(int vertex, vector<int> list);

// восстановить связь
extern "C" __declspec(dllexport) void restore_relation(int vertex);

//Заносит вершину в подмножесто отсекаемых вершин
extern "C" __declspec(dllexport) void vertex_to_subset(int vertex);


extern "C" __declspec(dllexport) bool is_increase(int vertex, vector<vector<int>> subset_list);

extern "C" __declspec(dllexport) bool delete_vertex(int vertex);

//Ищет максимальный разрез графа
//extern "C" __declspec(dllexport) vector<vector<int>> find_max_cut();

//#endif
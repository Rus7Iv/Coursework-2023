#pragma once
#include <iostream>
#include <fstream>
#include <vector>
#include <list>
#include <locale.h>
#include <string>

using namespace std;

extern "C" __declspec(dllexport) struct Graph
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

//��������� ���� �� �����
extern "C" __declspec(dllexport) Graph load_graph(const char* path);

//������� ���� � �������
extern "C" __declspec(dllexport) void print_graph(const Graph graph);

// ������� ������������
extern "C" __declspec(dllexport) void print_subset(vector<vector<int>> list);

//���������, ���� �� ������� � ������������ ���������� ������
bool is_in_subset(int vertex);

//���� ������� ������� � ���������� ����������� ����
extern "C" __declspec(dllexport) int find_max_edges(vector<int> vertex);

//���������� ����� ��������
extern "C" __declspec(dllexport) int cut_number(vector<vector<int>> list);

// �������� �� �������
extern "C" __declspec(dllexport) bool is_related(int vertex);

// ������� �����
extern "C" __declspec(dllexport) void delete_relation();

// ��������� �� � �������
extern "C" __declspec(dllexport) bool is_in_vertex(int vertex, vector<int> list);

// ������������ �����
extern "C" __declspec(dllexport) void restore_relation(int vertex);

//������� ������� � ����������� ���������� ������
extern "C" __declspec(dllexport) void vertex_to_subset(int vertex);


extern "C" __declspec(dllexport) bool is_increase(int vertex, vector<vector<int>> subset_list);

extern "C" __declspec(dllexport) bool delete_vertex(int vertex);

//���� ������������ ������ �����
extern "C" __declspec(dllexport) vector<vector<int>> find_max_cut();
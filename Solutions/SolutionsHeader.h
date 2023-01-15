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

//��������� ���� �� �����
Graph load_graph(const char* path);

//������� ���� � �������
void print_graph(const Graph graph);

// ������� ������������
void print_subset(vector<vector<int>> list);

//���������, ���� �� ������� � ������������ ���������� ������
bool is_in_subset(int vertex);

//���� ������� ������� � ���������� ����������� ����
int find_max_edges(vector<int> vertex);

//���������� ����� ��������
int cut_number(vector<vector<int>> list);

// �������� �� �������
bool is_related(int vertex);

// ������� �����
void delete_relation();

// ��������� �� � �������
bool is_in_vertex(int vertex, vector<int> list);

// ������������ �����
void restore_relation(int vertex);

//������� ������� � ����������� ���������� ������
void vertex_to_subset(int vertex);


bool is_increase(int vertex, vector<vector<int>> subset_list);

bool delete_vertex(int vertex);

//���� ������������ ������ �����
vector<vector<int>> find_max_cut();
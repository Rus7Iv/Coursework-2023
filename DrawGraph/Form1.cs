using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;
using System.IO;

namespace SystAnalys_lr1
{
    public partial class Form1 : Form
    {
        const string filePath = @"C:\Users\ivano\Desktop\Coursework-2023-main\DrawGraph\adjency_list.txt";

        const string libraryPath = @"C:\Users\ivano\Desktop\Coursework-2023-main\GraphLibrary\Debug\GraphLibrary.dll";

        [DllImport(libraryPath, CallingConvention = CallingConvention.Cdecl)]
        static extern int find_max_graph_cut(string path);

        private DrawGraph graph;
        private List<Vertex> vertexes;
        private List<Edge> edges;
        private int[,] adjencyMatrix; //матрица смежности

        //выбранные вершины, для соединения линиями
        private int selected1;
        private int selected2;

        public Form1()
        {
            InitializeComponent();
            this.adjencyMatrix = null;
            this.vertexes= new List<Vertex>();
            this.graph = new DrawGraph(sheet.Width, sheet.Height);
            this.edges = new List<Edge>();
            this.sheet.Image = this.graph.GetBitmap();
        }

        //кнопка - выбрать вершину
        private void selectButton_Click(object sender, EventArgs e)
        {
            this.selectButton.Enabled = false;
            this.drawVertexButton.Enabled = true;
            this.drawEdgeButton.Enabled = true;
            this.deleteButton.Enabled = true;
            this.graph.clearSheet();
            this.graph.drawGraph(this.vertexes, this.edges);
            this.sheet.Image = this.graph.GetBitmap();
            this.selected1 = -1;
        }

        //кнопка - рисовать вершину
        private void drawVertexButton_Click(object sender, EventArgs e)
        {
            this.drawVertexButton.Enabled = false;
            this.selectButton.Enabled = true;
            this.drawEdgeButton.Enabled = true;
            this.deleteButton.Enabled = true;
            this.graph.clearSheet();
            this.graph.drawGraph(this.vertexes, this.edges);
            this.sheet.Image = this.graph.GetBitmap();
        }

        //кнопка - рисовать ребро
        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            this.drawEdgeButton.Enabled = false;
            this.selectButton.Enabled = true;
            this.drawVertexButton.Enabled = true;
            this.deleteButton.Enabled = true;
            this.graph.clearSheet();
            this.graph.drawGraph(this.vertexes, this.edges);
            this.sheet.Image = this.graph.GetBitmap();
            this.selected1 = -1;
            this.selected2 = -1;
        }

        //кнопка - удалить элемент
        private void deleteButton_Click(object sender, EventArgs e)
        {
            this.deleteButton.Enabled = false;
            this.selectButton.Enabled = true;
            this.drawVertexButton.Enabled = true;
            this.drawEdgeButton.Enabled = true;
            this.graph.clearSheet();
            this.graph.drawGraph(this.vertexes, this.edges);
            this.sheet.Image = this.graph.GetBitmap();
        }

        //кнопка - удалить граф
        private void deleteALLButton_Click(object sender, EventArgs e)
        {
            this.selectButton.Enabled = true;
            this.drawVertexButton.Enabled = true;
            this.drawEdgeButton.Enabled = true;
            this.deleteButton.Enabled = true;

            this.clearAll();
        }

        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (this.selectButton.Enabled == false)
            {
                for (int i = 0; i < this.vertexes.Count; i++)
                {
                    if (Math.Pow((this.vertexes[i].x - e.X), 2) + Math.Pow((this.vertexes[i].y - e.Y), 2) <= this.graph.R * this.graph.R)
                    {
                        if (selected1 != -1)
                        {
                            this.selected1 = -1;
                            this.graph.clearSheet();
                            this.graph.drawGraph(this.vertexes, this.edges);
                            this.sheet.Image = this.graph.GetBitmap();
                        }
                        if (selected1 == -1)
                        {
                            this.graph.drawSelectedVertex(this.vertexes[i].x, this.vertexes[i].y);
                            this.selected1 = i;
                            this.sheet.Image = this.graph.GetBitmap();
                            this.fillAdjencyMatrix();

                            int degree = 0;
                            for (int j = 0; j < this.vertexes.Count; j++)
                                degree += this.adjencyMatrix[this.selected1, j];

                            break;
                        }
                    }
                }
            }

            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                this.vertexes.Add(new Vertex(e.X, e.Y));
                this.graph.drawVertex(e.X, e.Y, this.vertexes.Count.ToString());
                this.sheet.Image = this.graph.GetBitmap();
            }

            //нажата кнопка "рисовать ребро"
            if (this.drawEdgeButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < this.vertexes.Count; i++)
                    {
                        if (Math.Pow((this.vertexes[i].x - e.X), 2) + Math.Pow((this.vertexes[i].y - e.Y), 2) <= this.graph.R * this.graph.R)
                        {
                            if (this.selected1 == -1)
                            {
                                this.graph.drawSelectedVertex(this.vertexes[i].x, this.vertexes[i].y);
                                this.selected1 = i;
                                this.sheet.Image = this.graph.GetBitmap();

                                break;
                            }
                            if (this.selected2 == -1)
                            {
                                this.graph.drawSelectedVertex(this.vertexes[i].x, this.vertexes[i].y);
                                this.selected2 = i;
                                this.edges.Add(new Edge(this.selected1, this.selected2));
                                this.graph.drawEdge(this.vertexes[selected1], this.vertexes[selected2], this.edges[this.edges.Count - 1], this.edges.Count - 1);
                                this.selected1 = -1;
                                this.selected2 = -1;
                                this.sheet.Image = this.graph.GetBitmap();

                                break;
                            }
                        }
                    }
                }

                if (e.Button == MouseButtons.Right)
                {
                    if ((this.selected1 != -1) &&
                        (Math.Pow((this.vertexes[selected1].x - e.X), 2) + Math.Pow((this.vertexes[selected1].y - e.Y), 2) <= this.graph.R * this.graph.R))
                    {
                        this.graph.drawVertex(this.vertexes[selected1].x, this.vertexes[selected1].y, (this.selected1 + 1).ToString());
                        this.selected1 = -1;
                        this.sheet.Image = this.graph.GetBitmap();
                    }
                }
            }
            //нажата кнопка "удалить элемент"
            if (this.deleteButton.Enabled == false)
            {
                //удалили ли что-нибудь по этому клику
                bool flag = false;

                //ищем, возможно была нажата вершина
                for (int i = 0; i < this.vertexes.Count; i++)
                {
                    if (Math.Pow((this.vertexes[i].x - e.X), 2) + Math.Pow((this.vertexes[i].y - e.Y), 2) <= this.graph.R * this.graph.R)
                    {
                        for (int j = 0; j < this.edges.Count; j++)
                        {
                            if ((this.edges[j].v1 == i) || (this.edges[j].v2 == i))
                            {
                                this.edges.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (this.edges[j].v1 > i) 
                                    this.edges[j].v1--;

                                if (this.edges[j].v2 > i) 
                                    this.edges[j].v2--;
                            }
                        }
                        this.vertexes.RemoveAt(i);
                        flag = true;

                        break;
                    }
                }
                //ищем, возможно было нажато ребро
                if (!flag)
                {
                    for (int i = 0; i < this.edges.Count; i++)
                    {
                        if (this.edges[i].v1 == this.edges[i].v2) //если это петля
                        {
                            if ((Math.Pow((this.vertexes[this.edges[i].v1].x - this.graph.R - e.X), 2) + Math.Pow((this.vertexes[this.edges[i].v1].y - this.graph.R - e.Y), 2) <= ((this.graph.R + 2) * (this.graph.R + 2))) &&
                                (Math.Pow((this.vertexes[this.edges[i].v1].x - this.graph.R - e.X), 2) + Math.Pow((this.vertexes[this.edges[i].v1].y - this.graph.R - e.Y), 2) >= ((this.graph.R - 2) * (this.graph.R - 2))))
                            {
                                this.edges.RemoveAt(i);
                                flag = true;

                                break;
                            }
                        }
                        else //не петля
                        {
                            if (((e.X - this.vertexes[this.edges[i].v1].x) * (this.vertexes[this.edges[i].v2].y - this.vertexes[this.edges[i].v1].y) / (this.vertexes[this.edges[i].v2].x - this.vertexes[this.edges[i].v1].x) + this.vertexes[this.edges[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - this.vertexes[this.edges[i].v1].x) * (this.vertexes[this.edges[i].v2].y - this.vertexes[this.edges[i].v1].y) / (this.vertexes[this.edges[i].v2].x - this.vertexes[this.edges[i].v1].x) + this.vertexes[this.edges[i].v1].y) >= (e.Y - 4))
                            {
                                if ((this.vertexes[this.edges[i].v1].x <= this.vertexes[this.edges[i].v2].x && this.vertexes[this.edges[i].v1].x <= e.X && e.X <= this.vertexes[this.edges[i].v2].x) ||
                                    (this.vertexes[this.edges[i].v1].x >= this.vertexes[this.edges[i].v2].x && this.vertexes[this.edges[i].v1].x >= e.X && e.X >= this.vertexes[this.edges[i].v2].x))
                                {
                                    this.edges.RemoveAt(i);
                                    flag = true;

                                    break;
                                }
                            }
                        }
                    }
                }

                //если что-то было удалено, то обновляем граф на экране
                if (flag)
                {
                    this.graph.clearSheet();
                    this.graph.drawGraph(this.vertexes, this.edges);
                    this.sheet.Image = this.graph.GetBitmap();
                }
            }
        }

        private void solutionButton_Click(object sender, EventArgs e)
        {
            if (sheet.Image != null )
            {
                this.fillAdjencyMatrix();

                string sOut = "";

                StreamWriter sw = new StreamWriter(filePath);
                string text = Convert.ToString(this.vertexes.Count);
                sw.WriteLine(text);

                for (int i = 0; i < this.vertexes.Count; i++)
                {
                    sOut = "";
                    for (int j = 0; j < this.vertexes.Count; j++)
                        if (this.adjencyMatrix[i, j] != 0)
                        {
                            sOut += this.adjencyMatrix[i, j] * (j + 1);
                        }
                    sw.WriteLine(sOut);
                }
                sw.Close();

                int result = find_max_graph_cut(filePath);

                MessageBox.Show($"Максимальный разрез графа = {result}", "Результат");
            }
        }

        private void saveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Выберите текстовый файл для записи графа"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter file = new StreamWriter(openFileDialog.FileName);
                    string result = this.graphToString();

                    if (result != null)
                    {
                        file.Write(result);
                        MessageBox.Show("Граф был успешно записан в файл!", "Сообщение");
                    }
                    else
                    {
                        MessageBox.Show("Граф не был записан в файл, потому что \n возникли ошибки в преобразовании структуры графа в текст!", "Ошибка");
                    }

                    file.Close();
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}", "Ошибка");
                }
            }
        }

        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Выберите текстовый файл с графом"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamReader file = new StreamReader(openFileDialog.FileName);
                    string fileContent = file.ReadToEnd();
                    file.Close();

                    // очистка всего
                    this.clearAll();

                    // заполнение графа
                    this.readGraphFromString(fileContent);

                    // заполнение матрицы смежности
                    this.fillAdjencyMatrix();

                    // отрисовка графа
                    this.graph.drawGraph(this.vertexes, this.edges);
                }
                catch (SecurityException ex)
                {
                    MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                    return;
                }
            }
        }

        private string graphToString()
        {
            string result = $"{this.vertexes.Count}\n";

            for (int i = 0; i < this.vertexes.Count; i++)
            {
                result += $"{this.vertexes[i].x} {this.vertexes[i].y}\n";
            }

            for (int i = 0; i < this.edges.Count; i++)
            {
                result += $"{this.edges[i].v1} {this.edges[i].v2}\n";
            }

            return result;
        }

        private void readGraphFromString(string inputString) 
        {
            string[] rows = inputString.Split('\n');

            int vertexesCount = Convert.ToInt32(rows[0]);

            for (int i = 1; i < vertexesCount + 1; i++)
            {
                string[] row = rows[i].Split(' ');
                try
                {
                    this.vertexes.Add(new Vertex(Convert.ToInt32(row[0]), Convert.ToInt32(row[1])));
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректный формат текста!\n" +
                                    "Скорее всего это не граф " +
                                    "или файл некорректный вид.", "Ошибка");
                    this.clearAll();
                }
            }

            for (int i = vertexesCount + 1; i < rows.Length - 1; i++)
            {
                string[] row = rows[i].Split(' ');
                try
                {
                    this.edges.Add(new Edge(Convert.ToInt32(row[0]), Convert.ToInt32(row[1])));
                }
                catch (Exception)
                {
                    MessageBox.Show("Некорректный формат текста!\n" +
                                    "Скорее всего это не граф " +
                                    "или файл некорректный вид.", "Ошибка");
                    this.clearAll();
                }
            }
        }

        // создание списка смежности и запись его в файл
        private void fillAdjencyMatrix()
        {
            this.adjencyMatrix = new int[this.vertexes.Count, this.vertexes.Count];
            this.graph.fillAdjencyMatrix(this.vertexes.Count, this.edges, this.adjencyMatrix);
        }

        private void clearAll()
        {
            this.vertexes.Clear();
            this.edges.Clear();
            this.graph.clearSheet();
            this.sheet.Image = this.graph.GetBitmap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Кнопка \"Выбрать\" - управление курсором;\n" +
                "Кнопка \"Вершина\" - добавление новых вершин;\n" +
                "Кнопка \"Ребро\" - добавление новых рёбер;\n" +
                "Кнопка \"Убрать элемент\" - удаление выбранной пользователем вершины или ребра;\n" +
                "Кнопка \"Очистить страницу\" - удаление всех элементов с экрана;\n" +
                "Кнопка \"Вычислить максимальный разрез графа\" - вычисление максимального разрез графа;\n" +
                "\nЧтобы сохранить или выбрать раннее введённый граф нужно нажать на \"Файл\" " +
                "в левом верхнем углу и выбрать \"Сохранить\" или \"Открыть\" соответственно", "Иструкция по работе с программой");
        }
    }
}

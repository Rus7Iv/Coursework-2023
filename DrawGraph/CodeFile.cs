using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystAnalys_lr1
{
    class Vertex
    {
        public int x, y;

        public Vertex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class Edge
    {
        public int v1, v2;

        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }

    class DrawGraph
    {
        Bitmap bitmap;
        Pen blackPen;
        Pen redPen;
        Pen darkGoldPen;
        Graphics gr;
        Font fo;
        Brush br;
        PointF point;
        public int R = 20; //радиус окружности вершины

        public DrawGraph(int width, int height)
        {
            bitmap = new Bitmap(width, height);
            gr = Graphics.FromImage(bitmap);
            clearSheet();
            blackPen = new Pen(Color.Black);
            blackPen.Width = 2;
            redPen = new Pen(Color.Red);
            redPen.Width = 2;
            darkGoldPen = new Pen(Color.DarkGoldenrod);
            darkGoldPen.Width = 2;
            fo = new Font("Arial", 15);
            br = Brushes.Black;
        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void clearSheet()
        {
            gr.Clear(Color.White);
        }

        public void drawVertex(int x, int y, string number)
        {
            gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
            gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
            point = new PointF(x - 9, y - 9);
            gr.DrawString(number, fo, br, point);
        }

        public void drawSelectedVertex(int x, int y)
        {
            gr.DrawEllipse(redPen, (x - R), (y - R), 2 * R, 2 * R);
        }

        public void drawEdge(Vertex vertex1, Vertex vertex2, Edge edges, int edgesCount)
        {
            if (edges.v1 == edges.v2)
            {
                gr.DrawArc(darkGoldPen, (vertex1.x - 2 * R), (vertex1.y - 2 * R), 2 * R, 2 * R, 90, 270);
                point = new PointF(vertex1.x - (int)(2.75 * R), vertex1.y - (int)(2.75 * R));
                gr.DrawString(((char)('a' + edgesCount)).ToString(), fo, br, point);
                drawVertex(vertex1.x, vertex1.y, (edges.v1 + 1).ToString());
            }
            else
            {
                gr.DrawLine(darkGoldPen, vertex1.x, vertex1.y, vertex2.x, vertex2.y);
                point = new PointF((vertex1.x + vertex2.x) / 2, (vertex1.y + vertex2.y) / 2);
                gr.DrawString(((char)('a' + edgesCount)).ToString(), fo, br, point);
                drawVertex(vertex1.x, vertex1.y, (edges.v1 + 1).ToString());
                drawVertex(vertex2.x, vertex2.y, (edges.v2 + 1).ToString());
            }
        }

        public void drawGraph(List<Vertex> vertexes, List<Edge> edges)
        {
            //рисуем ребра
            for (int i = 0; i < edges.Count; i++)
            {
                if (edges[i].v1 == edges[i].v2)
                {
                    gr.DrawArc(darkGoldPen, (vertexes[edges[i].v1].x - 2 * R), (vertexes[edges[i].v1].y - 2 * R), 2 * R, 2 * R, 90, 270);
                    point = new PointF(vertexes[edges[i].v1].x - (int)(2.75 * R), vertexes[edges[i].v1].y - (int)(2.75 * R));
                    gr.DrawString(((char)('a' + i)).ToString(), fo, br, point);
                }
                else
                {
                    gr.DrawLine(darkGoldPen, vertexes[edges[i].v1].x, vertexes[edges[i].v1].y, vertexes[edges[i].v2].x, vertexes[edges[i].v2].y);
                    point = new PointF((vertexes[edges[i].v1].x + vertexes[edges[i].v2].x) / 2, (vertexes[edges[i].v1].y + vertexes[edges[i].v2].y) / 2);
                    gr.DrawString(((char)('a' + i)).ToString(), fo, br, point);
                }
            }
            //рисуем вершины
            for (int i = 0; i < vertexes.Count; i++)
            {
                drawVertex(vertexes[i].x, vertexes[i].y, (i + 1).ToString());
            }
        }

        //заполняет матрицу смежности
        public void fillAdjencyMatrix(int vertexesCount, List<Edge> edges, int[,] matrix)
        {
            for (int i = 0; i < vertexesCount; i++)
                for (int j = 0; j < vertexesCount; j++)
                    matrix[i, j] = 0;

            for (int i = 0; i < edges.Count; i++)
            {
                matrix[edges[i].v1, edges[i].v2] = 1;
                matrix[edges[i].v2, edges[i].v1] = 1;
            }
        }        
    }
}
using System;
using System.Collections.Generic;
using AdventOfCode2021;

namespace AStarSharp
{
    public class Node
    {
        // Change this depending on what the desired size is for each element in the grid
        public static int NODE_SIZE = 1;
        public Node Parent;
        public Vector2 Position;
        public int DistanceToTarget;
        public int Cost;
        public int Weight;
        public Vector2 Center => new Vector2(Position.x + NODE_SIZE / 2, Position.y + NODE_SIZE / 2);

        public int F
        {
            get {
                if (DistanceToTarget != -1 && Cost != -1)
                    return DistanceToTarget + Cost;
                return -1;
            }
        }
        public bool Walkable;

        public Node(Vector2 pos, bool walkable, int weight = 1)
        {
            Parent = null;
            Position = pos;
            DistanceToTarget = -1;
            Cost = 1;
            Weight = weight;
            Walkable = walkable;
        }
    }

    public class Astar
    {
        Node[][] Grid;
        int GridRows
        {
            get
            {
               return Grid[0].Length;
            }
        }
        int GridCols
        {
            get
            {
                return Grid.Length;
            }
        }

        public Astar(int[][] grid) {
            Grid = new Node[grid.Length][];
            for (int x = 0; x < grid.Length; x++) {
                Grid[x] = new Node[grid[x].Length];
                for (int y = 0; y < grid[x].Length; y++) {
                    Grid[x][y] = new Node(new Vector2(x, y), true, grid[x][y]);
                }
            }
        }

        public Stack<Node> FindPath(Vector2 Start, Vector2 End) {
            int nodeCount = GridCols * GridRows;
            Node start = new Node(new Vector2(Start.x/Node.NODE_SIZE, Start.y/Node.NODE_SIZE), true);
            Node end = new Node(new Vector2(End.x / Node.NODE_SIZE, End.y / Node.NODE_SIZE), true);

            Stack<Node> Path = new Stack<Node>();
            LinkedList<Node> OpenList = new LinkedList<Node>();
            Dictionary<Vector2, Node> ClosedList = new Dictionary<Vector2, Node>();
            List<Node> adjacentTemp = new List<Node>();
            List<Node> adjacencies;
            Node current = start;
           
            // add start node to Open List
            OpenList.AddFirst(start);

            while(OpenList.Count != 0 && !ClosedList.ContainsKey(end.Position))
            {
                Console.Out.WriteLine("({2:P0}) Open = {0}, Closed = {1}", OpenList.Count, ClosedList.Count, ClosedList.Count / (float)nodeCount);
                current = OpenList.First.Value;
                OpenList.Remove(current);
                ClosedList[current.Position] = current;
                adjacencies = GetAdjacentNodes(current, adjacentTemp);

 
                foreach(Node n in adjacencies)
                {
                    if (!ClosedList.ContainsValue(n) && n.Walkable)
                    {
                        if (!OpenList.Contains(n))
                        {
                            n.Parent = current;
                            n.DistanceToTarget = Math.Abs(n.Position.x - end.Position.x) + Math.Abs(n.Position.y - end.Position.y);
                            n.Cost = n.Weight + n.Parent.Cost;

                            var node = OpenList.First;
                            while (node != null && node.Value.F < n.F) {
                                node = node.Next;
                            }

                            if (node != null) {
                                OpenList.AddBefore(node, n);
                            } else {
                                OpenList.AddLast(n);
                            }
                        }
                    }
                }
            }
            
            // construct path, if end was not closed return null
            if(!ClosedList.ContainsKey(end.Position))
            {
                return null;
            }

            // if all good, return path
            Node temp = current;
            //if (temp == null) return null;
            do
            {
                Path.Push(temp);
                temp = temp.Parent;
            } while (temp != start && temp != null) ;
            return Path;
        }
		
        private List<Node> GetAdjacentNodes(Node n, List<Node> adjacentTemp)
        {
            adjacentTemp.Clear();

            int row = n.Position.y;
            int col = n.Position.x;

            if(row + 1 < GridRows)
            {
                adjacentTemp.Add(Grid[col][row + 1]);
            }
            if(row - 1 >= 0)
            {
                adjacentTemp.Add(Grid[col][row - 1]);
            }
            if(col - 1 >= 0)
            {
                adjacentTemp.Add(Grid[col - 1][row]);
            }
            if(col + 1 < GridCols)
            {
                adjacentTemp.Add(Grid[col + 1][row]);
            }

            return adjacentTemp;
        }
    }
}
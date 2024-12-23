namespace AdventOfCode8
{
    public class Pathfind
    {

        public static List<Vec2> Directions =
        [
            new Vec2(1, 0),
            new Vec2(-1, 0),
            new Vec2(0, 1),
            new Vec2(0, -1)
        ];

        public static double Heuristic(Vec2 current, Vec2 end)
        {
            return (end - current).Length();
        }

        public static List<Vec2> ConstructPath(Dictionary<Vec2, Vec2> parentMap, Vec2 current)
        {
            var path = new List<Vec2>() { current };

            while (parentMap.ContainsKey(current))
            {
                current = parentMap[current];
                path.Add(current);
            }

            path.Reverse();
            return path;
        }

        public static List<Vec2> DFS(Vec2 start, Vec2 end, int[,] map)
        {
            var queue = new Queue<Vec2>();
            queue.Enqueue(start);
            var visited = new HashSet<Vec2> { start };
            var parentMap = new Dictionary<Vec2, Vec2>();

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                if (current == end)
                {
                    return ConstructPath(parentMap, current);
                }
                visited.Add(current);

                foreach (var dir in Directions)
                {
                    var neighbor = current + dir;
                    if (visited.Contains(neighbor) || neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= map.GetLength(0) || neighbor.Y >= map.GetLength(1) || map[neighbor.X, neighbor.Y] != 0)
                    {
                        continue;
                    }


                    queue.Enqueue(neighbor);
                    parentMap[neighbor] = current;
                }
            }
            return [];
        }

        public static List<Vec2> AStar(Vec2 start, Vec2 end, int[,] map)
        {
            var openPoints = new PriorityQueue<Vec2, double>();
            var addedPoints = new HashSet<Vec2>();
            var closedPoints = new HashSet<Vec2>();

            var startHeuristic = Heuristic(start, end);
            openPoints.Enqueue(start, startHeuristic);

            var gScore = new Dictionary<Vec2, double> { [start] = 0 };
            var hScore = new Dictionary<Vec2, double> { [start] = startHeuristic };
            var parentMap = new Dictionary<Vec2, Vec2>();

            while (openPoints.Count > 0)
            {
                var current = openPoints.Dequeue();
                if (current == end)
                {
                    return ConstructPath(parentMap, current);
                }

                closedPoints.Add(current);

                foreach (var dir in Directions)
                {
                    var neighbor = current + dir;
                    if (closedPoints.Contains(neighbor) || neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= map.GetLength(0) || neighbor.Y >= map.GetLength(1) || map[neighbor.X, neighbor.Y] != 0)
                    {
                        continue;
                    }

                    var tentativeG = gScore[current] + 1;
                    if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                    {
                        var fScore = gScore[neighbor] = tentativeG;
                        fScore += hScore[neighbor] = Heuristic(neighbor, end);

                        parentMap[neighbor] = current;

                        if (!addedPoints.Contains(neighbor))
                        {
                            openPoints.Enqueue(neighbor, fScore);
                            addedPoints.Add(neighbor);
                        }
                    }
                }
            }

            return [];
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public static class GridPathfinder
    {
        public static readonly Vector2Int[] directions =
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right,
        };

        public static List<Vector2Int> FindPath(
            Vector2Int start,
            Vector2Int goal,
            int width,
            int height,
            HashSet<Vector2Int> blockedCells
            )
        {
            var path = new List<Vector2Int>();

            if (!IsInsideGrid(start, width, height) ||
                !IsInsideGrid(goal, width, height))
            {
                return path;
            }

            if (blockedCells != null && blockedCells.Contains(goal))
            {
                return path;
            }

            var openSet = new List<Vector2Int> { start };
            var closedSet = new HashSet<Vector2Int>();

            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, int>
            {
                [start] = 0
            };

            var fScore = new Dictionary<Vector2Int, int>
            {
                [start] = GetDistance(start, goal)
            };

            while (openSet.Count > 0)
            {
                int bestIndex = 0;

                for (int i = 1; i < openSet.Count; i++)
                {
                    if (fScore[openSet[i]] < fScore[openSet[bestIndex]])
                    {
                        bestIndex = i;
                    }
                }

                Vector2Int current = openSet[bestIndex];

                if (current == goal)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.RemoveAt(bestIndex);
                closedSet.Add(current);

                foreach (Vector2Int direction in directions)
                {
                    Vector2Int neighbor = current + direction;

                    if (!IsInsideGrid(neighbor, width, height))
                        continue;

                    if (blockedCells != null &&
                        blockedCells.Contains(neighbor))
                    {
                        continue;
                    }

                    if (closedSet.Contains(neighbor))
                        continue;

                    int tentativeGScore = gScore[current] + 1;

                    if (gScore.TryGetValue(neighbor, out int previousGScore) &&
                        tentativeGScore >= previousGScore)
                    {
                        continue;
                    }

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] =
                        tentativeGScore + GetDistance(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }

            return path;
        }

        private static List<Vector2Int> ReconstructPath(
            Dictionary<Vector2Int, Vector2Int> cameFrom,
            Vector2Int current)
        {
            var path = new List<Vector2Int> { current };

            while (cameFrom.TryGetValue(current, out Vector2Int previous))
            {
                current = previous;
                path.Add(current);
            }

            path.Reverse();

            return path;
        }

        private static int GetDistance(Vector2Int a, Vector2Int b)
        {
            // Манхэттенское расстояние: диагонали не учитываются.
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private static bool IsInsideGrid(
            Vector2Int cell,
            int width,
            int height)
        {
            return cell.x >= 0 &&
                   cell.y >= 0 &&
                   cell.x < width &&
                   cell.y < height;
        }
    }
}

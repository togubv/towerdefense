using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public class RouteStorage
    {
        public readonly Dictionary<int, Route> routes = new();

        public void Add(int id, Vector3[] pointsPos)
        {
            routes[id] = new Route { points = pointsPos };
        }

        public Route Get(int id)
        {
            return routes[id];
        }
    }
}

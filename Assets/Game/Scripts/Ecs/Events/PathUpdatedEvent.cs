using System.Collections.Generic;
using UnityEngine;

namespace TD
{
    public struct PathUpdatedEvent
    {
        public HashSet<Vector2Int> blockedCells;
        public int width;
        public int height;
    }
}
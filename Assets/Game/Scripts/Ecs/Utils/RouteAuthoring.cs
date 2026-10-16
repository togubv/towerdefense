using UnityEngine;

namespace TD
{
    public class RouteAuthoring : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private float radius = 0.1f;
        [SerializeField] private Color color;
        [SerializeField] private Transform[] points;

        public void Initialize(RouteStorage storage)
        {
            storage.Add(id, GetPoints());
        }

        public Vector3[] GetPoints()
        {
            Vector3[] result = new Vector3[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                result[i] = points[i].position;
            }

            return result;
        }

        private void OnDrawGizmosSelected()
        {
            if (points == null || points.Length == 0)
                return;

            Gizmos.color = color;

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] == null)
                    continue;

                Gizmos.DrawSphere(points[i].position, radius);

                if (i + 1 < points.Length && points[i + 1] != null)
                {
                    Gizmos.DrawLine(
                        points[i].position,
                        points[i + 1].position);
                }
            }
        }
    }
}

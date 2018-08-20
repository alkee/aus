// https://bitbucket.org/alkee/aus

using UnityEngine;

namespace aus.Debugging
{
    [ExecuteInEditMode]
    public class RadicalLinesGizmo : MonoBehaviour
    {
        public Vector3 LocalCenterPos;
        public Vector3[] Directions;

        private Renderer r;
        private float size;

        public static Color[] LineColors = { Color.red, Color.green, Color.blue, Color.cyan, Color.magenta, Color.yellow
        , Color.gray};

        public static Color GetColor(int i)
        {
            if (i >= LineColors.Length) return LineColors[LineColors.Length - 1];
            return LineColors[i];
        }

        void Start()
        {
            r = GetComponent<Renderer>();
            if (r == null) r = GetComponentInChildren<Renderer>();
            if (Directions == null || Directions.Length == 0) enabled = false;

            size = r.bounds.extents.magnitude;
        }

        private void OnDrawGizmosSelected()
        {
            if (enabled == false) return;

            var index = 0;
            foreach (var n in Directions)
            {
                Gizmos.color = GetColor(index);
                Gizmos.DrawRay(transform.position + LocalCenterPos, n * size);
                ++index;
            }
        }
    }
}

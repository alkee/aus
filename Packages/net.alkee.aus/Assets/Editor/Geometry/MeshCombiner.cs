using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aus.Geometry
{
    [CustomEditor(typeof(MeshCombiner))]
    public class MeshCombinerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var combiner = (target as MeshCombiner);

            if (GUILayout.Button("Combine/Refresh"))
            {
                combiner.CombineMeshes();
            }
        }
    }
}

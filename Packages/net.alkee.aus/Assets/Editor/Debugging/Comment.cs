using UnityEngine;
using UnityEditor;

namespace aus.Debugging
{
    [CustomEditor(typeof(Comment))]
    public class CommentPropertyEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as Comment;
            t._ = EditorGUILayout.TextArea(t._, GUILayout.MinHeight(32));
        }
    }
}

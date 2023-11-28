using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace aus.Event
{
    [CustomEditor(typeof(EnableDisableInit))]
    public class EnableDisableInitInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            var t = target as EnableDisableInit;

            EditorGUILayout.HelpBox("지정된 모든 GameObject 들은 바로 하위(child)의 GameObject 이어야 한다. 그외 자손(grandchild 등)의 경우 해당 부모에서 EnableDisableInit 을 사용해야한다", MessageType.None, true);
            EditorGUILayout.Space();

            showEnabledGameObject = EditorGUILayout.Foldout(showEnabledGameObject
                , new GUIContent("Enable on start(play)", "시작할 때 강제로 Active 시킬 항목"), true);
            if (showEnabledGameObject) ListGameObjects(t.gameObject, t.EnableTargets);
            EditorGUILayout.Space();

            showDisabledGameObject = EditorGUILayout.Foldout(showDisabledGameObject
                , new GUIContent("Disable on start(play)", "시작할 때 강제로 Inactive 시킬 항목"), true);
            if (showDisabledGameObject) ListGameObjects(t.gameObject, t.DisableTargets);
            EditorGUILayout.Space();

            if (t.EnableTargets.Count + t.DisableTargets.Count > 0
                && (t.EnableTargets.Count == 0 || t.DisableTargets.Count == 0))
            {
                EditorGUILayout.HelpBox("모든 자식이 같은 disable 또는 enable 상태로 시작해야 한다면 EnableDisableAllInit 을 사용하는 것으로 고려해 볼 것.", MessageType.Info, true);
                EditorGUILayout.Space();
            }
        }

        private bool showEnabledGameObject = true;
        private bool showDisabledGameObject = true;

        private void ListGameObjects(GameObject parent, List<GameObject> children)
        {
            for (var i = 0; i < children.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal();

                var r = GUILayout.Button(new GUIContent("-", "Remove this"), GUILayout.ExpandWidth(false));
                var child = children[i];
                var isChild = parent.transform.Cast<Transform>().Any((t) => t == child.transform); // prev info
                var label = isChild ? "child GameObject" : "NOT CHILD";

                var prev = GUI.color;
                if (isChild == false) GUI.color = Color.red;
                child = EditorGUILayout.ObjectField(label, child, typeof(GameObject), true) as GameObject;
                GUI.color = prev;

                if (r || child == null)
                {
                    children[i] = null;
                }
                else
                {
                    children[i] = child;
                }
                EditorGUILayout.EndHorizontal();
            }
            children.RemoveAll((g) => g == null);
            var created = EditorGUILayout.ObjectField("Insert new CHILD", null, typeof(GameObject), true) as GameObject;
            if (created != null) children.Add(created);
        }
    }
}

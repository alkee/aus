// https://bitbucket.org/alkee/aus
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace aus.Event
{
    // *CAUSION* this may not work if the target UnityEvent is changed somewhere else
    public class FirstUnityEventDetector : MonoBehaviour
    {
        public UnityEventField TargetField;
        public UnityEvent OnFirstInvoke;

        void Start()
        {
            if (TargetField != null)
            {
                target = TargetField.GetTarget();
                if (target != null)
                {
                    target.AddListener(OnInvoke);
                }
            }
        }

        private UnityEvent target;
        private void OnInvoke()
        {
            target.RemoveListener(OnInvoke);
            OnFirstInvoke.Invoke();
        }
    }


    [Serializable]
    public class UnityEventField
    {
        public GameObject Instance;
        public Component Component;
        public string EventName;

        public UnityEvent GetTarget()
        {
            if (Instance == null || Component == null || string.IsNullOrEmpty(EventName)) return null;
            var members = Component.GetType().GetMember(EventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);
            if (members.Length < 1) return null;
            var member = members[0];
            var field = member as FieldInfo;
            if (field == null) return null;
            return field.GetValue(Component) as UnityEvent;
        }

    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(UnityEventField), true)]
    public class UnityEventFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty propInstance = property.FindPropertyRelative("Instance");
            Debug.Assert(propInstance != null);
            SerializedProperty propComponent = property.FindPropertyRelative("Component");
            Debug.Assert(propComponent != null);
            SerializedProperty propEventName = property.FindPropertyRelative("EventName");
            Debug.Assert(propEventName != null);

            var firstLine = position;
            firstLine.height = EditorGUIUtility.singleLineHeight;

            var secondLine = position;
            secondLine.y = EditorGUIUtility.singleLineHeight + 1;
            secondLine.height = EditorGUIUtility.singleLineHeight;

            var grid = GetGridRect(position, 2, 2);
            GUI.Label(grid[0, 0], "Source");

            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(grid[1, 0], propInstance, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            { // reset
                propComponent.objectReferenceValue = null;
                propEventName.stringValue = null;
            }

            using (new EditorGUI.DisabledScope(propInstance.objectReferenceValue == null))
            {
                var content = NO_SOURCE;

                if (propComponent.objectReferenceValue != null && string.IsNullOrEmpty(propEventName.stringValue) == false)
                {
                    content = propComponent.objectReferenceValue.GetType().Name
                        + "." + propEventName.stringValue;
                }

                if (GUI.Button(grid[1, 1], new GUIContent(content), EditorStyles.popup))
                {
                    BuildPopupList(
                        propInstance.objectReferenceValue as GameObject,
                        propComponent, propEventName
                        ).DropDown(grid[1, 1]);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 3;
        }

        private Rect[,] GetGridRect(Rect area, int column, int row, float span = 2)
        {
            var array = new Rect[column, row];
            var w = area.width / column - span;
            var h = area.height / row - span;
            for (var r = 0; r < row; ++r)
            {
                for (var c = 0; c < column; ++c)
                {
                    array[c, r] = new Rect(area.x + (w + span) * c, area.y + (h + span) * r, w, h);
                }
            }
            return array;
        }

        private static GenericMenu BuildPopupList(GameObject instance, SerializedProperty component, SerializedProperty memberName)
        {
            var menu = new GenericMenu();

            var invalid = component.objectReferenceValue == null || string.IsNullOrEmpty(memberName.stringValue);
            menu.AddItem(new GUIContent(NO_SOURCE), invalid, () =>
            {
                component.objectReferenceValue = null;
                component.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                memberName.stringValue = null;
                memberName.serializedObject.ApplyModifiedProperties(); // why this is needed ?
            });

            menu.AddSeparator("");

            var components = instance.GetComponents<Component>();
            foreach (var c in components)
            {
                var type = c.GetType();
                var name = type.Name;
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

                // TODO: support inheritancy
                bindingFlags |= BindingFlags.DeclaredOnly;

                var fields = type.GetFields(bindingFlags);
                foreach (var f in fields)
                {
                    if (f.FieldType != typeof(UnityEvent)) continue;
                    var on = c == component.objectReferenceValue && f.Name == memberName.stringValue;
                    menu.AddItem(new GUIContent(c.GetType().Name + "/" + f.Name), on, () =>
                    {
                        component.objectReferenceValue = c;
                        component.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                        memberName.stringValue = f.Name;
                        memberName.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                    });
                }
            }
            return menu;
        }

        private const string NO_SOURCE = "No source";
    }

#endif
}

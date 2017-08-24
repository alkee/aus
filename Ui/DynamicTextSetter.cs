using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI
{
    [RequireComponent(typeof(Text))]
    [DisallowMultipleComponent]
    public class DynamicTextSetter : MonoBehaviour
    {
        public FieldOrProperty Source;
        [Tooltip("if the value is null, this NullAlias will be used instead and the Foramt will be ignored")]
        public string NullAlias;
        [Tooltip("c# string format like {0:0.00}")]
        public string Format = "{0}";

        void Awake()
        {
            target = GetComponent<Text>();
        }

        private void Start()
        {
            if (Source == null || Source.IsValid() == false) return;
            SetText(Source.GetValue(Format));
        }

        void Update()
        {
            if (Source == null || Source.IsValid() == false) return;
            var val = Source.GetValue(Format);
            if (lastText == val) return;
            SetText(val);
        }

        private Text target;
        private string lastText;

        private void SetText(string val)
        {
            lastText = val;
            target.text = val ?? NullAlias;
        }
    }

    [Serializable]
    public class FieldOrProperty
    {
        public GameObject Instance;
        public Component Component;
        public string MemberName;

        public bool IsValid()
        {
            if (Instance == null || Component == null) return false;
            var memberInfo = Component.GetType().GetMember(MemberName);
            if (memberInfo == null || memberInfo.Length != 1) return false;
            return true;
        }

        public string GetValue(string format)
        {
            if (IsValid() == false) return null;

            var memberInfo = Component.GetType().GetMember(MemberName);
            var value = GetValue(memberInfo[0]);
            if (value == null) return null;

            return string.Format(format, value);
        }

        private object GetValue(MemberInfo info)
        {
            if (info.MemberType == MemberTypes.Field)
                return (info as FieldInfo).GetValue(Component);
            else if (info.MemberType == MemberTypes.Property)
                return (info as PropertyInfo).GetValue(Component, null);

            return null;
        }

    }

#if UNITY_EDITOR

    // TODO: generalize member selector by refection
    [CustomPropertyDrawer(typeof(FieldOrProperty), true)]
    class FieldOrPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty propInstance = property.FindPropertyRelative("Instance");
            Debug.Assert(propInstance != null);
            SerializedProperty propComponent = property.FindPropertyRelative("Component");
            Debug.Assert(propComponent != null);
            SerializedProperty propMemberName = property.FindPropertyRelative("MemberName");
            Debug.Assert(propMemberName != null);

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
                propMemberName.stringValue = null;
            }

            using (new EditorGUI.DisabledScope(propInstance.objectReferenceValue == null))
            {
                var content = NO_SOURCE;

                if (propComponent.objectReferenceValue != null && string.IsNullOrEmpty(propMemberName.stringValue) == false)
                {
                    content = propComponent.objectReferenceValue.GetType().Name
                        + "." + propMemberName.stringValue;
                }

                if (GUI.Button(grid[1, 1], new GUIContent(content), EditorStyles.popup))
                {
                    BuildPopupList(
                        propInstance.objectReferenceValue as GameObject,
                        propComponent, propMemberName
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
                var type = c.GetType(); // TODO: duplicated component
                var name = type.Name;
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

                // TODO: support inheritancy
                bindingFlags |= BindingFlags.DeclaredOnly;

                var fields = type.GetFields(bindingFlags);
                foreach (var f in fields)
                {
                    var on = c == component.objectReferenceValue && f.Name == memberName.stringValue;
                    menu.AddItem(new GUIContent(c.GetType().Name + "/" + f.Name), on, () =>
                    {
                        component.objectReferenceValue = c;
                        component.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                        memberName.stringValue = f.Name;
                        memberName.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                    });
                }

                var properties = type.GetProperties(bindingFlags | BindingFlags.GetField);
                foreach (var p in properties)
                {
                    var on = c == component.objectReferenceValue && p.Name == memberName.stringValue;
                    menu.AddItem(new GUIContent(c.GetType().Name + "/" + p.Name), on, () =>
                    {
                        component.objectReferenceValue = c;
                        component.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                        memberName.stringValue = p.Name;
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

using System;
using UnityEngine;
using System.Reflection;
using System.Linq;
using UnityEditor;

namespace aus.Property
{
    // PropertyOrFieldAttribute 없이 사용하는 경우 경고
    [CustomPropertyDrawer(typeof(ComponentMemberField))]
    public class ComponentMemberFieldDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.HelpBox(position, "ComponentMemberField " + property.name
                + " needs PropertyOrFieldAttribute", MessageType.Error);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }

    [CustomPropertyDrawer(typeof(PropertyOrFieldAttribute))]
    public class PropertyOrFieldAttributeDrawer : PropertyDrawer
    {
        private bool IsValid(SerializedProperty property)
        {
            return property.type == typeof(ComponentMemberField).Name;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty propInstance = property.FindPropertyRelative("Instance");
            Debug.Assert(propInstance != null);
            SerializedProperty propComponent = property.FindPropertyRelative("Component");
            Debug.Assert(propComponent != null);
            SerializedProperty propMemberName = property.FindPropertyRelative("MemberName");
            Debug.Assert(propMemberName != null);

            EditorGUI.BeginChangeCheck();
            position.height *= 0.5f;
            EditorGUI.PropertyField(position, propInstance, label, true);
            if (EditorGUI.EndChangeCheck())
            { // reset
                propComponent.objectReferenceValue = null;
                propMemberName.stringValue = null;
                propInstance.serializedObject.ApplyModifiedProperties();
            }

            using (new EditorGUI.DisabledScope(propInstance.objectReferenceValue == null))
            {
                var content = NO_SOURCE;

                if (propComponent.objectReferenceValue != null && string.IsNullOrEmpty(propMemberName.stringValue) == false)
                {
                    content = propComponent.objectReferenceValue.GetType().Name
                        + "." + propMemberName.stringValue;
                }

                position.y += position.height;
                position.x += EditorGUIUtility.labelWidth;
                position.width -= position.x;

                if (GUI.Button(position, new GUIContent(content), EditorStyles.popup))
                {
                    BuildPopupList(
                        propInstance.objectReferenceValue as GameObject,
                        propComponent, propMemberName, attribute as PropertyOrFieldAttribute
                        ).DropDown(position);
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2 + 3;
        }

        private static GenericMenu BuildPopupList(GameObject instance, SerializedProperty component
            , SerializedProperty memberName, PropertyOrFieldAttribute attr)
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
                var type = c.GetType(); // TODO: duplicated component handling
                var name = type.Name;
                var bindingFlags = BindingFlags.Instance | BindingFlags.Public;

                // TODO: read only members support
                bindingFlags |= BindingFlags.SetField | BindingFlags.SetProperty;

                // TODO: support inheritancy
                bindingFlags |= BindingFlags.DeclaredOnly;

                // https://stackoverflow.com/questions/12680341/how-to-get-both-fields-and-properties-in-single-call-via-reflection
                MemberInfo[] members = type.GetFields(bindingFlags).Cast<MemberInfo>()
                    .Concat(type.GetProperties(bindingFlags)).ToArray();
                foreach (var m in members)
                {
                    var memberType = (m is PropertyInfo) ? (m as PropertyInfo).PropertyType
                        : (m as FieldInfo).FieldType;
                    if (memberType != attr.TargetType && memberType.IsSubclassOf(attr.TargetType) == false)
                    {
                        continue;
                    }
                    var on = c == component.objectReferenceValue && m.Name == memberName.stringValue;
                    menu.AddItem(new GUIContent(c.GetType().Name + "/" + m.Name), on, () =>
                    {
                        component.objectReferenceValue = c;
                        component.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                        memberName.stringValue = m.Name;
                        memberName.serializedObject.ApplyModifiedProperties(); // why this is needed ?
                    });
                }
            }

            return menu;
        }

        private const string NO_SOURCE = "No source";

    }
}
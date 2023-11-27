using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEditor;

namespace aus.Property
{
    [CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
    public class ConditionalHidePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
            if (condHAtt.Reversed) enabled = !enabled;

            bool wasEnabled = GUI.enabled;
            GUI.enabled = enabled;
            if (!condHAtt.HideInInspector || enabled)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }

            GUI.enabled = wasEnabled;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
            bool enabled = GetConditionalHideAttributeResult(condHAtt, property);
            if (condHAtt.Reversed) enabled = !enabled;

            if (!condHAtt.HideInInspector || enabled)
            {
                return EditorGUI.GetPropertyHeight(property, label);
            }
            else
            {
                return -EditorGUIUtility.standardVerticalSpacing;
            }
        }

        private bool GetConditionalHideAttributeResult(ConditionalHideAttribute condHAtt, SerializedProperty property)
        {
            bool enabled = true;
            string propertyPath = property.propertyPath; //returns the property path of the property we want to apply the attribute to

            var andFields = condHAtt.ConditionalSourceField.Split(',');

            foreach (var f in andFields)
            {
                string conditionPath = propertyPath.Replace(property.name, f); //changes the path to the conditionalsource property path
                SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);
                if (sourcePropertyValue == null)
                {
                    Debug.LogWarning("Attempting to use a ConditionalHideAttribute but no matching SourcePropertyValue found in object: " + f);
                    continue;
                }
                if (sourcePropertyValue.type != "bool") continue;

                enabled = sourcePropertyValue.boolValue;
                if (enabled == false) break;
            }
            return enabled;
        }
    }
}
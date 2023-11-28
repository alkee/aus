using UnityEngine;
using UnityEditor;

namespace aus.Property
{
    [CustomPropertyDrawer(typeof(HelpBoxAttribute))]
    public class HelpBoxAttributeDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            var attr = attribute as HelpBoxAttribute;
            if (attr == null) return 0;
            return EditorGUIUtility.singleLineHeight * 2.0f;
        }

        public override void OnGUI(Rect position)
        {
            var attr = attribute as HelpBoxAttribute;
            if (attr == null) return;

            EditorGUI.HelpBox(position, attr.Message, (MessageType)attr.Icon);
        }
    }
}
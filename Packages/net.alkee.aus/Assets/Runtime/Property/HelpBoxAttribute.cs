using UnityEngine;

namespace aus.Property
{
    public class HelpBoxAttribute : PropertyAttribute
    {
        public enum IconType { NONE = 0, INFORMATION = 1, WARNING = 2, ERROR = 3 }
        public string Message;
        public IconType Icon;
        public HelpBoxAttribute(string message, IconType icon = IconType.INFORMATION)
        {
            Message = message;
            Icon = icon;
        }
    }
}
using System;
using UnityEngine;
using System.Reflection;

namespace aus.Property
{
    /// <summary>
    /// ComponentMemberField must be used with PropertyOrFieldAttribute
    /// </summary>
    /// <example>
    ///     [PropertyOrField(typeof(Vector3))]
    ///     public ComponentMemberField TargetVector3;
    /// </example>
    [Serializable]
    public class ComponentMemberField
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

        public object GetValue()
        {
            if (IsValid() == false) return null;
            var info = Component.GetType().GetMember(MemberName)[0];
            if (info.MemberType == MemberTypes.Field)
            {
                return (info as FieldInfo).GetValue(Component);
            }
            else if (info.MemberType == MemberTypes.Property)
            {
                return (info as PropertyInfo).GetValue(Component, null);
            }
            return null;
        }

        public void SetValue(object val)
        {
            if (IsValid() == false) return;

            var info = Component.GetType().GetMember(MemberName)[0];
            if (info.MemberType == MemberTypes.Field)
            {
                (info as FieldInfo).SetValue(Component, val);
            }
            else if (info.MemberType == MemberTypes.Property)
            {
                (info as PropertyInfo).SetValue(Component, val, null);
            }
        }
    }

    public class PropertyOrFieldAttribute : PropertyAttribute
    {
        public Type TargetType;
        public PropertyOrFieldAttribute()
            : this(typeof(object)) // everything
        {
        }

        public PropertyOrFieldAttribute(Type targetType)
        {
            TargetType = targetType;
        }
    }
}
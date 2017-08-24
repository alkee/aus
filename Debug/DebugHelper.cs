using UnityEngine;
using System;
using System.Reflection;

public static class DebugHelper
{
    [System.Diagnostics.Conditional("DEBUG")]
    public static void AllPulblicMemberFildsShouldNotNull<T>(T target, bool declaredOnly = true) where T : class
    {
        if (target == null) throw new ArgumentNullException("target");

        var bindings = BindingFlags.Public | BindingFlags.Instance;
        if (declaredOnly) bindings |= BindingFlags.DeclaredOnly;
        var fields = target.GetType().GetFields(bindings);
        foreach (var f in fields)
        {
            if (f.FieldType.IsValueType) continue;
            Debug.Assert(!f.GetValue(target).Equals(null), "member " + f.Name + " should not be null in " + target.GetType().Name);
        }

    }
}

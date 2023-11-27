using System;
using UnityEngine;

namespace aus.Property
{
    // 추가적으로 conditionalSourceField 에 ',' 로 연결해 (and 조건) 사용 가능
    // ** 주의 ** Range 등의 attribute 와 함께 사용할 수 없다.

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    { // ref http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/

        //The name of the bool field that will be in control
        public string ConditionalSourceField = "";
        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public bool HideInInspector;
        public bool Reversed;

        public ConditionalHideAttribute(string conditionalSourceField, bool hideThanDisable = false, bool reversed = false)
        {
            ConditionalSourceField = conditionalSourceField;
            Reversed = reversed;
            HideInInspector = hideThanDisable;
        }
    }
}
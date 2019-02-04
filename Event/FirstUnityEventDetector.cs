// https://bitbucket.org/alkee/aus

using UnityEngine;
using UnityEngine.Events;

namespace aus.Event
{
    // *CAUSION* this may not work if the target UnityEvent is changed somewhere else
    public class FirstUnityEventDetector : MonoBehaviour
    {
        [Property.PropertyOrField(typeof(UnityEvent))]
        public Property.ComponentMemberField TargetField;
        public UnityEvent OnFirstInvoke;

        void Start()
        {
            if (TargetField != null)
            {
                target = TargetField.GetValue() as UnityEvent;
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

}

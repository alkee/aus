// https://bitbucket.org/alkee/aus
using UnityEngine;
using UnityEngine.Events;

namespace aus.Event
{
    [RequireComponent(typeof(Collider))]
    public class TriggerDetector : MonoBehaviour
    {
        public Rigidbody Target;

        [Header("Events")]
        public UnityEvent OnEnterTrigger;
        public UnityEvent OnExitTrigger;

        private void Start()
        {
            if (GetComponent<Collider>().isTrigger == false)
            {
                Debug.LogErrorFormat("{0} has collider but not trigger. {1} component must on uee of trigger collider"
                    , name, GetType().Name);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (Target != null && other.gameObject == Target.gameObject)
            {
                OnEnterTrigger.Invoke();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (Target != null && other.gameObject == Target.gameObject)
            {
                OnExitTrigger.Invoke();
            }
        }
    }

}

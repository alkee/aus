// https://bitbucket.org/alkee/aus

using aus.Property;
using UnityEngine;
using UnityEngine.Events;


namespace aus.Event
{

    public class DistanceInRange : MonoBehaviour
    {
        public Transform Target;

        [MinMaxRange(0.0f, 100.0f, true)]
        public MinMaxRange EffectiveDistance;

        [Header("Events")]
        public UnityEvent OnEnter;
        public UnityEvent OnExit;

        public void ChangeTarget(Transform target)
        {
            Target = target;
        }

        void Start()
        {
        }

        private bool prevInRange = false;
        void Update()
        {
            if (Target == null) return;

            var distance = Vector3.Distance(Target.position, transform.position);
            if (EffectiveDistance.IsIn(distance))
            { // looking
                if (prevInRange == false)
                {
                    prevInRange = true;
                    OnEnter.Invoke();
                }
            }
            else if (prevInRange == true)
            {
                prevInRange = false;
                OnExit.Invoke();
            }

        }
    }
}

// https://bitbucket.org/alkee/aus

using UnityEngine;
using UnityEngine.Events;

namespace aus.Action
{
    public class ChasePosition : MonoBehaviour
    {
        public Transform ChasingTarget;
        public float ChasingSpeed = 1;
        [Tooltip("이 거리보다 먼 경우에만 chasing")]
        public float StopDistance = 0.5f; // TODO: editor 에서 stop distance gizmo 표시
        public Property.XyzBool FreezeAxis = new Property.XyzBool { Y = true };
        [Header("Events")]
        public UnityEvent OnDeparture;
        public UnityEvent OnArrival;

        public void ChangeChasingTarget(Transform target)
        {
            ChasingTarget = target;
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void OnDisable()
        {
            if (rb != null)
            {
                rb.velocity = Vector3.zero; // stop
            }
        }

        void Update()
        { // rigid body 를 이용하지 않는 경우
            if (ChasingTarget == null || rb != null) return;
            if (IsInStopDistance()) return;

            var direction = GetNormalizedTargetDirection();

            var dt = Time.deltaTime;
            transform.position += direction * dt * ChasingSpeed;
        }

        void FixedUpdate()
        { // rigid body 를 이용하는 경우
            if (ChasingTarget == null || rb == null) return;
            if (IsInStopDistance()) return;

            var direction = GetNormalizedTargetDirection();

            // same as rb.velocity = direction * ChasingSpeed;
            // but AddForce makes boundary force on collision
            rb.AddForce(direction * ChasingSpeed - rb.velocity, ForceMode.VelocityChange);
        }

        private Rigidbody rb = null;
        private bool moving = false;

        private bool IsInStopDistance()
        {
            var shoudStop = Vector3.Distance(transform.position, ChasingTarget.position) < StopDistance;
            if (shoudStop && moving)
            {
                moving = false;
                OnArrival.Invoke();
            }
            else if (shoudStop == false && moving == false)
            {
                moving = true;
                OnDeparture.Invoke();
            }
            return shoudStop;
        }

        private Vector3 GetNormalizedTargetDirection()
        {
            if (ChasingTarget == null) return Vector3.zero;
            var direction = ChasingTarget.position - transform.position;
            if (FreezeAxis.X) direction.x = 0.0f;
            if (FreezeAxis.Y) direction.y = 0.0f;
            if (FreezeAxis.Z) direction.z = 0.0f;
            if (direction == Vector3.zero) return Vector3.zero;
            direction.Normalize();
            return direction;
        }
    }
}

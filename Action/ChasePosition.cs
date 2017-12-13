// https://bitbucket.org/alkee/aus

using UnityEngine;

namespace aus.Action
{
    public class ChasePosition : MonoBehaviour
    {
        public Transform ChasingTarget;
        public float ChasingSpeed = 1;
        public Property.XyzBool FreezeAxis = new Property.XyzBool { Y = true };

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
        {
            if (ChasingTarget == null || rb != null) return;

            var direction = GetNormalizedTargetDirection();

            var dt = Time.deltaTime;
            transform.position += direction * dt * ChasingSpeed;
        }

        void FixedUpdate()
        {
            if (ChasingTarget == null || rb == null) return;

            var direction = GetNormalizedTargetDirection();

            // same as rb.velocity = direction * ChasingSpeed;
            // but AddForce makes boundary force on collision
            rb.AddForce(direction * ChasingSpeed - rb.velocity, ForceMode.VelocityChange);
        }

        private Rigidbody rb = null;

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

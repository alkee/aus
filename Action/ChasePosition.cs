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

        void Update()
        {
            if (ChasingTarget == null) return;

            var direction = ChasingTarget.position - transform.position;
            if (FreezeAxis.X) direction.x = 0.0f;
            if (FreezeAxis.Y) direction.y = 0.0f;
            if (FreezeAxis.Z) direction.z = 0.0f;
            if (direction == Vector3.zero) return;
            direction.Normalize();

            var dt = Time.deltaTime;
            transform.position += direction * dt * ChasingSpeed;
        }
    }
}

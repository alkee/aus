// https://bitbucket.org/alkee/aus
using UnityEngine;
using UnityEngine.EventSystems;

namespace aus.Action
{
    // modified by alkee
    // from https://forum.unity3d.com/threads/fly-cam-simple-cam-script.67042/
    public class FpsMotion : MonoBehaviour
    {
        /*
         * Based on Windex's flycam script found here: http://forum.unity3d.com/threads/fly-cam-simple-cam-script.67042/
         * C# conversion created by Ellandar
         * Improved camera made by LookForward
         * Modifications created by Angryboy
         * 1) Have to hold right-click to rotate
         * 2) Made variables public for testing/designer purposes
         * 3) Y-axis now locked (as if space was always being held)
         * 4) Space/Ctrl keys are used to raise/lower the camera(like jump/crouch of FPS games)
         *
         * Another Modification created by micah_3d
         * 1) adding an isColliding bool to allow camera to collide with world objects, terrain etc.
         */

        public float mouseSensitivity = 2.5f; // Mouse rotation sensitivity.
        public float mainSpeed = 10.0f; //regular speed
        public float shiftAdd = 5.5f; //multiplied by how long shift is held.  Basically running
        public float maxShift = 1000.0f; //Maximum speed when holdin gshift
        public float camSens = 0.25f; //How sensitive it with mouse

        // physics and camera scpecific parts are removed
        // because it's more simple and independent.
        // if you want some like physics, you may add other components for it.

        void Start()
        {
            rotationY = -transform.localEulerAngles.x; // to keep camera angles by editor
        }

        void Update()
        {
            if (EventSystem.current != null
                && EventSystem.current.IsPointerOverGameObject(1))
            {
                return; // see http://answers.unity3d.com/questions/784617/how-do-i-block-touch-events-from-propagating-throu.html
            }

            // Angryboy: Hold right-mouse button to rotate
            if (Input.GetMouseButtonDown(1))
            {
                isRotating = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                isRotating = false;
            }
            if (isRotating)
            {
                // Made by LookForward
                // Angryboy: Replaced min/max Y with numbers, not sure why we had variables in the first place
                float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
                rotationY += Input.GetAxis("Mouse Y") * mouseSensitivity;
                rotationY = Mathf.Clamp(rotationY, -90, 90);
                transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
            }

            //Keyboard commands
            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
                // Angryboy: Use these to ensure that Y-plane is affected by the shift key as well
                speedMultiplier = totalRun * shiftAdd * Time.deltaTime;
                speedMultiplier = Mathf.Clamp(speedMultiplier, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
                speedMultiplier = mainSpeed * Time.deltaTime; // Angryboy: More "correct" speed
            }

            p = p * Time.deltaTime;

            // Angryboy: Removed key-press requirement, now perma-locked to the Y plane
            Vector3 newPosition = transform.position;//If player wants to move on X and Z axis only
            transform.Translate(p, Camera.main.transform);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            newPosition.y = transform.position.y;

            if (Input.GetKey(KeyCode.Space))
            {
                newPosition.y += speedMultiplier;
            }
            if (Input.GetKey(KeyCode.LeftControl)) newPosition.y += -speedMultiplier;

            transform.position = newPosition;
        }

        private float totalRun = 1.0f;
        private bool isRotating = false; // Angryboy: Can be called by other things (e.g. UI) to see if camera is rotating
        private float speedMultiplier; // Angryboy: Used by Y axis to match the velocity on X/Z axis
        private float rotationY = 0.0f;

        private Vector3 GetBaseInput()
        { //returns the basic values, if it's 0 than it's not active.
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(KeyCode.W))
            {
                p_Velocity += new Vector3(0, 0, 1);
            }
            if (Input.GetKey(KeyCode.S))
            {
                p_Velocity += new Vector3(0, 0, -1);
            }
            if (Input.GetKey(KeyCode.A))
            {
                p_Velocity += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                p_Velocity += new Vector3(1, 0, 0);
            }
            return p_Velocity;
        }
    }
}


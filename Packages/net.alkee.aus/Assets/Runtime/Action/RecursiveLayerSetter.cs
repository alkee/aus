using UnityEngine;

namespace aus.Action
{
    public class RecursiveLayerSetter : MonoBehaviour
    {
        public void SetLayerAllChildren()
        {
            SetLayerAllChildren(gameObject.layer);
        }

        public void SetLayerAllChildren(string layerName)
        {
            SetLayerAllChildren(LayerMask.NameToLayer(layerName));
        }

        public void SetLayerAllChildren(int layer)
        {
            SetLayerRecursively(transform, layer);
        }

        public static void SetLayerRecursively(Transform transform, int layer)
        {
            transform.gameObject.layer = layer;
            foreach (Transform t in transform) SetLayerRecursively(t, layer);
        }
    }
}

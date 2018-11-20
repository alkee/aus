// https://bitbucket.org/alkee/aus

using System;
using System.Collections;
using UnityEngine;

namespace aus
{
    public static class Coroutines
    {
        public static IEnumerator Lerp(Color from, Color to, float duration, Action<Color> frame)
        {
            yield return null;
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                frame(Color.Lerp(from, to, elapsed / duration));
                yield return null;
            }
            frame(to);
        }

        public static IEnumerator Lerp(float from, float to, float duration, Action<float> frame)
        {
            yield return null;
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                frame(Mathf.Lerp(from, to, elapsed / duration));
                yield return null;
            }
            frame(to);
        }

        public static IEnumerator Lerp(Vector3 from, Vector3 to, float duration, Action<Vector3> frame)
        {
            yield return null;
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                frame(Vector3.Lerp(from, to, elapsed / duration));
                yield return null;
            }
            frame(to);
        }

        public static IEnumerator Lerp(Quaternion from, Quaternion to, float duration, Action<Quaternion> frame)
        {
            yield return null;
            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                frame(Quaternion.Lerp(from, to, elapsed / duration));
                yield return null;
            }
            frame(to);
        }

        public static IEnumerator WaitAndRun(float duration, System.Action frame)
        {
            yield return new WaitForSeconds(duration);
            frame();
        }

    }
}

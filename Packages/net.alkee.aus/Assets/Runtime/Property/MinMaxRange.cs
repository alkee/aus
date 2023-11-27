using UnityEngine;

namespace aus.Property
{
    // modified by alkee
    // from http://www.grapefruitgames.com/blog/2013/11/a-min-max-range-for-unity/

    // Unity에서 기본 제공하는 RangeAttribute 는 float 또는 int 에서만 사용할 수 있다.
    public class MinMaxRangeAttribute : PropertyAttribute
    {
        public float minLimit;
        public float maxLimit;
        public bool showSlider;

        public MinMaxRangeAttribute(float minLimit, float maxLimit, bool showSlider = false)
        {
            this.minLimit = minLimit;
            this.maxLimit = maxLimit;
            this.showSlider = showSlider;
        }
    }

    [System.Serializable]
    public class MinMaxRange
    {
        public float rangeStart;
        public float rangeEnd;

        public MinMaxRange()
        {
        }

        public MinMaxRange(float min, float max)
        {
            rangeStart = min;
            rangeEnd = max;
        }

        public float GetRandomValue()
        {
            return Random.Range(rangeStart, rangeEnd);
        }

        public bool IsIn(float value, bool include = false)
        {
            if (!include && value > rangeStart && value < rangeEnd) return true;
            else if (include && value >= rangeEnd && value <= rangeEnd) return true;
            return false;
        }
    }
}

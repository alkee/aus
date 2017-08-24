using UnityEngine;
using UnityEngine.UI;

using aus.Debugging;

namespace aus.Example
{
    public class Example : MonoBehaviour
    {
        public MinMaxRange MinMaxRangeValue;

        public GameObject SampleTarget1;
        public Text InitEventTriggerOutput;

        public void InitEventTriggerAwake(string value)
        {
            InitEventTriggerOutput.text = value;
        }

        public void InitEventTriggerStart(Text target)
        {
            target.text = "Started";
        }

        void Awake()
        {
            this.Should().NotHaveNullMemberFields();
        }

        void Start()
        {
        }
    }
}

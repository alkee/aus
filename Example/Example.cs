using UnityEngine;
using UnityEngine.UI;

using aus.Debugging;
using aus.Property;

namespace aus.Example
{
    public class Example : MonoBehaviour
    {
        [Header("Simple property and attributes")]
        [MinMaxRange(1, 10, true)]
        public MinMaxRange MinMaxRangeValue;
        public SceneField SelectedScene;

        [Header("ConditionalHide attribute")]
        public bool ShowConditional;
        [ConditionalHide("ShowConditional", true)]
        public int ConditionalValue;

        [Space(10)]
        [Header("References from scene")]
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

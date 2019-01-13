using UnityEngine;
using UnityEngine.UI;

using aus.Debugging;
using aus.Property;

namespace aus.Example
{
    public class Example : Singleton<Example>
    {
        [Header("Simple property and attributes")]
        [MinMaxRange(1, 10, true)]
        public MinMaxRange MinMaxRangeValue;
        public SceneField SelectedScene;
        [ReadOnly(true)]
        [Tooltip("You cannot modify this value while running by inspector")]
        public float RuntimeReadonly = 1.0f;
        public XyzBool XyzBoolean = new XyzBool { X = false, Y = true, Z = false };

        [Header("ConditionalHide attribute")]
        public bool ShowConditional;
        [ConditionalHide("ShowConditional", true)]
        public int ConditionalValue;

        [InspectorInvokable]
        public UnityEngine.Events.UnityEvent SomeEvent;

        [Header("ETC")]
        [ReadOnly]
        public string TestMD5 = Hash.Md5("Test");
        [ReadOnly]
        public string TestSHA1 = Hash.Sha1("Test");

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

            #region RollbackTransform
            var p = transform.position;
            var r = transform.rotation;
            using (new RollbackTransform(transform))
            { // push
                transform.position = Vector3.up * 15;
                transform.rotation = Quaternion.Euler(10, 50, 80);
            } // pop
            Debug.Assert(p == transform.position);
            Debug.Assert(r == transform.rotation);
            #endregion

            #region Singleton
            Example.Instance.RuntimeReadonly = 15; // access this class as singleton
            #endregion

            #region Coroutines
            StartCoroutine(Coroutines.WaitAndRun(1.0f // run after 1 seconds
                , () => // yield is not allowed in C# lambda(CS1621)
                {
                    var re = SampleTarget1.GetComponent<MeshRenderer>().material;
                    var beginColor = re.color;
                    StartCoroutine(Coroutines.Lerp(beginColor, Color.red, 3, (x) =>
                    {
                        re.color = x;
                    }));
                }));
            #endregion
        }
    }
}

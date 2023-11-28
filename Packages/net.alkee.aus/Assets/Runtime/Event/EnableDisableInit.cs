using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace aus.Event
{
    // scene 을 편집하다보면 GameObject 들을 active(enable, visible)/inactive(disable, invisible) 시켜가면서
    // 작업하게 되는데, 편집상태와 상관없이 실제 플레이 시에 설정되어야 하는 값을 스크립트로 셋팅하기 위함.
    // 특정상태(에디터 편집)에서 시작하고 싶다면 이 컴포넌트만 disable 하면 될 것.
    public class EnableDisableInit : MonoBehaviour
    {
        public List<GameObject> EnableTargets = new List<GameObject>();
        public List<GameObject> DisableTargets = new List<GameObject>();

        void Awake()
        {
            SetActive(EnableTargets, true);
            SetActive(DisableTargets, false);
        }

        private void SetActive(List<GameObject> targets, bool value)
        {
            foreach (var t in targets)
            {
                if (t == null) continue;
                Debug.Log("[EnableDisableInit] " + (value ? "Enabling " : "Disabling ") + t.name);
                var isChild // 자손 전부를 포함하지 않으므로 transform.IsChildOf 를 사용할 수 없다.
                    = transform.Cast<Transform>().Any((x) => x == t.transform);
                if (isChild == false)
                {
                    Debug.LogWarning("[EnableDisableInit] " + t.name + " is not a child of " + name);
                    continue;
                }

                t.gameObject.SetActive(value);
            }
        }
    }
}

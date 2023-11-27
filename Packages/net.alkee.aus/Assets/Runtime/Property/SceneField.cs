using UnityEngine;
using UnityEngine.SceneManagement;

namespace aus.Property
{
    // ref: https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html
    [System.Serializable]
    public class SceneField
    {
        [SerializeField]
        private Object m_SceneAsset;
        [SerializeField]
        private string m_SceneName = "";

        /// <summary>
        /// implicit operator string 이 있어 직접 이 멤버에 접근할 필요는 없을 것
        /// </summary>
        public string SceneName
        {
            get
            {
                return m_SceneName;
            }
        }

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }
    }
}

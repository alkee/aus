// https://bitbucket.org/alkee/aus
using UnityEngine;
using UnityEngine.SceneManagement;

namespace aus.InterScene
{
    public class Dependency : MonoBehaviour
    {
        [Tooltip("loads additive scene automatically")]
        public Property.SceneField DependentScene;

        void Awake()
        {
            if (enabled == false) return;
            Debug.Assert(DependentScene != null && string.IsNullOrEmpty(DependentScene.SceneName) == false
                , "Dependent scene is not set");

            var thisScene = gameObject.scene;
            var count = SceneManager.sceneCount;
            // TODO: duplicated scene name ; AssetDatabase is only available in EDITOR
            var dependent = SceneManager.GetSceneByName(DependentScene.SceneName);

            // if the dependent is already loading somewhere else,
            //    isLoaded could be false BUT dependent.IsValid() is TRUE.
            if (dependent.IsValid() || dependent.isLoaded) return; // prevent duplication

            // TODO: asyncronous load option
            SceneManager.LoadScene(DependentScene.SceneName, LoadSceneMode.Additive);
        }

        // just to use 'enabled' in inspector
        void Start()
        {
        }
    }
}

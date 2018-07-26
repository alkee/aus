// https://bitbucket.org/alkee/aus
using UnityEngine;
using UnityEngine.SceneManagement;

namespace aus.InterScene
{
    public class Dependency : MonoBehaviour
    {
        [Tooltip("loads additive scene automatically")]
        public Property.SceneField DependentScene;

        // TODO: asyncronous load option

        void Start()
        {
            Debug.Assert(DependentScene != null && string.IsNullOrEmpty(DependentScene.SceneName) == false
                , "Dependent scene is not set");
            Debug.Assert(DependentScene.IsOnBuildList
                , "you cannot set the scene which is not on the build list");

            var thisScene = gameObject.scene;
            var count = SceneManager.sceneCount;
            var dependent = SceneManager.GetSceneByPath(DependentScene.ScenePath);

            // if the dependent is already loading somewhere else,
            //    isLoaded could be false BUT dependent.IsValid() is TRUE.
            if (dependent.IsValid() || dependent.isLoaded) return; // prevent duplication

            SceneManager.LoadScene(DependentScene.BuildIndex, LoadSceneMode.Additive);
        }
    }
}

// https://bitbucket.org/alkee/aus
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
#endif

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

#if UNITY_EDITOR

    [CustomPropertyDrawer(typeof(SceneField))]
    public class SceneFieldPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
        {
            if (extend) _position.height /= (1 + ERROR_BOX_HEIGHT_RATIO);

            EditorGUI.BeginProperty(_position, GUIContent.none, _property);
            SerializedProperty sceneAsset = _property.FindPropertyRelative("m_SceneAsset");
            SerializedProperty sceneName = _property.FindPropertyRelative("m_SceneName");
            _position = EditorGUI.PrefixLabel(_position, GUIUtility.GetControlID(FocusType.Passive), _label);
            if (sceneAsset != null)
            {
                sceneAsset.objectReferenceValue = EditorGUI.ObjectField(_position, sceneAsset.objectReferenceValue, typeof(SceneAsset), false);
                if (sceneAsset.objectReferenceValue != null)
                {
                    sceneName.stringValue = (sceneAsset.objectReferenceValue as SceneAsset).name;
                    var path = AssetDatabase.GetAssetPath(sceneAsset.objectReferenceValue);
                    if (EditorBuildSettings.scenes.Any(x => x.path == path) == false)
                    { // not in the build settins
                        extend = true;
                        _position.y += _position.height;
                        _position.xMax -= 18; // select icon
                        _position.height *= ERROR_BOX_HEIGHT_RATIO;
                        EditorGUI.HelpBox(_position, "this scene is not in the build-list", MessageType.Error);
                    }
                    else
                    {
                        extend = false;
                    }
                }
            }
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var defualtHeight = base.GetPropertyHeight(property, label);
            if (extend == false) return defualtHeight;

            return defualtHeight * (1 + ERROR_BOX_HEIGHT_RATIO);
        }

        private const float ERROR_BOX_HEIGHT_RATIO = 1.5f;
        private bool extend;

    }

#endif
}

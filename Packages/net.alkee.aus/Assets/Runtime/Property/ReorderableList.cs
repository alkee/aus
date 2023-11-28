using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace aus.Property
{
    public abstract class ReorderableList<T>
    {
        public List<T> members = new List<T>();
    }

    // CustomPropertyDrawer 가 generic 을 지원하지 않기 때문에 지원하는 type 들을 개별적으로
    // 만들어줘야 한다. 같은 방식으로 사용자 정의 ReorderableList 를 만들 수 있다.

    // [Serializable] public class BLABLA : ReorderableList<T> {}
    // #if UNITY_EDITOR [CustomPropertyDrawer(typeof(BLABLA))] public class BLABLA_Drawer : ReorderableListDrawer #end if

    // CustomPropertyDrawer 와 손쉬운 확인을 위해 알파벳 순 정렬된 순서로 선언
    [Serializable] public class Animators : ReorderableList<Animator> { }
    [Serializable] public class Colors : ReorderableList<Color> { }
    [Serializable] public class Floats : ReorderableList<float> { }
    [Serializable] public class GameObjects : ReorderableList<GameObject> { }
    [Serializable] public class Int32s : ReorderableList<Int32> { }
    [Serializable] public class MeshRenderers : ReorderableList<MeshRenderer> { }
    [Serializable] public class SkinnedMeshRenderers : ReorderableList<SkinnedMeshRenderer> { }
    [Serializable] public class Strings : ReorderableList<string> { }
    [Serializable] public class Transforms : ReorderableList<Transform> { }
    [Serializable] public class Vector3s : ReorderableList<Vector3> { }

}
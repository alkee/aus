## Project AUS

__A__ lkee's __U__ nity __S__ nippets. [Unity](https://unity3d.com) 에서 사용되는
유용하고 작은 코드 조각들 모음. [bitbucket](https://bitbucket.org/alkee/aus)
프로젝트에서 옮겨옴.

### Features

대부분 단순(simple)하고 독립적인(하나의 파일이 하나의 기능) unity component
들 모음으로 비 개발자가 unity 로 간단한 prototype 을 하고자 하는 경우를 상정해
game object 에 component 들을 조합해 원하는 동작을 할 수 있도록.

### Getting started

 1. `Package Manager` 열기
 2. [`+` icon 선택하고 `Add package from git url`](https://docs.unity3d.com/Manual/upm-ui-giturl.html)
 3. 입력 경로에 `https://github.com/alkee/aus.git?path=/Packages/net.alkee.aus`
    사용.
    

특정한 tag 나 branch 를 이용하고자 하는 경우 뒤에 `#` 과 함께 이름을 붙임.
예) `https://github.com/alkee/aus.git?path=/Packages/net.alkee.aus#v0.1`


### Namespaces

 * `Event` : 특정 상황에 발생하는 event 를 이용해 동작(`Action` namespace)을
    수행할 수 있도록 다양한 상황(triggers)을 제공.
 * `Extension` : Unity 나 C# class 들을 확장하는 extension methods 제공
 * `Geometry` : 3D mesh, point cloud 관련
 * `Property` : Unity editor 의 Inspector 창에서 편리하게 사용할 수 있는 editor
    확장 기능 제공


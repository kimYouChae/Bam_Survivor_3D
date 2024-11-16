using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    private static T instance;

    public static T Instance 
    {
        get 
        {
            // 인스턴스가 null 일 때
            if(instance == null) 
            {
                // 찾기
                instance = (T)FindAnyObjectByType(typeof(T));

                // 찾았는데 null 이면 ?
                if(instance == null) 
                {
                    // T 타입으로 오브젝트 생성
                    GameObject _obj = new GameObject(typeof(T).Name , typeof(T));
                    instance = _obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // dontDestroy 세팅 
        F_SetUpDontDestroy();

        // 각 Manager가 Awake가 필요한 경우 실행
        Singleton_Awake();
    }

    protected abstract void Singleton_Awake();

    private void F_SetUpDontDestroy() 
    {
        // Managers
        //  ㄴ UiManager
        //  ㄴ SkillManager 처럼 manager가 하위에 있을수도 있다.

        // 내가 하위에 있는 오브젝트라면
        if (transform.parent != null && transform.root != null)
        {
            // 사우이 부모를 dontDestroy
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        // 내가 상위 오브젝트면 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}

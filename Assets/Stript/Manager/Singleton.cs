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
            // �ν��Ͻ��� null �� ��
            if(instance == null) 
            {
                // ã��
                instance = (T)FindAnyObjectByType(typeof(T));

                // ã�Ҵµ� null �̸� ?
                if(instance == null) 
                {
                    // T Ÿ������ ������Ʈ ����
                    GameObject _obj = new GameObject(typeof(T).Name , typeof(T));
                    instance = _obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // dontDestroy ���� 
        F_SetUpDontDestroy();

        // �� Manager�� Awake�� �ʿ��� ��� ����
        Singleton_Awake();
    }

    protected abstract void Singleton_Awake();

    private void F_SetUpDontDestroy() 
    {
        // Managers
        //  �� UiManager
        //  �� SkillManager ó�� manager�� ������ �������� �ִ�.

        // ���� ������ �ִ� ������Ʈ���
        if (transform.parent != null && transform.root != null)
        {
            // ����� �θ� dontDestroy
            DontDestroyOnLoad(this.transform.root.gameObject);
        }
        // ���� ���� ������Ʈ�� 
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

}

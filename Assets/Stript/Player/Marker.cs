using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    /// <summary>
    /// marker Prefab�� ���ִ� ��ũ��Ʈ
    /// </summary>

    [Header("===State===")]
    [SerializeField]
    private MarkerState _markerState;

    [Header("===HP Bar===")]
    [SerializeField]
    private Slider _markerHpBar;

    [Header("===�ѱ� Transfrom===")]
    [SerializeField]
    private Transform _markerMuzzleTrs;

    // ������Ƽ
    public MarkerState markerState => _markerState;
    public Slider markerHpBar => _markerHpBar;
    public Transform markerMuzzleTrs => _markerMuzzleTrs;

    private void Start()
    {
        // ���� ��� �ڷ�ƾ
        StartCoroutine(IE_MarkerUseShield());

        // bullet �߻� �ڷ�ƾ
        StartCoroutine(IE_MarkerShootBullet());
    }

    IEnumerator IE_MarkerUseShield()
    {
        // 0.02f��ŭ wait, ������Ʈ ȿ�� 
        // ���尡 0.5f �ð� ������ �ΰ� Ŀ������
        // -> �� �ð����ݸ��� ���� ��� ��������Ʈ�� �����ؾ��� ( ���� ȿ�� ��ø )

        while (true) 
        {
        
            // shield ��Ÿ�ӵ��� ��ٸ���
            yield return new WaitForSeconds
                (_markerState.markerShieldCoolTime);

            //  ���� ��������Ʈ ���� 
            PlayerManager.instance.markerShieldController.del_shieldCreate(this);

        }
    }

    IEnumerator IE_MarkerShootBullet()
    {
        while (true)
        {
            // shoot ��Ÿ�ӵ��� ��ٸ���
            yield return new WaitForSeconds
                (_markerState.markerBulletShootCoolTime);

            // �Ѿ� �߻� �Լ� ����
            PlayerManager.instance.markerBulletController.F_BasicBulletShoot(_markerMuzzleTrs);
        }
    }

    // �ӽ� ) radious��ŭ draw
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( gameObject.transform.position
            , 7f);
    }

    // ���� Hp����
    public void F_UpdateHP(float HP = 0)
    {
        // hp ���� 
        _markerState.markerHp += HP;

        // max ������ max�� 
        if (_markerState.markerHp > _markerState.markerMaxHp)
            _markerState.markerHp = _markerState.markerMaxHp;

        
    }

}
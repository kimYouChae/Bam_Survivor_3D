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
        while (true) 
        {
            // shield ��Ÿ�ӵ��� ��ٸ���
            yield return new WaitForSeconds
                (_markerState.markerShieldCoolTime);

            //  ���� ��������Ʈ ���� 
            PlayerManager.instance.markerShieldController.del_markerShieldUse(this.gameObject.transform);
        }
    }

    IEnumerator IE_MarkerShootBullet()
    {
        while (true)
        {
            // shoot ��Ÿ�ӵ��� ��ٸ���
            yield return new WaitForSeconds
                (_markerState.markerShootCoolTime);

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
}
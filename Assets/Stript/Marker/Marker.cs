using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    /// <summary>
    /// marker Prefab에 들어가있는 스크립트
    /// </summary>

    [Header("===State===")]
    [SerializeField]
    private MarkerState _markerState;

    [Header("===HP Bar===")]
    [SerializeField]
    private Slider _markerHpBar;

    [Header("===총구 Transfrom===")]
    [SerializeField]
    private Transform _markerMuzzleTrs;

    // 프로퍼티
    public MarkerState markerState => _markerState;
    public Slider markerHpBar => _markerHpBar;
    public Transform markerMuzzleTrs => _markerMuzzleTrs;

    private void Start()
    {
        // 쉴드 사용 코루틴
        StartCoroutine(IE_MarkerUseShield());

        // bullet 발사 코루틴
        StartCoroutine(IE_MarkerShootBullet());
    }

    IEnumerator IE_MarkerUseShield()
    {
        while (true) 
        {
            // shield 쿨타임동안 기다리기
            yield return new WaitForSeconds
                (_markerState.markerShieldCoolTime);

            //  쉴드 델리게이트 실행 
            PlayerManager.instance.markerShieldController.del_markerShieldUse(this.gameObject.transform);
        }
    }

    IEnumerator IE_MarkerShootBullet()
    {
        while (true)
        {
            // shoot 쿨타임동안 기다리기
            yield return new WaitForSeconds
                (_markerState.markerShootCoolTime);

            // 총알 발사 함수 실행
            PlayerManager.instance.markerBulletController.F_BasicBulletShoot(_markerMuzzleTrs);
        }
    }

    // 임시 ) radious만큼 draw
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere( gameObject.transform.position
            , 7f);
    }
}
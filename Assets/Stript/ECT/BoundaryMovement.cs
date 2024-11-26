using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryMovement : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _boundaryParent;

    public void F_SettlingPlayer(GameObject v_player)
    {
        _player = v_player;
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            _boundaryParent.transform.position = _player.transform.position + new Vector3(0,0,-0.6f);
        }
    }
}

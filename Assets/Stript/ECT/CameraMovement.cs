using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _camera;

    public void F_SettlingPlayer(GameObject v_player) 
    { 
        _player = v_player;
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            _camera.transform.position = _player.transform.position + new Vector3(0, 9.9f, 0);
        }
    }
}

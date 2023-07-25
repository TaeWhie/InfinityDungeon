using UnityEngine;
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 _cameraPosition;
    [SerializeField] private float _cameraMoveSpeed;
    [SerializeField] private Player _player;
    private Vector3 _mTargetPosition = Vector3.zero;
    void Update()
    {
        if (_player.close && _player.destination!=null)
        {
            _mTargetPosition = (_player.destination.transform.position +
                                _player.transform.position)/2;
        }
        else
        {
            _mTargetPosition = _player.nowPos;
        }
        transform.position = Vector3.Lerp(transform.position, _mTargetPosition + _cameraPosition,
                                  Time.deltaTime * _cameraMoveSpeed);
    }
}

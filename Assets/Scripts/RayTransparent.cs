using System.Collections.Generic;
using UnityEngine;

public class RayTransparent : MonoBehaviour
{
    private int _layerMask;
    [SerializeField] private Material[] _dungeon1Mat = new Material[2];
    private List<GameObject> _hitedObj=new();
    private Player _player;
    private void Start()
    {
        _player = GameManager.Instance.ReturnPlayer().GetComponent<Player>();
    }
    void Update()
    {
        float maxDistance = Vector3.Distance(transform.position,GameManager.Instance.ReturnPlayer().transform.position);
        RaycastHit[] hit = null;
        RaycastHit[] hit2 = null;
        _layerMask = 1 << LayerMask.NameToLayer("Obstacle");
        
        hit= Physics.RaycastAll(transform.position,_player.transform.position-transform.position,maxDistance,_layerMask);
        if (_player.destination != null&&_player.close)
        {
            hit2 = Physics.RaycastAll(transform.position ,_player.destination.transform.position-transform.position,
                maxDistance, _layerMask);
        }
        foreach (GameObject obj in _hitedObj)
        {
            obj.GetComponent<MeshRenderer>().material = _dungeon1Mat[0];
        }
        _hitedObj.Clear();
        if (hit != null)
        {
            foreach (RaycastHit oneHit in hit)
            {
                if (oneHit.collider.transform.position.z < _player.transform.position.z)
                {
                    oneHit.collider.GetComponent<MeshRenderer>().material = _dungeon1Mat[1];
                    _hitedObj.Add(oneHit.collider.gameObject);
                }
            }
        }
        if (hit2 != null)
        {
            foreach (RaycastHit oneHit in hit2)
            {
                if (oneHit.collider.transform.position.z < _player.transform.position.z)
                {
                    oneHit.collider.GetComponent<MeshRenderer>().material = _dungeon1Mat[1];
                    _hitedObj.Add(oneHit.collider.gameObject);
                }
            }
        }
    }
}

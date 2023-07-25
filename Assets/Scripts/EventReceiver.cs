using UnityEngine;
public class EventReceiver : MonoBehaviour
{
    private ObjectStat _obj;
    private AtkState _atkState;
    private void Awake()
    {
        _obj = gameObject.GetComponent<ObjectStat>();
    }
    void Attack()
    {
        _atkState = _obj.stateMachine.ReturnCurrentState() as AtkState;
        if (_atkState != null)
        {
            _atkState = (AtkState)_obj.stateMachine.ReturnCurrentState();
            _atkState.Attack();
        }
    }
    void StartAni()
    {
        _obj.stateMachine.ReturnCurrentState().StartAni();
    }
    void EndAni()
    {
        _obj.stateMachine.ReturnCurrentState().EndAni();
    }
   
}

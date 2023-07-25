using UnityEngine;
public class MoveState : IState
{
    private float _extraRotationSpeed = 5f;
    public MoveState(ObjectStat obj,int ID) : base(obj,ID)
    {
        animStateID = ID;
    }
    public override void StateUpdate()
    {
        currentGameObject.navMeshAgent.SetDestination(currentGameObject.destination.transform.position);
        if (currentGameObject.hp.Value <= 0 && currentGameObject.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            currentGameObject.stateMachine.ChangeState(new DeadState(currentGameObject, 2));
        }
        else if (currentGameObject.close)
        {
            currentGameObject.stateMachine.ChangeState(currentGameObject.comboList[currentGameObject.combo]);
        }
    }
    public override void StateFixedUpdate()
    {
        Vector3 lookrotation = currentGameObject.navMeshAgent.steeringTarget-currentGameObject.nowPos;
        if (lookrotation != Vector3.zero)
        {
            currentGameObject.transform.rotation = Quaternion.Slerp(currentGameObject.transform.rotation,
                Quaternion.LookRotation(lookrotation), _extraRotationSpeed * Time.deltaTime);
        }
    }
    public override void OnEnter(){}
    public override void OnExit(){}
    public override void StartAni(){}
    public override void EndAni(){}
}

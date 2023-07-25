using UnityEngine;
using UnityEngine.AI;
public class IdleState : IState
{
    public IdleState(ObjectStat obj,int ID) : base(obj,ID)
    {
        animStateID = ID;
    }
    public override void StateUpdate()
    {
        if (currentGameObject.hp.Value <= 0 )//&&currentGameObject.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            currentGameObject.stateMachine.ChangeState(new DeadState(currentGameObject, 2));
        }
        else
        {
            if(currentGameObject.destination!=null)
            {
                if (currentGameObject.close)
                {
                    currentGameObject.stateMachine.ChangeState(currentGameObject.comboList[currentGameObject.combo]);
                }
                else
                {
                    currentGameObject.stateMachine.ChangeState(new MoveState(currentGameObject, 1));
                }
            }
        }
    }
    public override void OnEnter()
    {
        currentGameObject.navMeshAgent.SetDestination(currentGameObject.nowPos);
        currentGameObject.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        currentGameObject.navMeshAgent.avoidancePriority = (int)ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    }
    public override void StateFixedUpdate(){}
    public override void OnExit() {}
    public override void StartAni(){}
    public override void EndAni(){}
}

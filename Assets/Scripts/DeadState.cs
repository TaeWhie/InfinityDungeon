using UnityEngine;
using UnityEngine.AI;
public class DeadState : IState
{
    private Enemy _enemy;
    public DeadState(ObjectStat obj,int ID) : base(obj,ID)
    {
        animStateID = ID;
    }
    public override void OnEnter()
    { 
        currentGameObject.navMeshAgent.SetDestination(currentGameObject.nowPos);
        currentGameObject.navMeshAgent.avoidancePriority = (int)ObstacleAvoidanceType.NoObstacleAvoidance;
        currentGameObject.gameObject.GetComponent<CapsuleCollider>().enabled = false;
    }
    public override void EndAni()
    {
        _enemy=currentGameObject as Enemy;
        if (_enemy != null)
        {
            _enemy = (Enemy)currentGameObject;
        }
        _enemy.Die();
    }
    
    public override void StateFixedUpdate(){}
    public override void StateUpdate(){}
    public override void OnExit(){}
    public override void StartAni(){}
}

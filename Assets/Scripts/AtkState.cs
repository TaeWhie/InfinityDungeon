using System.Collections.Generic;
using UnityEngine;
public abstract class AtkState : IState
{
    protected List<GameObject> targets=new();
    protected int randomMin = 0;
    protected int randomMax = 0;
    protected float additionalDamage = 0;
    protected int attackID;
    
    public abstract void SetTarget();
    public AtkState(ObjectStat obj,int ID) : base(obj,ID)
    {
        attackID = ID;
    }
    public void TakeDamage(int Ranmin,int Ranmax)
    {
        if (targets != null)
        {
            foreach (GameObject target in targets)
            {
                int ranNum = Random.Range(Ranmin, Ranmax);
                Vector3 newpos = target.transform.position;
                Vector3 ranVec = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.3f, 0.3f),
                    Random.Range(-0.2f, 0.2f));
                GameObject effect = GameManager.Instance.effectpool.Get(currentGameObject.hitEffectID);
                
                newpos.y += 0.5f;
                newpos += ranVec;
                ranVec = new Vector3(ranNum, ranNum, ranNum);
                effect.transform.position = newpos;
                effect.transform.localScale = ranVec;
                currentGameObject.totalDamage = currentGameObject.stat.attack + additionalDamage - Random.Range(0, 5);
                target.GetComponent<ObjectStat>().hp.Value -= currentGameObject.totalDamage;
            }
        }
    }
    public override void StateUpdate()
    {
        if (currentGameObject.hp.Value <= 0 && currentGameObject.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            currentGameObject.stateMachine.ChangeState(new DeadState(currentGameObject, 2));
        }
        if (currentGameObject.comboList.Count <= currentGameObject.combo)
        {
            currentGameObject.combo = 0;
        }
    }
    public override void OnEnter()
    {
        currentGameObject.transform.LookAt(currentGameObject.destination.transform);
        currentGameObject.navMeshAgent.SetDestination(currentGameObject.nowPos);
        currentGameObject.anim.speed = currentGameObject.stat.attackSpeed;
        animStateID = attackID;
        if (targets != null)
        {
            targets.Clear();
        }
    }
    public override void OnExit()
    {
        currentGameObject.anim.speed = 1;
        if (currentGameObject.destination.GetComponent<ObjectStat>().stateMachine.ReturnCurrentState() is DeadState)
        {
            currentGameObject.destination = null;
        }
    }
    public virtual void Attack()
    {
        if (currentGameObject.close)
        {
            SetTarget();
            TakeDamage(randomMin,randomMax);
        }
    }
    public override void StartAni()
    {
        if (currentGameObject.stateMachine.ReturnCurrentState() is not DeadState)
        {
            currentGameObject.combo += 1;
            currentGameObject.attackCount.Value += 1;
        }
    }
    public override void EndAni()
    {
        currentGameObject.stateMachine.ChangeState(new IdleState(currentGameObject, 0));
    }

    public override void StateFixedUpdate() { }
}

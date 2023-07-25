using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UniRx;

[RequireComponent(typeof(EventReceiver))]
public abstract class ObjectStat : MonoBehaviour
{ 
    public float totalDamage;
    public float shortestDistance;
    public int targetLayer;
    public int attackSoundID;
    public int combo;
    public bool close;
    public Stat stat;
    public ReactiveProperty<float> hp;
    public NavMeshAgent navMeshAgent;
    public GameObject destination;
    public StateMachine stateMachine=new();
    public Vector3 nowPos;
    public Animator anim;
    public List<AtkState> comboList = new ();
    public int hitEffectID;
    public ReactiveProperty<int> attackCount;
    public Dictionary<string,AttackGimic> attackGimic=new ();
    
    protected float prevHp;
    protected List<string> chartRead=new();
    public abstract void SetStat();
    public abstract void SetCombo();
    public virtual void OnAwake()
    {
        anim = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        SetStat();
    }
    public virtual void OnStart()
    { 
        SetCombo();
        navMeshAgent.updateRotation = false;
        prevHp = hp.Value;
        stateMachine.ChangeState(new IdleState(this,0));
    }
    public virtual void OnUpdate()
    {
        if (hp.Value <= 0)
        {
            hp.Value = 0;
        }
        if (hp.Value >= stat.maxHp)
        {
            hp.Value = stat.maxHp;
        }
        if (destination != null && stat.attackRange >= shortestDistance&&destination.GetComponent<ObjectStat>().stateMachine.ReturnCurrentState() is not DeadState)
        {
            RaycastHit hit;
            
            if(Physics.Raycast(nowPos,
                   (destination.transform.position - nowPos).normalized,out hit,shortestDistance,(-1) - (1<<gameObject.layer)))
            {
                if (hit.collider != null && hit.collider.gameObject.layer == targetLayer
                                        && hit.collider.gameObject.GetComponent<ObjectStat>().stateMachine.ReturnCurrentState() is not DeadState)
               {
                   close = true;
               }
               else
               {
                   close = false;
               }
            }
        }
        else
        {
            close = false;
        }
    }
    private void OnDisable()
    {
        destination = null;
    }
    public virtual void OnFixedUpdate(){}
}

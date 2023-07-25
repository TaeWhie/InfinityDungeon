using UnityEngine;

public class RangeAtkState : AtkState
{
    public float viewAngle=140;
    public float attackDistance;
    public RangeAtkState(ObjectStat obj,int ID,float plusDamage) : base(obj,ID)
    {
        additionalDamage = plusDamage;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        attackDistance = currentGameObject.stat.rangeAttackRange;
        randomMin = 6;
        randomMax = 9;
    }
    public override void OnExit()
    {
        base.OnExit();
        attackDistance = currentGameObject.stat.attackRange;
    }
    public override void SetTarget()
    {
        FindVisibleTargets();
    }
    public override void Attack()
    {
        base.Attack();
        GameObject sound = GameManager.Instance.soundpool.Get(1);
        sound.transform.position = currentGameObject.destination.transform.position;
    }
    public void FindVisibleTargets()
    {
        Collider[] colTargets = Physics.OverlapSphere(currentGameObject.transform.position, attackDistance,1<<currentGameObject.targetLayer);
        RaycastHit hit;
        
        foreach (Collider target in colTargets)
        {
            Vector3 dirToTarget = (target.transform.position - currentGameObject.transform.position).normalized;
            
            if (Vector3.Dot(currentGameObject.transform.forward, dirToTarget) > Mathf.Cos((viewAngle / 2) * Mathf.Deg2Rad))
            {
                float distToTarget = Vector3.Distance(currentGameObject.transform.position, target.transform.position);
                if (Physics.Raycast(currentGameObject.transform.position, dirToTarget,out hit,distToTarget ))
                {
                    if (hit.collider.gameObject.layer == currentGameObject.targetLayer)
                    {
                        targets.Add(target.gameObject);
                    }
                }
            }    
        }
    }
}

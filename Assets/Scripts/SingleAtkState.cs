using UnityEngine;
public class SingleAtkState : AtkState
{
    public SingleAtkState(ObjectStat obj,int ID) : base(obj,ID){}
    public override void OnEnter()
    {
        base.OnEnter();
        randomMin = 3;
        randomMax = 6;
    }
    public override void SetTarget()
    {
        if (currentGameObject.destination.GetComponent<ObjectStat>().stateMachine.ReturnCurrentState() is not DeadState)
        {
            targets.Add(currentGameObject.destination);
        }
    }
    public override void Attack()
    {
        base.Attack();
        GameObject sound = GameManager.Instance.soundpool.Get(currentGameObject.attackSoundID);
        sound.transform.position = currentGameObject.destination.transform.position;
    }
}

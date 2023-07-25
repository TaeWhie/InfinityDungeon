using UnityEngine;

public class DrainGimic : AttackGimic
{
    public DrainGimic(int gimicCount,int effectID ,ObjectStat objStat) : base(gimicCount,effectID,objStat){}
    public override void DoGimic()
    {
        if (GameManager.Instance.spawner._waveStart)
        {
            if (objStat.hp.Value <= objStat.stat.maxHp - objStat.stat.attack)
            {
                objStat.hp.Value += objStat.stat.attack;
            }
            else
            {
                objStat.hp.Value = objStat.stat.maxHp;
            }
            
            GameObject effect = GameManager.Instance.effectpool.Get(effectID);
            GameManager.Instance.soundpool.Get(4);
            if (effect.GetComponent<AttachGameObject>().target == null)
            {
                effect.GetComponent<AttachGameObject>().target = GameManager.Instance.ReturnPlayer().transform;
            }
        }
    }
}

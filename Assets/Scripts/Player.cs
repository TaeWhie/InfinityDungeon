using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Player : ObjectStat
{
    public int[] upgradeMoney = new int[(int)Upgrade.Count];
    public TrailRenderer swordLine;
    public ReactiveProperty<int>[] upgradLevel = new ReactiveProperty<int>[(int)Upgrade.Count];
    public List<float> upgradeValue;
    private enum PlayerMode
    {
        Start,SwordMan,BowMan,Magician,Count//차트에서 맨위 한칸을 빼기위해 Start삽입
    }
    public enum Upgrade
    {
        Attack,AttackSpeed,SkillRange,Count
    }
    public override void OnAwake()
    {
        base.OnAwake();
        targetLayer = LayerMask.NameToLayer("Enemy");
    }
    public override void OnStart()
    {
        base.OnStart();
        hp.Value = stat.maxHp;
        attackGimic.Add("DrainAttack",new DrainGimic(3, 2,this));
        attackGimic["DrainAttack"].DoGimic();
        upgradLevel[(int)Upgrade.Attack].Where(x=> x!=0).Subscribe(data=>
        {
            stat.attack += upgradeValue[0];
        });
        upgradLevel[(int)Upgrade.AttackSpeed].Where(x=> x!=0).Subscribe(data=>
        {
            stat.attackSpeed += upgradeValue[1];
        });
        upgradLevel[(int)Upgrade.SkillRange].Where(x=> x!=0).Subscribe(data=>
        {
            stat.rangeAttackRange += upgradeValue[2];
        });
    }

    public override void OnUpdate()
    {
        nowPos = transform.position;
        shortestDistance = 100;
        anim.SetInteger("StateID",stateMachine.ReturnCurrentState().animStateID);
        for (int i = 0; i < GameManager.Instance.enemypool.prefabs.Length; i++)
        {
            for (int j = 0; j < GameManager.Instance.enemypool.pools[i].Count; j++)
            {
                float distance = Vector3.Distance(transform.position, GameManager.Instance.enemypool.pools[i][j].transform.position);
                if (shortestDistance > distance&&GameManager.Instance.enemypool.pools[i][j].gameObject.GetComponent<ObjectStat>().hp.Value>0)
                {
                    shortestDistance = distance;
                    destination = GameManager.Instance.enemypool.pools[i][j];
                }
            }
        }
        swordLine.enabled = close ? true : false;
        stateMachine.Update();
        base.OnUpdate();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        stateMachine.FixedUpdate();
    }
    public override void SetStat()
    {
        chartRead = GameManager.Instance.dataManager.ReadRow((int)PlayerMode.SwordMan, chartRead, (int)DataManager.ChartName.PlayerChart,2);
        stat = new Stat(chartRead);
    }
    public override void SetCombo()
    {  
        comboList.Add(new SingleAtkState(this,3));
        comboList.Add(new RangeAtkState(this,4,10));
    }
}

using TMPro;
using UniRx;
using UnityEngine;
public class Enemy : ObjectStat
{
    public int giveMoney = 50;
    public int enemyID = 1;
    public override void OnAwake()
    {
        base.OnAwake();
        targetLayer = LayerMask.NameToLayer("Player");
    } 
    public override void OnStart()
    {
        base.OnStart();
        attackCount.Value = 0;
        stat.maxHp = stat.maxHp * (1 + 0.5f * GameManager.Instance.resetCount);
        stat.attack = stat.attack * (1 + 0.2f * GameManager.Instance.resetCount);
        stat.attackSpeed = stat.attackSpeed * (1 + 0.2f * GameManager.Instance.resetCount);

        hp.Subscribe(data =>
        {
            if (prevHp > data)
            {   //나중에 클래스로 뺄것
                GameObject damageUI=GameManager.Instance.effectpool.Get(3);
                Vector3 uiPosition = Camera.main.WorldToScreenPoint(transform.position);
                Vector3 ranVec = new Vector3(Random.Range(-35, 35), Random.Range(-35, 35), 0);
                FloatingText floatingText = damageUI.gameObject.transform.GetComponent<FloatingText>();
                
                damageUI.transform.position = uiPosition+ranVec;
                damageUI.transform.SetParent(GameManager.Instance.uiManager.canvas.gameObject.transform);
                floatingText.floatTextPrint = damageUI.GetComponent<TextMeshProUGUI>();
                floatingText.SetText(GameManager.Instance.ReturnPlayer().GetComponent<ObjectStat>().totalDamage.ToString());
            }
            else
            {
                //heal?
            }
            prevHp = hp.Value;
        });
    }
    public override void OnUpdate()
    {
        nowPos = transform.position;
        if (hp.Value > 0)
        {
            destination = GameManager.Instance.ReturnPlayer();
        }
        shortestDistance = Vector3.Distance(transform.position, GameManager.Instance.ReturnPlayer().transform.position);;
        anim.SetInteger("StateID",stateMachine.ReturnCurrentState().animStateID);
        stateMachine.Update(); 
        base.OnUpdate();
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();;
        stateMachine.FixedUpdate();
    }
    public override void SetStat()
    {
        chartRead = GameManager.Instance.dataManager.ReadRow(enemyID, chartRead, (int)DataManager.ChartName.MonsterChart,2);
        stat = new Stat(chartRead);
    }
    public override void SetCombo()
    {
        comboList.Add(new SingleAtkState(this,3));
    }
    public void Die()
    {
        GameManager.Instance.spawner.spawncount[enemyID-1] -= 1;
        GameManager.Instance.spawner._killCount.Value++;
        GameManager.Instance.money.Value += giveMoney;
        comboList.Clear();
        gameObject.SetActive(false);
    }
}

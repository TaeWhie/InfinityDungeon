using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UniRx;
using Random = UnityEngine.Random;
public class Spawner : MonoBehaviour
{
    public float waveDelaytime = 2;
    public float delaytime=0;
    public int totalMax = 0;
    public float minDist = 3;
    public float maxDist = 5;
    public bool _waveStart = false;
    public ReactiveProperty<int> _killCount;
    public List<int> spawncount =new() ;
    public List<int> max = new();
    
    private List<int> _mEnemyDeck = new();
    private int _mCount = 0;
    private List<string> _chartRead=new();
    private NavMeshTriangulation _triangulation;
    public void Init()
    {
        _triangulation = NavMesh.CalculateTriangulation();
        SetOneStage();
        _killCount.Where(x=>x == totalMax).Subscribe(data =>
        {
            _waveStart = false;
            GameManager.Instance.stageLevel++;
            GameManager.Instance.stageTotal.Value++;
            if (GameManager.Instance.stageLevel >= GameManager.Instance.dataManager
                    .chartInfos[(int)DataManager.ChartName.WaveChart].lineSize)
            {
                GameManager.Instance.resetCount++;
                GameManager.Instance.stageLevel = 1;//무한 던전
            }
            spawncount.Clear();
            max.Clear();
            _mEnemyDeck.Clear();
            _killCount.Value = 0;
            SetOneStage();
        }).AddTo(gameObject);
    }
    
    void Spawn(int i)
    {
        GameObject enemy = null; 
        NavMeshHit Hit;
        int VertexIndex = Random.Range(0, _triangulation.vertices.Length);
        
        if (NavMesh.SamplePosition(_triangulation.vertices[VertexIndex], out Hit, maxDist, -1))
        {
            if (minDist < Vector3.Distance(Hit.position,GameManager.Instance.ReturnPlayer().transform.position))
            {
                enemy = GameManager.Instance.enemypool.Get(i);
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                enemyComponent.OnAwake();
                enemyComponent.OnStart();
                enemyComponent.navMeshAgent.Warp(Hit.position);
                enemy.GetComponent<ObjectStat>().hp.Value = enemy.GetComponent<ObjectStat>().stat.maxHp;
            }
            else
            {
                Spawn(i);
            }
        }
        if (enemy != null)
        {
            spawncount[i]++;
        }
    }
    public void SetOneStage()
    {
        _chartRead = GameManager.Instance.dataManager.ReadRow(GameManager.Instance.stageLevel, _chartRead,
            (int)DataManager.ChartName.WaveChart,0);
        totalMax=Int32.Parse(_chartRead[1]);
        delaytime = float.Parse(_chartRead[2]);
        for (int i = 0; i < _chartRead.Count-3; i++)
        {
            max.Add(int.Parse(_chartRead[i+3]));
            for (int j = 0; j < max[i]; j++)
            {
                _mEnemyDeck.Add(i);
            }
            spawncount.Add(0);
        }
        Util.Shuffle(_mEnemyDeck);
        GameManager.Instance.ReturnPlayer().GetComponent<Player>().attackCount.Value = 0;
        StartCoroutine("TimeSpawn",_mCount);
    }
    IEnumerator TimeSpawn(int count)
    {
        yield return new WaitForSeconds(waveDelaytime);
        _waveStart = true;
        while (count < totalMax)
        {
            Spawn(_mEnemyDeck[count]);
            count++;
            yield return new WaitForSeconds(delaytime);
        }
    }
}

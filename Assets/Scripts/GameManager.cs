using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour
{ 
    public DataManager dataManager;
    public UIManager uiManager;
    public PoolManager enemypool;
    public PoolManager effectpool;
    public PoolManager soundpool;
    public Spawner spawner;
    public ReactiveProperty<int> money;
    public int stageLevel = 1;
    public ReactiveProperty<int> stageTotal;
    public int resetCount = 0;
     
    private GameObject _player;
    private static GameManager s_Instance;
    
    public static GameManager Instance
    {
        get { Init(); return s_Instance; }
    }
    static void Init()
    {
        if (s_Instance == null)
        {
            GameObject go = GameObject.Find("GameManager");

            if (go == null)
            {
                go = new GameObject { name = "GameManager" };
                go.AddComponent<GameManager>();
            }
            s_Instance = go.GetComponent<GameManager>();
        }
    }
    public  GameObject ReturnPlayer()
    {
        return _player;
    }
    private void Awake()
    {
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _player = GameObject.Find("Player");
        dataManager.allComplete.Where(x => x == true).Subscribe(data =>
        { 
            _player.GetComponent<Player>().OnAwake();
            stageTotal.Value = stageLevel+resetCount*(dataManager.chartInfos[(int)DataManager.ChartName.WaveChart].lineSize-1);
            uiManager.loadingCanvas.SetActive(false);
            uiManager.OnAwake();
        }).AddTo(gameObject);
    }
    void Start()
    {
        Init();
        dataManager.OnStart();
        dataManager.allComplete.Where(x => x==true).Subscribe(data =>
        {
            _player.GetComponent<Player>().OnStart();
            uiManager.Init();
            spawner.Init();
        }).AddTo(gameObject);
    }
    void Update()
    {
        dataManager.allComplete.Where(x => x==true).Subscribe(data =>
        {
            _player.GetComponent<Player>().OnUpdate();
            foreach (List<GameObject> oneset in enemypool.pools)
            {
                foreach (GameObject enemy in oneset)
                {
                    enemy.GetComponent<Enemy>().OnUpdate();
                }
            }
        }).AddTo(gameObject);
    }

    private void FixedUpdate()
    {
        dataManager.allComplete.Where(x => x == true).Subscribe(data =>
        {
            _player.GetComponent<Player>().OnFixedUpdate();
            foreach (List<GameObject> oneset in enemypool.pools)
            {
                foreach (GameObject enemy in oneset)
                {
                    enemy.GetComponent<Enemy>().OnFixedUpdate();
                }
            }
        }).AddTo(gameObject);
    }
}

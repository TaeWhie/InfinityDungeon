using System.Collections.Generic;
using UnityEngine;
public class PoolManager : MonoBehaviour
{
    public GameObject[] prefabs;
    public List<GameObject>[] pools;
    
    private int _count = 0;
    private void Start()
    {
        pools = new List<GameObject>[prefabs.Length];
        for(int i = 0; i<pools.Length;i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int i)
    {
        GameObject select = null;
        foreach (GameObject item in pools[i])
        {
            if (item != null && !item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                return select;
            }
        }
        if (select == null)
        {
            select = Instantiate(prefabs[i],transform);
            _count++;
            select.name += _count;
            pools[i].Add(select);
        }
        else
        {
            select = null;
        }
        return select;
    }
}

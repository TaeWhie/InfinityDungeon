using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UniRx;

public class DataManager : MonoBehaviour
{
    public ChartInfo[] chartInfos = new ChartInfo[(int)ChartName.Count];
    public ReactiveProperty<bool> allComplete = new ReactiveProperty<bool>();
    
    private ReactiveProperty<int> _counter = new ReactiveProperty<int>();
    public enum ChartName
    {
        MonsterChart, PlayerChart , WaveChart , Count
    }
    private void OneTimeLoad(ReactiveProperty<int> i)//i번호 부터 하나가 완료되면 다음꺼를 로딩하는식
    {
        i.Value++;
        if (i.Value < (int)ChartName.Count)
        {
            chartInfos[i.Value] = new ChartInfo();
            chartInfos[i.Value].chartName = ((ChartName)i.Value).ToString();
            Addressables.LoadAssetAsync<TextAsset>(chartInfos[i.Value].chartName).Completed +=
            (AsyncOperationHandle<TextAsset> obj) =>
            {
                chartInfos[i.Value].chart = obj.Result;
                OneTimeLoad(i);
            };
        }
    }
    public void OnStart()
    {
        _counter.Value = -1;
        OneTimeLoad(_counter);
        _counter.Where(x => x == (int)ChartName.Count).Subscribe(data =>
        { 
            for (int i = 0; i < chartInfos.Length; i++)
            {
                Analyzetxt( chartInfos[i]);
            }
            allComplete.Value = true;    
        }).AddTo(gameObject);
    }
    public List<string> ReadRow(int id, List<string> word,int chartInfoNum,int start)
    {
        word.Clear();
        for (int i = start ;i < chartInfos[chartInfoNum].rowSize ;i++)
        {
            if (i == chartInfos[chartInfoNum].rowSize - 1)
            {
                chartInfos[chartInfoNum].stringchart[id, i] =
                    chartInfos[chartInfoNum].stringchart[id, i].Replace("\r", "");
            }
            word.Add(chartInfos[chartInfoNum].stringchart[id, i]);
        }
        return word;
    }
    public int ReturnlineSize(ChartInfo chart)
    {
        return chart.lineSize;
    }
    private void Analyzetxt(ChartInfo chartInfo)
    {
        string[] word = null;
        string currentText = chartInfo.chart.text.Substring(0, chartInfo.chart.text.Length - 1);
        
        chartInfo.line = currentText.Split('\n');
        chartInfo.lineSize = chartInfo.line.Length;
        chartInfo.rowSize = chartInfo.line[0].Split(',').Length;
        chartInfo.stringchart = new string[chartInfo.lineSize, chartInfo.rowSize];
        for (int i = 0; i < chartInfo.lineSize; i++)
        {
            string[] sentence = chartInfo.line[i].Split(',');
            word = sentence;

            for (int j = 0; j < chartInfo.rowSize; j++)
            {
                chartInfo.stringchart[i, j] = word[j];
            }
        }
    }
}


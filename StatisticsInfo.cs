using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using E2C;
using TMPro;
using UnityEngine;

public class StatisticsInfo : MonoBehaviour
{
    public int generation;
    public float believersPercent;
    public TMP_Text percentsText;
    public TMP_Text generationText;
    public List<float> precentsList = new List<float>();
    public List<float> generationsList = new List<float>();
    public E2ChartData plotData;

    public bool firstRuning = true;

    public int lastGeneration;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateStatistics(float percent)
    {
       // if (percent != believersPercent) ++lastGeneration;
       //++lastGeneration;
        updateGeneration();
        if (percent != believersPercent) lastGeneration = generation;
        updatePercent(percent);
        precentsList.Add(percent);
        generationsList.Add(generation);
        //if (Math.Abs(percent - believersPercent) < 0.001) --lastGeneration;
    }
    
    
    
    public void updateGeneration()
    {
        ++generation;
        updateGui();
    }

    public void updateToLastGeneration()
    {
        generation = lastGeneration;
        generationText.text = lastGeneration.ToString();
        updateGui();
    }

    public void ResetGeneration()
    {
        generation = 0;
    }

    public void updatePercent(float percent)
    {
        believersPercent = percent;
        updateGui();
    }

    public void resentPercent()
    {
        believersPercent = 0;
    }

    public void resetAll()
    {
        precentsList.Clear();
        generationsList.Clear();
        resentPercent();
        ResetGeneration();
        updateGui();
        plotData.series.Clear();
        lastGeneration = 0;
    }

    private void updateGui()
    {
        percentsText.text = believersPercent.ToString();
        generationText.text = generation.ToString();
    }

    public void showSpeedStatistic()
    {
        E2ChartData.Series s = new E2ChartData.Series();
        //s.name = "Percents";
        List<float> newGenList = new List<float>();
        List<float> newPercentList = new List<float>();
        copyListUntil(generationsList,newGenList);
        copyListUntil(precentsList,newPercentList);
        s.dataX = newGenList;
        s.dataY = newPercentList;//precentsList;
        plotData.series.Add(s);
        s.show = true;
    }

    private void copyListUntil(List<float> oldList,List<float> newList)
    {
        for (int i = 0; i < lastGeneration; i++)
        {
            newList.Add(oldList[i]);
        }
    }
    
}

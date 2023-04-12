using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//using Random = System.Random;

public class GridManager : MonoBehaviour
{
    public GameObject cell;
    private float leftUpx;
    public Transform startPoint;
    private float leftUpY;
    private CellManager[,] cells = new CellManager[100,100];
    private List<CellManager> livingCells = new List<CellManager>();
    private float p;
    private float l;
    private bool running = false;
    private float s1Probality;
    private float s2Probality;
    private float s3Probality;
    private float s4Probality;
    public StatisticsInfo st;
    public SliderManager sm;
    public int MultiRuningCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        leftUpx = startPoint.localPosition.x;
        leftUpY = startPoint.localPosition.y;
        generateGrid();
    }

    // Update is called once per frame
    void Update()
    {
    }
    // fixed update is runing every fixed time, i prefer update the simulation here for more
    // smooth movement.
    private void FixedUpdate()
    {
        if (running) 
        {
            updateSimulation();
            float percent = countPercentsOfBelievers();
            st.updateStatistics(percent);
            if (percent == 100) stopSimulation();
        } 
    }

    /***************************************************
     * Function Name: generateGrid
     * Description: this function create and initialize
     * the grid;
     **************************************************/
    private void generateGrid()
    {
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                GameObject c = Instantiate(cell);
                cells[x, y] = c.GetComponent<CellManager>();
                c.transform.localPosition = startPoint.localPosition;
                c.transform.localPosition = new Vector3(leftUpx, leftUpY, 0);
                leftUpx += 0.178f;
            }

            leftUpY -= 0.084f;
            leftUpx = startPoint.localPosition.x;
        }
    }
    /***************************************************
     * Function Name: createLivingCells
     * Description: on each cell, choose if he live
     * or dead with "p" probability.
     **************************************************/
    private void createLivingCells()
    {
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                cells[x, y].liveWithProbality(p);
                setNeighbors(cells[x,y],x,y);
                cells[x,y].changeLvalue(l);
                if(cells[x,y].isAlive) livingCells.Add(cells[x,y]);
            }
        }
    }
    /***************************************************
     * Function Name: setNeighbors
    * Description: add to each cell his neighbors.
     **************************************************/
    private void setNeighbors(CellManager cell,int x,int y)
    {
        if(x+1 < 100) cell.addNeighboor(cells[x+1,y]);
        if(x-1 >= 0)  cell.addNeighboor(cells[x-1,y]);
        if(y+1 < 100) cell.addNeighboor(cells[x,y+1]);
        if(y-1 >= 0) cell.addNeighboor(cells[x,y-1]);
        if(x+1 < 100 && y+1 < 100) cell.addNeighboor(cells[x+1,y+1]);
        if(x+1 < 100 && y-1 >= 0) cell.addNeighboor(cells[x+1,y-1]);
        if(x-1 >= 0 && y+1 < 100) cell.addNeighboor(cells[x-1,y+1]);
        if(x-1 >= 0 && y-1 >= 0) cell.addNeighboor(cells[x-1,y-1]);
    }
    
    /***************************************************
   * Function Name: chooseState
   * Description: this function initialize the state
    * of each cell
   **************************************************/  
    private void chooseState()
    {
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                int state = sm.getStateByUserProbality();
                cells[x,y].changeState(state);
            }
        }
    }
    /***************************************************
   * Function Name: SetPvalue
   * Description: this method set the p value the user
     * wrote in the input box.
   **************************************************/  
    public void SetPvalue(string number)
    {
        if (number == null || running) return;
        float f;
        if (float.TryParse(number,out f))
        {
            p = f/100;
        }
    }
    /***************************************************
   * Function Name: setLvalue
   * Description: here just select the l value the user
     * entered.
   **************************************************/ 
    public void setLvalue(string number)
    {
        if (number == null || running) return;
        int f;
        if (int.TryParse(number,out f))
        {
            l = f;
        }
    }
    
    
    /*****************************************************************************************
    * Function Name: startSimulation
    * Description: this method is invoked when the user press the "start simulation" button
    **************************************************************************************/ 
    public void startSimulation()
    {
        createLivingCells();
        chooseState();
        selectRandomStart();
        running = true;

    }
    /***************************************************
    * Function Name: selectRandomStart
    * Description: this method select random living cell
     * that will start to spread the rumor.
    **************************************************/ 
    private void selectRandomStart()
    {
        float maxPossibleCell = livingCells.Count - 1;
        int randomCellIndex = (int)Random.Range(0f, maxPossibleCell);
        livingCells[randomCellIndex].makeBelieving();
    }
    /*******************************************************************************
   * Function Name: updateSimulation
   * Description: this method is cover full generation which means:
     * 1) if a person believe and l = 0 then he will spread the rumor
     * 2) if a person got more  rumors then 2, he will update his state and he
     * decide if believe
   ********************************************************************************/  
    private void updateSimulation()
    {
        // start with spreading the rumor
        foreach (var cell in livingCells)
        {
            if (cell.isBelieving)
            {
                cell.spreadTheRumor();
            }
        }
        // decide if believe or not.
        foreach (var cell in livingCells)
        {
            if (!cell.isBelieving)
            {
                cell.decideIfBelieve();
            }
        }
    }
    /**********************************************************************
    * Function Name: resetSimulation
    * Description: this method invoked when the user press "reset" button,
    ***********************************************************************/ 
    public void resetSimulation()
    {
        running = false;
        livingCells.Clear();
        foreach (var cell in cells)
        {
            cell.resetCell();
        }
        st.resetAll();
    }
    /*********************************************************************
    * Function Name: countPercentsOfBelievers
    * Description: this method if for statistics, count the percentage
     * of believers from the total living cells.
    ********************************************************************/ 
    private float countPercentsOfBelievers()
    {
        float total = livingCells.Count;
        float believers = 0;
        foreach (var cell in livingCells)
        {
            if (cell.isBelieving) ++believers;
        }
        float p = (believers / total)  * 100;
        return p;
    }

    public void stopSimulation()
    {
        running = false;
        st.updateToLastGeneration();
        st.showSpeedStatistic();
    }
    /*********************************************************************
    * Function Name: blockOfLinesStrategy
    * Description: this method is to block of lines with the same state.
    ********************************************************************/ 
    public void blockOfLinesStrategy(int blockSize)
    {
        int currentBlockSize = 0;
        int firstState = 4;
        int currentState = 4;
        for (int y = 0; y < 100; y++)
        {
            for (int x = 0; x < 100; x++)
            {
                cells[x,y].changeState(currentState);
            }

            ++currentBlockSize;
            if (currentBlockSize == blockSize)
            {
                currentState = nextState(currentState);
                currentBlockSize = 0;
            }
        }
        
    }
    private int nextState(int s)
    {
        int newState = --s;
        if (newState == 0) return 4;
        return newState;
    }
    /*********************************************************************
    * Function Name: blockOfLineSimulation
    * Description: when the user click on "Blocks Simulation"
     * this method start.
    ********************************************************************/ 
    public void blockOfLineSimulation()
    {
        createLivingCells();
        blockOfLinesStrategy(20);
        selectRandomStart();
        running = true;
    }

}

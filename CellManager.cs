using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public bool isAlive;
    public bool isBelieving;
    private float p;
    private int maxL;
    private int currentL;
    private float state;
    private float StateProbality;
    private SpriteRenderer spriteRenderer;
    public List<CellManager> neighbors;
    public int rumorsFromNeighbors;
    private bool UpdatedBelieve;

    // Start is called before the first frame update
    void Start()
    {
        isBelieving = false;
        UpdatedBelieve = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
    }


    public void changePvalue(float newValue)
    {
        p = newValue;
    }

    public void changeLvalue(float newValue)
    {
        maxL = (int)newValue;
    }

    public void changeState(int s)
    {
        state = s;
        updateStateProbality();
    }

    private void updateStateProbality()
    {
        if (state == 1) StateProbality = 1;
        if (state == 2) StateProbality = 2 / 3f;
        if (state == 3) StateProbality = 1 / 3f;
        if (state == 4) StateProbality = 0f;
    }
    /*********************************************************************
     * Function Name: liveWithProbality
     * Description: this method decide if the cell will be alive or dead,
     * if he chosen to be alive then his color will be red.
    *********************************************************************/  
    public void liveWithProbality(float p)
    {
        bool rand = Random.value < p;
        if (rand)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            isAlive = true;
        }
    }
    public void addNeighboor(CellManager n)
    {
        if (neighbors == null) neighbors = new List<CellManager>();
        neighbors.Add(n);
    }
    /*********************************************************************
    * Function Name: makeBelieving
    * Description: when cell is believe, the the is color became green.
  ********************************************************************/  
    public void makeBelieving()
    {
        isBelieving = true;
        spriteRenderer.color = Color.green;
    }
    /*********************************************************************
    * Function Name: spreadTheRumor
    * Description: spread the rumor to each neighbor if possible.
  ********************************************************************/   
    public void spreadTheRumor()
    {
        if (currentL == 0)
        {
            foreach (var n in neighbors)
            {
                spreadToNeighbor(n);
            }
            currentL = maxL;
        }
        else
        {
            --currentL;
        }
        
        
    }
    /*********************************************************************
  * Function Name: spreadToNeighbor
  * Description: spread the rumor to the neighbors 
  ********************************************************************/   
    private void spreadToNeighbor(CellManager n)
    {
        n.rumorsFromNeighbors += 1;
    }
    /*********************************************************************
    * Function Name: decideIfBelieve
    * Description: in this method the cell decide if believe or not,
     * is decided yes then believe (became green).
    ********************************************************************/ 
    public void decideIfBelieve()
    {
        if (rumorsFromNeighbors == 0 || !isAlive) return;
        if (isBelieving)
        {
            makeBelieving();
            return;
        }
        if(rumorsFromNeighbors >= 2) believeMore();
        var r = Random.Range(0f, 1f);
        if(r < StateProbality) makeBelieving();
        decreaseBelieve();
        rumorsFromNeighbors = 0;

    }
    /*********************************************************************
    * Function Name: believeMore
    * Description: this method increased the believe state.
    ********************************************************************/ 
    private void believeMore()
    {
        if (state == 1) return;
        changeState((int)(state - 1));
        UpdatedBelieve = true; // if the state changed, this bool let know that it should resotred.
    }
    /*********************************************************************
    * Function Name: decreaseBelieve
    * Description: this method decrease the state of believe, since increasing
     * is relevant only for one generation, this method restore the state
    ********************************************************************/  
    private void decreaseBelieve()
    {
        if(UpdatedBelieve) changeState((int)(state + 1));
        UpdatedBelieve = false;
    }
    /*********************************************************************
    * Function Name: resetCell
    * Description: this method reset the cell to his first state before
     * the user start the simulation, in other words it "kills" the cell.
    ********************************************************************/  
    public void resetCell()
    {
        isAlive = false;
        spriteRenderer.color = Color.black;
        isBelieving = false;
        currentL = 0;
        neighbors.Clear();
        rumorsFromNeighbors = 0;
        UpdatedBelieve = false;
    }
}

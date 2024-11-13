using System.Collections;
using System.Collections.Generic;
using BennetWang.MovementSystem;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }
    List<Dimension> dimensions;

    
    public int loopingDimensionsTill;

    private int currentDimension;

 
    public int totalDimensions { get; private set; }

    [SerializeField]
    Transform spawnPoint;
    List<Transform> spawnPoints;

    static Dimension duel, player1Illusion, player2Illusion;

    public int dimensionStreak { get; private set; }
    bool prevNext;

    PlayerActor[] pB;
    PlayerActor[] pI;

    public PlayerMovementController[] players;

    [SerializeField] float lowAlpha;
    public int winningStreak;
    private void Awake()
    {
        // Check if an instance already exists and destroy the new one if so
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Assign this instance as the singleton instance and make it persistent
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = new List<Transform>();

        foreach(Transform child in spawnPoint.transform)
        {
            spawnPoints.Add(child);
        }

        
       
        pB = new PlayerActor[players.Length];
        pI = new PlayerActor[players.Length];

        for(int i=0;i<players.Length;i++)
        {
            pI[i] = players[i].illusion;
            pB[i] = players[i].body;
        }

        GameObject[] dims = GameObject.FindGameObjectsWithTag("Dimension");

        dimensions = new List<Dimension>();
        for(int i=0; i<dims.Length;i++)
        {

            Dimension dimComponent = dims[i].GetComponent<Dimension>();
            if (dimComponent != null)
            {
                dimensions.Add(dimComponent);
            }
            else
            {
                Debug.LogWarning("GameObject at index " + i + " does not have a Dimension component.");
            }
        }

        dimensions.Sort((a, b) => a.GetComponent<Dimension>().Index.CompareTo(b.GetComponent<Dimension>().Index));
        totalDimensions = dimensions.Count;
   

        
       
       

      

        currentDimension = -1;
        changeDimension(true);
    }

   

    public void setCurrentColor(Color col)
    {
        dimensions[currentDimension].GetComponent<DimensionWin>().changeToWinnerColor(col);
    }



    public void changeDimension(bool next)
    {
        bool inLoop = winningStreak < (loopingDimensionsTill);


        if (inLoop)
        {
            if (next) 
                currentDimension = (currentDimension + 1) % loopingDimensionsTill;
            else
                currentDimension = ((loopingDimensionsTill - 1 + currentDimension) % loopingDimensionsTill);
        }

       
        else
        {
            if (winningStreak == loopingDimensionsTill)
            {

                currentDimension = loopingDimensionsTill;
            }
            else
            {
                currentDimension++;
                if (currentDimension >= totalDimensions)
                {
                    foreach (Dimension dim in dimensions)
                    {
                        currentDimension = 0;
                        dim.gameObject.GetComponent<DimensionWin>().changeToWinnerColor(Color.white);
                    }
                }
            }
        }

        
        //change collision layer

        // change position

        //change gravity


        int leftDimension = (loopingDimensionsTill - 1 + currentDimension) % loopingDimensionsTill;

        int rightDimension = (currentDimension + 1) % loopingDimensionsTill;


        if (player1Illusion)
        {
            player1Illusion.Unregister();
        }
        if (player2Illusion)
        {
            player2Illusion.Unregister();
        }
        if (duel)
        {
            duel.Unregister();
        }


        player1Illusion = dimensions[leftDimension];

        duel = dimensions[currentDimension];

        player2Illusion = dimensions[rightDimension];

        for (int i = 0; i < dimensions.Count; i++)
        {
            dimensions[i].gameObject.GetComponent<DimensionWin>().setAlpha(lowAlpha);
        }

        player1Illusion.SetAsDimension(Dimension.SpecialDimension.Player1Illusion);
        player1Illusion.gameObject.GetComponent<DimensionWin>().setAlpha(lowAlpha);

        duel.SetAsDimension(Dimension.SpecialDimension.Duel);
        duel.gameObject.GetComponent<DimensionWin>().setAlpha(1f);

        player2Illusion.SetAsDimension(Dimension.SpecialDimension.Player2Illusion);
        player2Illusion.gameObject.GetComponent<DimensionWin>().setAlpha(lowAlpha);


        int spawnLoop = inLoop ? loopingDimensionsTill : spawnPoints.Count;
        pB[0].setPosition(spawnPoints[currentDimension].position);
        pB[1].setPosition(spawnPoints[(currentDimension+1)%spawnLoop].position);

        pI[0].setPosition(spawnPoints[leftDimension].position);
        pI[1].setPosition(spawnPoints[(rightDimension+1)%spawnLoop].position);

        setPlayersAccordingToDimension();
        
    }

   


    void setPlayersAccordingToDimension()
    {
        switch (currentDimension)
        {
            case 0:
                

                pB[0].GravityDirection = PlayerActor.GravityType.Downward;
                pB[1].GravityDirection = PlayerActor.GravityType.Downward;

                pI[0].GravityDirection = PlayerActor.GravityType.Leftward;
                pI[1].GravityDirection = PlayerActor.GravityType.Rightward;
                break;

            case 1:

                pB[0].GravityDirection = PlayerActor.GravityType.Rightward;
                pB[1].GravityDirection = PlayerActor.GravityType.Rightward;

                pI[0].GravityDirection = PlayerActor.GravityType.Downward;
                pI[1].GravityDirection = PlayerActor.GravityType.Upward;
                break;

            case 2:

                pB[0].GravityDirection = PlayerActor.GravityType.Upward;
                pB[1].GravityDirection = PlayerActor.GravityType.Upward;

                pI[0].GravityDirection = PlayerActor.GravityType.Rightward;
                pI[1].GravityDirection = PlayerActor.GravityType.Leftward;
                break;

            case 3:

                pB[0].GravityDirection = PlayerActor.GravityType.Leftward;
                pB[1].GravityDirection = PlayerActor.GravityType.Leftward;

                pI[0].GravityDirection = PlayerActor.GravityType.Upward;
                pI[1].GravityDirection = PlayerActor.GravityType.Downward;
                break;

            case 4:

                pB[0].GravityDirection = PlayerActor.GravityType.Downward;
                pB[1].GravityDirection = PlayerActor.GravityType.Downward;

                break;
        }

        bool dualOn=false;
        if (currentDimension < loopingDimensionsTill)
        {
            dualOn = true;
        }

        foreach (BennetWang.MovementSystem.PlayerMovementController p in players)
        {
            p.DualExistence = dualOn;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameManager : MonoBehaviour
{
    public static AttackGameManager Instance { get; private set; }

    [SerializeField]
    WinScreen winScreen;

    public
    GameStatusUI gameStatus;

    public bool roundOver;

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
        
       
    }

    //Update is called once per frame
    void Update()
    {
       
    }

    public void playerKilled(GameObject playerBodyDead, GameObject playerBodyKiller)
    {
        if (roundOver)
        {
            return;
        }
        BennetWang.MovementSystem.PlayerActor[] playerBodys = { DimensionManager.Instance.players[0].body, DimensionManager.Instance.players[1].body };

        bool direction = (playerBodyKiller == playerBodys[0].gameObject);

       
        AttackPlayerCircleType killerPlayer = playerBodyKiller.GetComponent<AttackPlayerCircleType>();
        AttackPlayerCircleType deadPlayer = playerBodyDead.GetComponent<AttackPlayerCircleType>();


        //Player increasing dimensions streak
        if (deadPlayer.dimensionStreak==0)
        {

           
            DimensionManager.Instance.winningStreak = ++killerPlayer.dimensionStreak;
            gameStatus.setText(killerPlayer.fightName, killerPlayer.dimensionStreak);
            
            //Player Won
            if (killerPlayer.dimensionStreak >= DimensionManager.Instance.totalDimensions)
            {
                Debug.Log("GameWonnnnw");
                winScreen.gameWon(killerPlayer.fightName, killerPlayer.winColor);
            }
            else
            {
                //Normal Increase
                DimensionManager.Instance.setCurrentColor(killerPlayer.winColor);
                DimensionManager.Instance.changeDimension(direction);
                
            }

        }
        //Moving Down
        else if(killerPlayer.dimensionStreak==0)
        {

            if (deadPlayer.dimensionStreak == DimensionManager.Instance.totalDimensions - 1)
            {
                DimensionManager.Instance.winningStreak++;
                DimensionManager.Instance.changeDimension(direction);
                DimensionManager.Instance.winningStreak = deadPlayer.dimensionStreak = 0;
                
            }
            else
            {
                
                DimensionManager.Instance.winningStreak = --deadPlayer.dimensionStreak;
                gameStatus.setText(deadPlayer.fightName, deadPlayer.dimensionStreak);

                DimensionManager.Instance.changeDimension(direction);
                DimensionManager.Instance.setCurrentColor(Color.white);
            }
            
            if(deadPlayer.dimensionStreak==0 && killerPlayer.dimensionStreak == 0)
            {
                gameStatus.setText("", 0);
            }
        }



        //if (killerPlayer.dimensionStreak == DimensionManager.Instance.totalDimensions - 2)
        //{
        //    DimensionManager.Instance.winningStreak = killerPlayer.dimensionStreak++;

        //    foreach (Bennet.MovementSystem.PlayerMovementController player in DimensionManager.Instance.players)
        //    {
        //        player.DualExistence = false;
        //    }
        //}

        //if (killerPlayer.dimensionStreak == DimensionManager.Instance.totalDimensions)
        //{



        //}

        foreach (BennetWang.MovementSystem.PlayerActor player in playerBodys)
        {
            player.gameObject.GetComponent<HitsHealth>().resetHealth();
        }



        Debug.Log(playerBodyDead.name + "Got Killed by" + playerBodyKiller);
       

        
    }


        
}

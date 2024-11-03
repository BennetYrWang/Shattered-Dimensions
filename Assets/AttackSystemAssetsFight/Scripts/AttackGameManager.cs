using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameManager : MonoBehaviour
{
    public static AttackGameManager Instance { get; private set; }

    [SerializeField]
    WinScreen winScreen;

    

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
        Bennet.MovementSystem.PlayerActor[] playerBodys = { DimensionManager.Instance.players[0].body, DimensionManager.Instance.players[1].body };

        bool direction = (playerBodyKiller == playerBodys[0].gameObject);

       
        PlayerAttackController killerPlayer = playerBodyKiller.GetComponent<PlayerAttackController>();
        PlayerAttackController deadPlayer = playerBodyDead.GetComponent<PlayerAttackController>();


        //Player increasing dimensions streak
        if (deadPlayer.dimensionStreak==0)
        {
           
            DimensionManager.Instance.winningStreak = ++killerPlayer.dimensionStreak;

            
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
            DimensionManager.Instance.winningStreak = --deadPlayer.dimensionStreak;


            DimensionManager.Instance.changeDimension(direction);
            DimensionManager.Instance.setCurrentColor(Color.white);

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

        foreach (Bennet.MovementSystem.PlayerActor player in playerBodys)
        {
            player.gameObject.GetComponent<HitsHealth>().resetHealth();
        }



        Debug.Log(playerBodyDead.name + "Got Killed by" + playerBodyKiller);
       

        
    }


        
}

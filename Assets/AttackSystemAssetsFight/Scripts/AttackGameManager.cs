using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameManager : MonoBehaviour
{
    public static AttackGameManager Instance { get; private set; }

    [SerializeField]
    GameObject[] playerBodys;

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
        //playerBodys = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playerKilled(GameObject playerBodyDead, GameObject playerBodyKiller)
    {


        PlayerAttackController killerPlayer = playerBodyKiller.GetComponent<PlayerAttackController>();
        PlayerAttackController deadPlayer = playerBodyDead.GetComponent<PlayerAttackController>();

        if (killerPlayer.dimensionStreak == DimensionManager.Instance.totalDimensions-2)
        {
            killerPlayer.dimensionStreak++;
            foreach(GameObject player in playerBodys)
            {
                player.transform.parent.GetComponent<Bennet.MovementSystem.PlayerMovementController>().DualExistence = false;
            }
        }

        if (killerPlayer.dimensionStreak == DimensionManager.Instance.totalDimensions )
        {
            Debug.Log("GameWon");
            winScreen.gameWon(killerPlayer.fightName, killerPlayer.winColor);
           
        }

       

        if ((killerPlayer.dimensionStreak==0&& deadPlayer.dimensionStreak == 0)|| (killerPlayer.dimensionStreak!=0 && deadPlayer.dimensionStreak==0))
        {
            killerPlayer.dimensionStreak++;
            DimensionManager.Instance.setCurrentColor(killerPlayer.winColor);

        }

        Debug.Log(playerBodyDead.name + "Got Killed by" + playerBodyKiller);
        
        DimensionManager.Instance.changeDimension(playerBodyKiller==playerBodys[0]);

        if (killerPlayer.dimensionStreak == 0 && deadPlayer.dimensionStreak != 0)
        {
            DimensionManager.Instance.setCurrentColor(Color.white);
            deadPlayer.dimensionStreak--;

            if (deadPlayer.dimensionStreak == DimensionManager.Instance.totalDimensions - 1)
            {
                foreach (GameObject player in playerBodys)
                {
                    player.transform.parent.GetComponent<Bennet.MovementSystem.PlayerMovementController>().DualExistence = true;
                }
            }

        }
        foreach (GameObject player in playerBodys)
        {
            player.GetComponent<HitsHealth>().resetHealth();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGameManager : MonoBehaviour
{
    public static AttackGameManager Instance { get; private set; }

    [SerializeField]
    GameObject[] playerBodys;

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
        
        Debug.Log(playerBodyDead.name + "Got Killed by" + playerBodyKiller);
        DimensionManager.Instance.changeDimension(playerBodyKiller==playerBodys[0]);
        foreach (GameObject player in playerBodys)
        {
            player.GetComponent<HitsHealth>().resetHealth();
        }
    }
}

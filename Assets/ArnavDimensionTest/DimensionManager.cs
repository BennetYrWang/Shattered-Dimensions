using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [SerializeField]
    Dimension[] dimensions;

    int currentDimension;

    int totalDimensions;

    static Dimension duel, player1Illusion, player2Illusion;

    [SerializeField] Transform firstSpawnPoint;
    [SerializeField] float DistanceBetweenSpawnPoints=10f;

    [SerializeField] PlayerActor[] playerIllusion;
    [SerializeField] PlayerActor[] playerBody;

    Vector3 spawnPos00;
    Vector3 spawnPos10;
    Vector3 spawnPos01;
    Vector3 spawnPos11;

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
        spawnPos00 = firstSpawnPoint.position;
        spawnPos10 = new Vector3(spawnPos00.x + DistanceBetweenSpawnPoints, spawnPos00.y, spawnPos00.z);
        Debug.Log(spawnPos10);
        spawnPos11 = new Vector3(spawnPos10.x , spawnPos10.y + DistanceBetweenSpawnPoints, spawnPos10.z);
        spawnPos01 = new Vector3(spawnPos11.x - DistanceBetweenSpawnPoints , spawnPos11.y, spawnPos11.z);
        totalDimensions = dimensions.Length;

        currentDimension--;
        changeDimension(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDimension(bool next)
    {
        //change collision layer

        // change position

        //change gravity

        if(next)
            currentDimension=(currentDimension+1)%totalDimensions;
        else
            currentDimension = (totalDimensions - 1 + currentDimension) % totalDimensions; ;

        int leftDimension = (totalDimensions -1 + currentDimension) %totalDimensions;

        int rightDimension = (currentDimension + 1) % totalDimensions;


        if (player1Illusion)
        {
            player1Illusion.Unregister(Dimension.SpecialDimension.Player1Illusion);
        }
        if (player2Illusion)
        {
            player2Illusion.Unregister(Dimension.SpecialDimension.Player2Illusion);
        }

        if (duel)
        {
            player1Illusion.Unregister(Dimension.SpecialDimension.Duel);
        }

        player1Illusion = dimensions[leftDimension];

        duel= dimensions[currentDimension];

        player2Illusion = dimensions[rightDimension];

       
        player1Illusion.SetAsDimension(Dimension.SpecialDimension.Player1Illusion);

        duel.SetAsDimension(Dimension.SpecialDimension.Duel);

        player2Illusion.SetAsDimension(Dimension.SpecialDimension.Player2Illusion);

        switch (currentDimension)
        {
            case 0:


                playerBody[0].gravityDirection = PlayerActor.GravityDirection.Downward;
                playerBody[1].gravityDirection = PlayerActor.GravityDirection.Downward;

                playerIllusion[0].gravityDirection = PlayerActor.GravityDirection.Leftward;

                playerIllusion[1].gravityDirection = PlayerActor.GravityDirection.Rightward;


                playerBody[0].gameObject.transform.position = spawnPos00;
                playerBody[1].gameObject.transform.position = spawnPos10;

                playerIllusion[0].gameObject.transform.position = spawnPos01;
                playerIllusion[1].gameObject.transform.position = spawnPos11;


                break;
            case 1:

                playerBody[0].gravityDirection = PlayerActor.GravityDirection.Rightward;
                playerBody[1].gravityDirection = PlayerActor.GravityDirection.Rightward;

                playerIllusion[0].gravityDirection = PlayerActor.GravityDirection.Downward;
                playerIllusion[1].gravityDirection = PlayerActor.GravityDirection.Upward;

                playerBody[0].gameObject.transform.position = spawnPos10;
                playerBody[1].gameObject.transform.position = spawnPos11;

                playerIllusion[0].gameObject.transform.position = spawnPos00;
                playerIllusion[1].gameObject.transform.position = spawnPos01;
                break;

            case 2:

                playerBody[0].gravityDirection = PlayerActor.GravityDirection.Upward;
                playerBody[1].gravityDirection = PlayerActor.GravityDirection.Upward;

                playerIllusion[0].gravityDirection = PlayerActor.GravityDirection.Rightward;

                playerIllusion[1].gravityDirection = PlayerActor.GravityDirection.Leftward;

                playerBody[0].gameObject.transform.position = spawnPos11;
                playerBody[1].gameObject.transform.position = spawnPos01;

                playerIllusion[0].gameObject.transform.position = spawnPos10;
                playerIllusion[1].gameObject.transform.position = spawnPos00;
                break;

            case 3:

                playerBody[0].gravityDirection = PlayerActor.GravityDirection.Leftward;
                playerBody[1].gravityDirection = PlayerActor.GravityDirection.Leftward;

                playerIllusion[0].gravityDirection = PlayerActor.GravityDirection.Upward;

                playerIllusion[1].gravityDirection = PlayerActor.GravityDirection.Downward;

                playerBody[0].gameObject.transform.position = spawnPos01;
                playerBody[1].gameObject.transform.position = spawnPos10;

                playerIllusion[0].gameObject.transform.position = spawnPos11;
                playerIllusion[1].gameObject.transform.position = spawnPos10;



                break;
        }

    }
   
}

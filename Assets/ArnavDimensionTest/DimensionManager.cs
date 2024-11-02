using System.Collections;
using System.Collections.Generic;
using Bennet.MovementSystem;
using UnityEngine;

public class DimensionManager : MonoBehaviour
{
    public static DimensionManager Instance { get; private set; }

    [SerializeField]
    Dimension[] dimensions;

    [SerializeField]
    int outOfLoopDimension;

    int currentDimension;

    int totalDimensions;

    static Dimension duel, player1Illusion, player2Illusion;


    [SerializeField] Transform firstSpawnPoint;
    [SerializeField] float DistanceBetweenSpawnPoints = 10f;


    [SerializeField]  PlayerMovementController[]  players;

    Vector3 spawnPos00;
    Vector3 spawnPos10;
    Vector3 spawnPos01;
    Vector3 spawnPos11;

    Vector3[,] spawnPos = new Vector3[2, 2];

    [SerializeField]
    Transform FinalSpawnPos;
    [SerializeField]
    float finalPosDisT = 5f;

    int dimensionStreak;

    bool prevNext;

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
        spawnPos[0, 0] = firstSpawnPoint.position;
        spawnPos[1, 0] = new Vector3(spawnPos[0, 0].x + DistanceBetweenSpawnPoints, spawnPos[0, 0].y, spawnPos[0, 0].z);
        spawnPos[1, 1] = new Vector3(spawnPos[1, 0].x, spawnPos[1, 0].y + DistanceBetweenSpawnPoints, spawnPos[1, 0].z);
        spawnPos[0, 1] = new Vector3(spawnPos[1, 1].x - DistanceBetweenSpawnPoints, spawnPos[1, 1].y, spawnPos[1, 1].z);

        totalDimensions = dimensions.Length;

        currentDimension = -1;
        changeDimension(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setCurrentColor(Color col)
    {
        dimensions[currentDimension].GetComponent<DimensionWin>().changeToWinnerColor(col);
    }


    public void changeDimension(bool next)
    {
        if (prevNext == next)
        {
            dimensionStreak++;
        }
        else
        {
            dimensionStreak--;
        }

        prevNext = next;


        if (!(dimensionStreak >= outOfLoopDimension))
        {
            if (next)
                currentDimension = currentDimension = (currentDimension + 1) % outOfLoopDimension;
            else
                currentDimension = ((outOfLoopDimension - 1 + currentDimension) % outOfLoopDimension);

        }


        else
        {
            if (currentDimension == outOfLoopDimension - 1)
            {
                currentDimension = outOfLoopDimension + 1;
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


        int leftDimension = (outOfLoopDimension - 1 + currentDimension) % outOfLoopDimension;

        int rightDimension = (currentDimension + 1) % outOfLoopDimension;


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
            duel.Unregister(Dimension.SpecialDimension.Duel);
        }


        player1Illusion = dimensions[leftDimension];

        duel = dimensions[currentDimension];

        player2Illusion = dimensions[rightDimension];


        player1Illusion.SetAsDimension(Dimension.SpecialDimension.Player1Illusion);

        duel.SetAsDimension(Dimension.SpecialDimension.Duel);

        player2Illusion.SetAsDimension(Dimension.SpecialDimension.Player2Illusion);

        PlayerActor[] pB= { players[0].body, players[1].body };
        PlayerActor[] pI= { players[0].body, players[1].body };
        switch (currentDimension)
        {
            case 0:
                pB[0].GravityDirection = PlayerActor.GravityType.Downward;
                pB[1].GravityDirection = PlayerActor.GravityType.Downward;

                pB[0].GravityDirection = PlayerActor.GravityType.Leftward;
                pB[1].GravityDirection = PlayerActor.GravityType.Rightward;


                pB[0].setPosition(spawnPos[0,0]);
                pB[1].setPosition(spawnPos[1, 0]);

                pI[0].setPosition(spawnPos[0,1]);
                pI[1].setPosition(spawnPos[1,1]);


                break;
            case 1:

                pB[0].GravityDirection = PlayerActor.GravityType.Rightward;
                pB[1].GravityDirection = PlayerActor.GravityType.Rightward;

                pI[0].GravityDirection = PlayerActor.GravityType.Downward;
                pI[1].GravityDirection = PlayerActor.GravityType.Upward;

                pB[0].setPosition(spawnPos[1,0]);
                pB[1].setPosition(spawnPos[1,1]);

                pI[0].setPosition(spawnPos[0,0]);
                pI[1].setPosition(spawnPos[0,1]);

                break;

            case 2:

                playerBody[0].GravityDirection = PlayerActor.GravityType.Upward;
                playerBody[1].GravityDirection = PlayerActor.GravityType.Upward;

                playerIllusion[0].GravityDirection = PlayerActor.GravityType.Rightward;

                playerIllusion[1].GravityDirection = PlayerActor.GravityType.Leftward;

                playerBody[0].gameObject.transform.position = spawnPos11;
                playerBody[1].gameObject.transform.position = spawnPos01;

                playerIllusion[0].gameObject.transform.position = spawnPos10;
                playerIllusion[1].gameObject.transform.position = spawnPos00;
                break;

            case 3:

                playerBody[0].GravityDirection = PlayerActor.GravityType.Leftward;
                playerBody[1].GravityDirection = PlayerActor.GravityType.Leftward;

                playerIllusion[0].GravityDirection = PlayerActor.GravityType.Upward;

                playerIllusion[1].GravityDirection = PlayerActor.GravityType.Downward;

                playerBody[0].gameObject.transform.position = spawnPos01;
                playerBody[1].gameObject.transform.position = spawnPos10;

                playerIllusion[0].gameObject.transform.position = spawnPos11;
                playerIllusion[1].gameObject.transform.position = spawnPos10;
                break;

            case 4:

                playerBody[0].GravityDirection = PlayerActor.GravityType.Downward;
                playerBody[1].GravityDirection = PlayerActor.GravityType.Downward;



                playerBody[0].gameObject.transform.position = FinalSpawnPos.position;
                playerBody[1].gameObject.transform.position = new Vector3(FinalSpawnPos.position.x + finalPosDisT, FinalSpawnPos.position.y, FinalSpawnPos.position.z);

                break;
        }

    }
}
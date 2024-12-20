﻿using UnityEngine;

public class Dimension : MonoBehaviour
{
    [SerializeField] private int index;
    public int Index => index;

   
    public void SetAsDimension(SpecialDimension dimension)
    {
        int layer = 0;
        switch (dimension)
        {
            case SpecialDimension.Duel:
                //duel.Unregister(SpecialDimension.Duel);
                layer = LayerMask.NameToLayer("Duel Dimension");
                break;
            case SpecialDimension.Player1Illusion:
                //player1Illusion.Unregister(SpecialDimension.Player1Illusion);
                layer = LayerMask.NameToLayer("Player1 Illusion");
                break;
            case SpecialDimension.Player2Illusion:
                //player2Illusion.Unregister(SpecialDimension.Player2Illusion);
                layer = LayerMask.NameToLayer("Player2 Illusion");
                break;
        }

        gameObject.layer = layer;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.layer = layer;
    }

    public void Unregister()
    {
        int layer = LayerMask.NameToLayer("TransparentFX");
        gameObject.layer = layer;
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.layer = layer ;
    }


    public enum SpecialDimension
    {
        Duel,
        Player1Illusion,
        Player2Illusion,
    }


}
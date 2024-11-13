using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionWin : MonoBehaviour
{
    
    List<SpriteRenderer> allSprites;
    SpriteRenderer[] spritesChild;

    Color normColor;
    // Start is called before the first frame update
    void Awake()
    {
        spritesChild = GetComponentsInChildren<SpriteRenderer>();
        //CollectChildSpriteRenderers();

        normColor = spritesChild[0].color;
    }


    void CollectChildSpriteRenderers()
    {

        try
        {
            // Check if there are any children before iterating
            if (transform.childCount == 0)
            {
                return;
            }

            // Iterate through each child to find SpriteRenderer components
            foreach (Transform child in transform)
            {
                // Check if the child GameObject exists
                if (child != null)
                {
                    // Try to get a SpriteRenderer component
                    SpriteRenderer spriteRenderer;

                    // Add to list only if SpriteRenderer exists
                    if (child.TryGetComponent(out spriteRenderer))
                    {
                        if (!allSprites.Contains(spriteRenderer))
                            allSprites.Add(spriteRenderer);
                    }
                    else
                    {
                        Debug.LogWarning($"Child {child.name} has no SpriteRenderer component.");
                    }
                }
            }
        }

        catch(System.Exception e)
        {
            Debug.Log(e);
        }
        
    }


    // Update is called once per frame
   void Update()
   {
        

   }


    public void changeToWinnerColor(Color winCol)
    {
        if (winCol == Color.white)
        {
            winCol = normColor;
        }
        //CollectChildSpriteRenderers();
        SpriteRenderer sp;
       for(int i=0;i<spritesChild.Length;i++){
            sp = spritesChild[i]?spritesChild[i]:null;
            if(sp!=null)
            sp.color = winCol;
        }
    }

    public void setAlpha(float alphaVal)
    {
        SpriteRenderer sp;
        for (int i = 0; i < spritesChild.Length; i++)
        {
            sp = spritesChild[i] ? spritesChild[i] : null;
            if (sp != null)
            {
                Color originalColor = sp.color;
                sp.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaVal);
            }
                
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionWin : MonoBehaviour
{
    
    List<SpriteRenderer> allSprites;
    // Start is called before the first frame update
    void Start()
    {
        CollectChildSpriteRenderers();
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
        CollectChildSpriteRenderers();
        
       foreach (SpriteRenderer sp in allSprites){
            if(sp!=null)
            sp.color = winCol;
        }
    }
}

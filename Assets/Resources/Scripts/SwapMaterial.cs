using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapMaterial : MonoBehaviour
{
    public Material illuminated;
    public Material off;

    bool activated = false;

    public void Swap()
    {
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (activated)
            {
                activated = false;
                rend.material = off;
            }
            else
            {
                rend.material = illuminated;
                activated = true;
            }
        }
    }
}

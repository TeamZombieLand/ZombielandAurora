using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void SetLayer(this GameObject gameObject, int layer, bool includeChildren = false)
    {
        if (!gameObject) return;
        gameObject.layer = layer;
        if (!includeChildren)
        {           
            return;
        }

        foreach (var child in gameObject.GetComponentsInChildren(typeof(Renderer), true))
        {
            child.gameObject.layer = layer;
        }
    }
}

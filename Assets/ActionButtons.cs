using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButtons : MonoBehaviour {
    public List<GameObject> buttonsInOrder = new List<GameObject>();

    public void SetLevel(int level)
    {
        for(int i = 0; i < buttonsInOrder.Count; i++)
        {
            if (i < level)
                buttonsInOrder[i].SetActive(true);
            else
                buttonsInOrder[i].SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowAvailableTalent : MonoBehaviour
{
    private void CheckAvailableTalentPoints(int points)
    {
        if (points > 0) gameObject.GetComponent<CanvasGroup>().alpha = 1; 
        else gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    void Update()
    {
        CheckAvailableTalentPoints(TalentNodeManager.availableTalentPoints);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Character character;
    public TextMeshProUGUI stateText;

    private void Update()
    {
        if (character.functionTracks[0].clip != null) 
        stateText.text = character.functionTracks[0].clip.GetType().ToString();
    }
}

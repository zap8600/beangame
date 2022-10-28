using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

public class SceneScript : NetworkBehaviour
{
    public TMP_Text canvasStatusText;
    public PlayerScript playerScript;

    [SyncVar(hook = nameof(OnStatusTextChanged))]
    public string statusText;

    void OnStatusTextChanged(string _Old, string _New)
    {
        canvasStatusText.text = statusText;
    }

    public void ButtonSendMessage()
    {
        if (playerScript != null)
            playerScript.CmdSendPlayerMessage();
    }
}

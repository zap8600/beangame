using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class SceneScript : NetworkBehaviour
{
    public Text canvasStatusText;
    public PlayerScript playerScript;
	
	public InputField textInput;
	
    [SyncVar(hook = nameof(OnStatusTextChanged))]
    public string statusText;

    void OnStatusTextChanged(string _Old, string _New)
    {
        //called from sync var hook, to update info on screen for all players
        canvasStatusText.text = statusText;
    }

    public void ButtonSendMessage()
    {
        if (playerScript != null)  
            playerScript.CmdSendPlayerMessage();
    }
}
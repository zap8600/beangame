using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GROW : MonoBehaviour
{
	public PlayerScript playerScript;
	
	public void GROW() {
		if (playerScript != null)  
            playerScript.CmdGROW();
	}
}

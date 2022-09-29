using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class PlayerScript : NetworkBehaviour
{
	private float currentHeight;
	
	private SceneScript sceneScript;
	
	public TextMesh playerNameText;
	public GameObject floatingInfo;
	
	private Material playerMaterialClone;
	
	[SyncVar(hook = nameof(OnNameChanged))]
	public string playerName;
	
	[SyncVar(hook = nameof(OnColorChanged))]
	public Color playerColor = Color.white;
	
	void OnNameChanged(string _Old, string _New) {
		playerNameText.text = playerName;
	}
	
	void OnColorChanged(Color _Old, Color _New) {
		playerNameText.color = _New;
		playerMaterialClone = new Material(GetComponent<Renderer>().material);
		playerMaterialClone.color = _New;
		GetComponent<Renderer>().material = playerMaterialClone;
	}
	
	public Canvas canvas;
	public Joystick joystickL;
	public override void OnStartLocalPlayer() {
		sceneScript.playerScript = this;
		
		Camera.main.transform.SetParent(transform);
		Camera.main.transform.localPosition = new Vector3(0, 0, 0);
		
		floatingInfo.transform.localPosition = new Vector3(0, -0.3f, 0.6f);
        floatingInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        string name = "Player" + Random.Range(100, 999);
        Color color = new Color (Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        CmdSetupPlayer(name, color);
        		
		if(Application.platform == RuntimePlatform.Android) {
			canvas.GetComponent<Canvas>().enabled = true;
		}
	}
	
	void Awake()
	{
    		//allow all players to run this
    		sceneScript = GameObject.FindObjectOfType<SceneScript>();
	}
    	[Command]
	public void CmdSetupPlayer(string _name, Color _col) {
		playerName = _name;
		playerColor = _col;
		sceneScript.statusText = $"{playerName} joined.";
	}
	
	[Command]
	public void CmdSendPlayerMessage()
	{
		if (sceneScript) 
			sceneScript.statusText = $"{playerName} says hello.";
	}
	
	void Update() {
		float moveX;
		float moveZ;
		if(!isLocalPlayer) {
			floatingInfo.transform.LookAt(Camera.main.transform);
			return;
		}
		
		if(Application.platform == RuntimePlatform.Android) {
			moveX = joystickL.Horizontal * Time.deltaTime * 110.0f;
			moveZ = joystickL.Vertical * Time.deltaTime * 4f;
		} else {
			moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
			moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;
		}
		transform.Rotate(0, moveX, 0);
		transform.Translate(0, 0, moveZ);
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerScript : NetworkBehaviour
{
	private int selectedWeaponLocal = 1;
	public GameObject[] weaponArray;

	[SyncVar(hook = nameof(OnWeaponChanged))]
	public int activeWeaponSynced = 1;

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
		foreach (var item in weaponArray)
        	if (item != null)
            		item.SetActive(false);
 
		//allow all players to run this
		sceneScript = GameObject.FindObjectOfType<SceneScript>();
	}
	
	void OnWeaponChanged(int _Old, int _New)
	{
    	// disable old weapon
    	// in range and not null
    	if (0 < _Old && _Old < weaponArray.Length && weaponArray[_Old] != null)
        	weaponArray[_Old].SetActive(false);
    
    	// enable new weapon
    	// in range and not null
    	if (0 < _New && _New < weaponArray.Length && weaponArray[_New] != null)
        	weaponArray[_New].SetActive(true);
	}

	[Command]
	public void CmdChangeActiveWeapon(int newIndex)
	{
    		activeWeaponSynced = newIndex;
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
		
		if (Input.GetButtonDown("Fire2")) //Fire2 is mouse 2nd click and left alt
    		{
        		swapWeapon();
    		}
	}

	public void swapWeapon() {
		selectedWeaponLocal += 1;

        	if (selectedWeaponLocal > weaponArray.Length) 
            		selectedWeaponLocal = 1; 

        	CmdChangeActiveWeapon(selectedWeaponLocal);
	}
}
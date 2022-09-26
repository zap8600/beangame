using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject PanelStart;
    public GameObject PanelStop;

    public Button buttonHost, buttonServer, buttonClient, buttonStop;

    public InputField inputFieldAddress;

    public Text serverText;
    public Text clientText;
    
    // Start is called before the first frame update
    void Start()
    {
        buttonHost.onClick.AddListener(ButtonHost);
    }
    
    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
    }
}

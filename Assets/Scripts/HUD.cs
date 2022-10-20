using Mirror;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUD : MonoBehaviour
{
    public GameObject PanelStart;
    public GameObject PanelStop;
    public GameObject PanelChat;
    public GameObject PanelServer;
    
    public GameObject PanelWebGLCheck;

    public Button buttonHost, buttonServer, buttonClient, buttonStop;

    public TMP_InputField inputFieldAddress;

    public TMP_Text serverText;
    public TMP_Text clientText;
    
    // Start is called before the first frame update
    void Start()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            PanelWebGLCheck.SetActive(false);
        }
        
        if (NetworkManager.singleton.networkAddress != "localhost") { inputFieldAddress.text = NetworkManager.singleton.networkAddress; }
        
        inputFieldAddress.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        
        buttonHost.onClick.AddListener(ButtonHost);
        buttonServer.onClick.AddListener(ButtonServer);
        buttonClient.onClick.AddListener(ButtonClient);
        buttonStop.onClick.AddListener(ButtonStop);
        
        SetupCanvas();
    }
    
    public void ValueChangeCheck()
    {
        NetworkManager.singleton.networkAddress = inputFieldAddress.text;
    }
    
    public void ButtonHost()
    {
        NetworkManager.singleton.StartHost();
        SetupCanvas();
    }
    
    public void ButtonServer()
    {
        NetworkManager.singleton.StartServer();
        SetupCanvas();
    }

    public void ButtonClient()
    {
        NetworkManager.singleton.StartClient();
        SetupCanvas();
    }

    public void ButtonStop()
    {
        // stop host if host mode
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopHost();
        }
        // stop client if client-only
        else if (NetworkClient.isConnected)
        {
            NetworkManager.singleton.StopClient();
        }
        // stop server if server-only
        else if (NetworkServer.active)
        {
            NetworkManager.singleton.StopServer();
        }
        SceneManager.LoadScene("SampleScene");
    }

    public void SetupCanvas()
    {
        // Here we will dump majority of the canvas UI that may be changed.

        if (!NetworkClient.isConnected && !NetworkServer.active)
        {
            if (NetworkClient.active)
            {
                PanelStart.SetActive(false);
                PanelStop.SetActive(true);
                PanelChat.SetActive(true);
                clientText.text = "Client: Address=" + NetworkManager.singleton.networkAddress;
                serverText.text = "Server: Client Only.";
            }
            else
            {
                PanelStart.SetActive(true);
                PanelStop.SetActive(false);
            }
        }
        else if (!NetworkClient.active && NetworkServer.active)
        {
            PanelStart.SetActive(false);
            PanelStop.SetActive(true);
            PanelChat.SetActive(true);
            PanelServer.SetActive(false);

            clientText.text = "Client: Server Only.";
            serverText.text = "Server: Active.";
        }
        else
        {
            PanelChat.SetActive(true);
            PanelStart.SetActive(false);
            PanelStop.SetActive(true);

            // server / client status message
            if (NetworkServer.active)
            {
                serverText.text = "Server: Active";
            }
            if (NetworkClient.isConnected)
            {
                clientText.text = "Client: Address=" + NetworkManager.singleton.networkAddress;
            }
        }
    }
}

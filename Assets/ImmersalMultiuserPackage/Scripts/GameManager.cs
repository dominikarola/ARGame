using Immersal;
using Immersal.XR;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private GameObject multiuserUI;
    [SerializeField] private NetworkDragonSpawner dragonSpawner;

    private ImmersalSDK immersalSDK;
    private NetworkManager networkManager;
    private Localizer localizer;

    private bool isSessionPaused;

    private void OnEnable()
    {
        multiuserUI.SetActive(false);
    }

    private void Start()
    {
        immersalSDK = ImmersalSDK.Instance;

        localizer = GameObject.FindObjectOfType<Localizer>();
        localizer.OnFirstSuccessfulLocalization.AddListener(OnSuccessfulLocalizations);

        networkManager = NetworkManager.Instance;
        networkManager.onPlayerJoinedEvent.AddListener(OnPlayerJoined);
    }

    private void OnSuccessfulLocalizations()
    {
        multiuserUI.SetActive(true);
    }

    private void OnPlayerJoined()
    {
        multiuserUI.SetActive(false);
        dragonSpawner.SpawnDragon();
    }

    private void Update()
    {
        int q = immersalSDK.TrackingStatus?.TrackingQuality ?? 0;

        if (q >= 1 && !isSessionPaused)
        {
            immersalSDK.Session.PauseSession();
            isSessionPaused = true;
            debugText.text += "\n Localizer Paused.";
        }
        else if (q < 1 && isSessionPaused)
        {
            immersalSDK.Session.ResumeSession();
            isSessionPaused = false;
            debugText.text += "\n Localizer Resumed.";
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TrackSelector : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Dropdown trackDropdown;
    [SerializeField] private Button startRaceButton;

    [Header("Track Settings")]
    [SerializeField] private string[] trackSceneNames;
    [SerializeField] private string[] trackNames;

    void Start()
    {
        if (trackDropdown != null)
        {
            trackDropdown.ClearOptions();
            trackDropdown.AddOptions(new System.Collections.Generic.List<string>(trackNames));
        }

        if (startRaceButton != null)
        {
            startRaceButton.onClick.AddListener(StartRace);
        }
    }

    void StartRace()
    {
        int selectedTrack = trackDropdown != null ? trackDropdown.value : 0;

        if (selectedTrack < trackSceneNames.Length)
        {
            SceneManager.LoadScene(trackSceneNames[selectedTrack]);
        }
    }

    public void BackToTrackSelection()
    {
        SceneManager.LoadScene("TrackSelection");
    }
}
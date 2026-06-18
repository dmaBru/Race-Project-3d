using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI finishTimeText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;

    [Header("Records UI")]
    [SerializeField] private GameObject recordsPanel;
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button saveRecordButton;
    [SerializeField] private Transform recordsContainer;
    [SerializeField] private GameObject recordPrefab;
    [SerializeField] private Button skipRecordButton;

    [Header("Finish Settings")]
    [SerializeField] private GameObject finishObject;

    [Header("Car Controller")]
    [SerializeField] private MonoBehaviour carController;
    [SerializeField] private Transform startPosition;

    private RecordsManager recordsManager;
    private bool isGameStarted = false;
    private bool isFinished = false;
    private float timer = 0f;
    private float finishTime = 0f;
    private Vector3 carStartPosition;
    private Quaternion carStartRotation;

    private string currentTrackName;

    void Start()
    {
        recordsManager = GetComponent<RecordsManager>();
        if (recordsManager == null)
        {
            recordsManager = gameObject.AddComponent<RecordsManager>();
        }

        currentTrackName = SceneManager.GetActiveScene().name;

        Time.timeScale = 0f;
        isGameStarted = false;
        isFinished = false;
        timer = 0f;

        if (carController != null)
        {
            carStartPosition = carController.transform.position;
            carStartRotation = carController.transform.rotation;
        }

        startButton.onClick.AddListener(StartCountdown);

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
            restartButton.gameObject.SetActive(false);
        }

        if (saveRecordButton != null)
        {
            saveRecordButton.onClick.AddListener(SaveRecord);
            saveRecordButton.gameObject.SetActive(false);
        }

        if (skipRecordButton != null)
        {
            skipRecordButton.onClick.AddListener(SkipRecord);
            skipRecordButton.gameObject.SetActive(false);
        }

        if (nameInputField != null)
        {
            nameInputField.gameObject.SetActive(false);
        }

        if (recordsPanel != null)
        {
            recordsPanel.SetActive(false);
        }

        if (finishTimeText != null)
        {
            finishTimeText.gameObject.SetActive(false);
        }

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        if (menuButton != null)
        {
            menuButton.onClick.AddListener(BackToMenu);
            menuButton.gameObject.SetActive(false);
        }

        timerText.text = "00:00:00";
        SetTimerDisplay(timer);
    }

    void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TrackSelection");
    }

    void Update()
    {
        if (isGameStarted && !isFinished)
        {
            timer += Time.unscaledDeltaTime;
            SetTimerDisplay(timer);
        }
    }

    void StartCountdown()
    {
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }

        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
        }

        for (int i = 3; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = i.ToString();
            }
            yield return new WaitForSecondsRealtime(1f);
        }

        if (countdownText != null)
        {
            countdownText.text = "GO!";
        }
        yield return new WaitForSecondsRealtime(0.5f);

        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }

        StartGame();
    }

    void StartGame()
    {
        isGameStarted = true;
        isFinished = false;
        timer = 0f;
        Time.timeScale = 1f;

        if (finishTimeText != null)
        {
            finishTimeText.gameObject.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }

        if (recordsPanel != null)
        {
            recordsPanel.SetActive(false);
        }

        SetTimerDisplay(0f);
    }

    public void FinishRace()
    {
        if (!isFinished && isGameStarted)
        {
            isFinished = true;
            finishTime = timer;

            Time.timeScale = 0f;

            ShowRecordsTable();

            if (finishTimeText != null)
            {
                timerText.gameObject.SetActive(false);
                finishTimeText.gameObject.SetActive(true);

                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer % 60f);
                int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f);

                finishTimeText.text = string.Format("{0:00}:{1:00}:{2:00}",
                    minutes, seconds, milliseconds);
            }

            if (recordsManager.IsRecordWorthy(finishTime))
            {
                ShowRecordInput();
            }

            if (restartButton != null)
            {
                    restartButton.gameObject.SetActive(true);
            }

            if (menuButton != null)
            {
                menuButton.gameObject.SetActive(true);
            }
        }
    }

    void ShowRecordInput()
    {
        if (nameInputField != null)
        {
            nameInputField.gameObject.SetActive(true);
            nameInputField.text = "";
        }

        if (skipRecordButton != null)
        {
            skipRecordButton.gameObject.SetActive(true);
        }

        if (saveRecordButton != null)
        {
            saveRecordButton.gameObject.SetActive(true);
        }

    }

    void SaveRecord()
    {
        string playerName = nameInputField.text;
        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player";
        }

        recordsManager.AddRecord(playerName, finishTime);


        HideRecordInput();

        UpdateRecordsDisplay();
    }

    void SkipRecord()
    {
        HideRecordInput();
    }

    void HideRecordInput()
    {
        if (nameInputField != null)
        {
            nameInputField.gameObject.SetActive(false);
        }

        if (saveRecordButton != null)
        {
            saveRecordButton.gameObject.SetActive(false);
        }

        if (skipRecordButton != null)
        {
            skipRecordButton.gameObject.SetActive(false);
        }
    }

    void ShowRecordsTable()
    {
        if (recordsPanel != null)
        {
            recordsPanel.SetActive(true);
            UpdateRecordsDisplay();
        }
    }

    void UpdateRecordsDisplay()
    {
        foreach (Transform child in recordsContainer)
        {
            Destroy(child.gameObject);
        }

        List<RecordData> records = recordsManager.GetRecords();

        if (records.Count == 0)
        {
            GameObject emptyMessage = new GameObject("EmptyMessage");
            emptyMessage.transform.SetParent(recordsContainer, false);

            TextMeshProUGUI textComponent = emptyMessage.AddComponent<TextMeshProUGUI>();
            textComponent.fontSize = 24;
            textComponent.color = Color.gray;
            textComponent.alignment = TextAlignmentOptions.Center;
            return;
        }

        for (int i = 0; i < records.Count; i++)
        {
            GameObject recordEntry;

            if (recordPrefab != null)
            {
                recordEntry = Instantiate(recordPrefab, recordsContainer);
            }
            else
            {
                recordEntry = new GameObject($"Record_{i + 1}");
                recordEntry.transform.SetParent(recordsContainer, false);
                recordEntry.AddComponent<TextMeshProUGUI>();
            }

            TextMeshProUGUI textComponent = recordEntry.GetComponent<TextMeshProUGUI>();
            if (textComponent == null)
            {
                textComponent = recordEntry.GetComponentInChildren<TextMeshProUGUI>();
            }

            if (textComponent != null)
            {
                textComponent.text = string.Format("{0}. {1} - {2}",
                    i + 1,
                    records[i].playerName,
                    records[i].formattedTime);
            }
        }
    }


    void RestartGame()
    {
        if (carController != null)
        {
            carController.transform.position = carStartPosition;
            carController.transform.rotation = carStartRotation;

            Rigidbody rb = carController.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }

        if (restartButton != null)
        {
            restartButton.gameObject.SetActive(false);
        }

        if (recordsPanel != null)
        {
            recordsPanel.SetActive(false);
        }

        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }

        if (finishTimeText != null)
        {
            finishTimeText.gameObject.SetActive(false);
        }

        if (menuButton != null)
        {
            menuButton.gameObject.SetActive(false);
        }

        HideRecordInput();

        StartCountdown();
    }

    void SetTimerDisplay(float timeToShow)
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeToShow / 60f);
            int seconds = Mathf.FloorToInt(timeToShow % 60f);
            int milliseconds = Mathf.FloorToInt((timeToShow * 100f) % 100f);

            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    public float GetCurrentTime()
    {
        return timer;
    }
}
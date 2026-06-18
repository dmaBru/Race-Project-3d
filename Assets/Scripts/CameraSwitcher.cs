using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public GameObject[] cameras;
    private int currentCameraIndex = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i].SetActive(i == 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            cameras[currentCameraIndex].SetActive(false);
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Length;
            cameras[currentCameraIndex].SetActive(true);
        }
    }
}

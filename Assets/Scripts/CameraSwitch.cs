using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    public GameObject FPCamera;
    public GameObject TPCamera;

    private bool isFirstPerson = true;

    void Start()
    {
        FPCamera.SetActive(true);
        TPCamera.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPerson = !isFirstPerson;

            FPCamera.SetActive(isFirstPerson);
            TPCamera.SetActive(!isFirstPerson);
        }
    }
}
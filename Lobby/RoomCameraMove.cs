using UnityEngine;
using UnityEngine.UI;

public class RoomCameraMove : MonoBehaviour
{
    public Button rightButton;
    public Button leftButton;
    public Transform cameraPos;
    Vector3 cameraPosition; // 카메라 위치
    Vector3 cameraRotation; // 카메라 각도
    void Start()
    {
        rightButton.onClick.AddListener(OnRightButtonClick);
        leftButton.onClick.AddListener(OnLeftButtonClick);
        int random = Random.Range(0, 10);

        if (random < 5) // 반반 확률로 로비 띄우기
        {
            cameraPosition = new Vector3(-2f, 2.5f, -8.138f);
            cameraRotation = new Vector3(20f, 0, 0);
            cameraPos.transform.position = cameraPosition;
            cameraPos.transform.rotation = Quaternion.Euler(cameraRotation);
        }
        else
        {
            cameraPosition = new Vector3(-32.6f, 2.5f, -6.53f);
            cameraRotation = new Vector3(7.7f, 0, 0);
            cameraPos.transform.position = cameraPosition;
            cameraPos.transform.rotation = Quaternion.Euler(cameraRotation);
        }

        if (cameraPos.transform.position.x == -32.6f)
        {
            rightButton.interactable = false;
        }
        else
        {
            leftButton.interactable = false;
        }
    }

    void OnLeftButtonClick()
    {
        if (cameraPos.transform.position.x == -32.6f)
        { 
            cameraPosition = new Vector3(-2f, 2.5f, -8.138f);
            cameraRotation = new Vector3(20f, 0, 0);

            cameraPos.transform.position = cameraPosition;
            cameraPos.transform.rotation = Quaternion.Euler(cameraRotation);

            leftButton.interactable = false;
            rightButton.interactable = true;
        }
    }

    void OnRightButtonClick()
    {
        if (cameraPos.transform.position.x == -2f)
        { 
            cameraPosition = new Vector3(-32.6f, 2.5f, -6.53f);
            cameraRotation = new Vector3(7.7f, 0, 0);

            cameraPos.transform.position = cameraPosition;
            cameraPos.transform.rotation = Quaternion.Euler(cameraRotation);

            rightButton.interactable = false;
            leftButton.interactable = true;
        }
    }
}

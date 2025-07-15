using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    float _scaleSpeed = 3f;
    float _scaleAmount = 0.08f;
    Vector3 _originalScale;

    public TMP_Text startText;

    private void Start()
    {
        _originalScale = startText.transform.localScale;
    }

    void Update()
    {
//        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
//        {
//            LoadLobby();
//        }

//#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            LoadLobby();
        }
//#endif

        float scaleOffset = Mathf.Sin(Time.time * _scaleSpeed) * _scaleAmount;
        startText.transform.localScale = _originalScale + Vector3.one * scaleOffset;
    }

    void LoadLobby()
    {
        SceneManager.LoadScene(Define.Lobby);
    }

    
}

using UnityEngine;
using UnityEngine.UI;

public class BlurController : MonoBehaviour
{
    private Text buttonText;
    private bool isBlurEnabled;
    [SerializeField]
    private GameObject blurObject;

    void Awake()
    {
        buttonText = GetComponentInChildren<Text>();
    }
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(BlurButtonPress);
        //gameObject.GetComponent<Button>().interactable = false;
        //blurObject.SetActive(false);
        isBlurEnabled = true;
    }
    void BlurButtonPress()
    {
        if (!isBlurEnabled)
        {
            buttonText.text = "Blur ON";
            blurObject.SetActive(true);
        }
        else
        {
            buttonText.text = "Blur OFF";
            blurObject.SetActive(false);
        }

        isBlurEnabled = !isBlurEnabled;
    }
}

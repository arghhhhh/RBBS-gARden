using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class ITNotificationPanel : MonoBehaviour
{
    private ITReferencePasser objectReference;
    private AudioManager audioManager;

    public Button playButton;
    public Button stopButton;
    public Image playImage;
    public Image exitImage;
    public Sprite playSprite;
    public Sprite pauseSprite;
    public Sprite exitSprite;
    public Sprite stopSprite;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        gameObject.SetActive(false);
        playButton.onClick.AddListener(PlayButtonPress);
        stopButton.onClick.AddListener(ExitButtonPress); 
    }
    private void PlayButtonPress()
    {
        StopAllCoroutines();
        //the object reference here is the prefab generated from the tracked image
        if (objectReference != null)
        {
            bool animatorState = objectReference.GetComponent<Animator>().GetBool("isSpinning");
            if (!animatorState)
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", true);
                playImage.sprite = pauseSprite;
                exitImage.sprite = stopSprite;
                audioManager.PlaySound(objectReference.name);
            }
            else
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", false);
                playImage.sprite = playSprite;
                exitImage.sprite = exitSprite;
                audioManager.PauseSound(objectReference.name);
                EventSystem.current.SetSelectedGameObject(null); //disables button selected color
            }
        }
        else return;
    }

    private void ExitButtonPress()
    {
        //the object reference here is the prefab generated from the tracked image
        if (objectReference != null)
        {
            if (!audioManager.IsSoundPlaying(objectReference.name))
            {
                gameObject.SetActive(false);
            }
            else
            {
                audioManager.StopSound(objectReference.name);
            }
        }
        else return;
    }

    public void GiveReference(ITReferencePasser _newObject)
    {
        objectReference = _newObject;

        objectReference.EnabledEvent += EnablePanel;
        //objectReference.DisabledEvent += DisablePanel;
    }

    void EnablePanel()
    {
        if (!gameObject.activeSelf) //no need to set active if it's already active
        {
            gameObject.SetActive(true);
            StartCoroutine(HideObjectAfterDelay(10f)); //Disables button after X amount of seconds
        }
        {
            //if (audioManager.IsSoundPlaying(objectReference.name))
            //    StartCoroutine(LerpUp(2f));
        }
    }

    void DisablePanel()
    {
        //if (audioManager.IsSoundPlaying(objectReference.name))
        //    StartCoroutine(LerpDown(5f));
    }

    private void OnDestroy()
    {
        objectReference.EnabledEvent -= EnablePanel;
        //objectReference.DisabledEvent -= DisablePanel;
    }

    IEnumerator HideObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }

    public IEnumerator Wait(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}

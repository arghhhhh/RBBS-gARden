using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class PlayButtonController : MonoBehaviour
{
    private PlayButtonReferencePasser objectReference;
    private AudioManager audioManager;

    [SerializeField]
    private Sprite playSprite;
    [SerializeField]
    private Sprite stopSprite;

    [SerializeField]
    private AudioMixerGroup defaultMixerGroup;
    [SerializeField]
    private AudioMixerGroup lowpassMixerGroup;
    [SerializeField]
    private AudioMixerGroup lerpdownMixerGroup;
    [SerializeField]
    private AudioMixerGroup lerpupMixerGroup;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SpinButtonPress); //add a listener for button press

    }
    private void SpinButtonPress()
    {
        //the object reference here is the 3D leaf logo
        if (objectReference != null)
        {
            bool animatorState = objectReference.GetComponent<Animator>().GetBool("isSpinning");
            if (!animatorState)
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", true);
                gameObject.GetComponent<Image>().sprite = stopSprite;
                audioManager.PlaySound(objectReference.name);
            }
            else
            {
                objectReference.GetComponent<Animator>().SetBool("isSpinning", false);
                gameObject.GetComponent<Image>().sprite = playSprite;
                audioManager.StopSound(objectReference.name);
                EventSystem.current.SetSelectedGameObject(null); //disables button selected color
            }
        }
        else return;
    }

    public void GiveReference(PlayButtonReferencePasser _newObject)
    {
        objectReference = _newObject;

        objectReference.EnabledEvent += EnableButton;
        objectReference.DisabledEvent += DisableButton;
    }

    void EnableButton()
    {
        gameObject.SetActive(true);
        //StartCoroutine(Lerper(1.0f, true));
        //Lerper(20.0f, true);
        audioManager.ChangeAudioMixerGroup(objectReference.name, defaultMixerGroup);
    }

    void DisableButton()
    {
        gameObject.SetActive(false);
        //StartCoroutine(Lerper(2.0f, false));
        //Lerper(20.0f, false);
        audioManager.ChangeAudioMixerGroup(objectReference.name, lowpassMixerGroup);
    }

    private void OnDestroy()
    {
        objectReference.EnabledEvent -= EnableButton;
        objectReference.DisabledEvent -= DisableButton;
    }

    IEnumerator Lerper(float fadeTime, bool direction)
    {
        if (!direction) audioManager.ChangeAudioMixerGroup(objectReference.name, lerpdownMixerGroup);
        else audioManager.ChangeAudioMixerGroup(objectReference.name, lerpupMixerGroup);

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            if (!direction) //lerp down from 5000 to 537
            {
                lerpdownMixerGroup.audioMixer.SetFloat("LerpDownLP", Mathf.Lerp(5000f, 537f, elapsedTime / (fadeTime)));
            }
            else //lerp up from 537 to 5000
            {
                lerpupMixerGroup.audioMixer.SetFloat("LerpUpLP", Mathf.Lerp(537f, 5000f, elapsedTime / (fadeTime)));
            }
            yield return null;
        }
        if (!direction) audioManager.ChangeAudioMixerGroup(objectReference.name, lowpassMixerGroup);
        else audioManager.ChangeAudioMixerGroup(objectReference.name, defaultMixerGroup);

        yield return null;
    }
}

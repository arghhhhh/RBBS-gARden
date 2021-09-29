using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class PlayButtonController : MonoBehaviour
{
    private ITReferencePasser objectReference;
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
        GetComponent<Button>().onClick.AddListener(PlayButtonPress); //add a listener for button press
    }
    private void PlayButtonPress()
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

    public void GiveReference(ITReferencePasser _newObject)
    {
        objectReference = _newObject;

        objectReference.EnabledEvent += EnableButton;
        objectReference.DisabledEvent += DisableButton;
    }

    void EnableButton()
    {
        gameObject.SetActive(true);
        //if (audioManager.IsSoundPlaying(objectReference.name))
        //    StartCoroutine(LerpUp(2f));
    }

    void DisableButton()
    {
        //Create timer function that disables button after X amount of seconds
        gameObject.SetActive(false);
        //if (audioManager.IsSoundPlaying(objectReference.name))
        //    StartCoroutine(LerpDown(5f));
    }

    private void OnDestroy()
    {
        objectReference.EnabledEvent -= EnableButton;
        objectReference.DisabledEvent -= DisableButton;
    }

    IEnumerator LerpDown(float fadeTime)
    {
        audioManager.ChangeAudioMixerGroup(objectReference.name, lerpdownMixerGroup);

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            lerpdownMixerGroup.audioMixer.SetFloat("LerpDownLP", Mathf.Lerp(5000f, 537f, elapsedTime / (fadeTime)));
            yield return null;
        }
        audioManager.ChangeAudioMixerGroup(objectReference.name, lowpassMixerGroup);
    }
    IEnumerator LerpUp(float fadeTime)
    {
        audioManager.ChangeAudioMixerGroup(objectReference.name, lerpupMixerGroup);

        float elapsedTime = 0;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            lerpupMixerGroup.audioMixer.SetFloat("LerpUpLP", Mathf.Lerp(537f, 5000f, elapsedTime / (fadeTime)));

            yield return null;
        }
        audioManager.ChangeAudioMixerGroup(objectReference.name, defaultMixerGroup);
    }
}

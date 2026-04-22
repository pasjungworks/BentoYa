using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition_bento_Manager : MonoBehaviour
{
    public GameObject main;
    public GameObject cook;
    public GameObject cookReview;
    public GameObject bento;

    public CanvasGroup fadeOverlay;
    public FreePackBento freePackBento;

    public string nextSceneName;
    public bool skipScene = false;

    private void Start()
    {
        main.SetActive(true);
        cook.SetActive(false);
        bento.SetActive(false);
        cookReview.SetActive(false);
    }
    public void TransitionToMinigame()
    {
        Play_MiniGame.instance.canPlay = true;

        StartCoroutine(SequenceCook());

    }
    public void TransitionToCookReview()
    {
        Play_MiniGame.instance.canPlay = false;
        StartCoroutine(SequenceCookReview());
        freePackBento.packAble = false;
    }
    public void TransitionToBento()
    {
        StartCoroutine(SequenceBento());
        freePackBento.packAble = true;
    }
    public void GoLobby()
    {
        Lobby();
    }
    IEnumerator Lobby()
    {
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(0, 1, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("Lobby");

        elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(1, 0, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator SequenceCook()
    {
        if (skipScene)
        {
            TransitionToBento();
            yield break;
        }
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(0, 1, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        main.gameObject.SetActive(false);
        //Debug.Log(mainCamera.enabled);
        cook.gameObject.SetActive(true);
        //Debug.Log(bentoCamera.enabled);


        yield return new WaitForSeconds(0.2f);

        elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(1, 0, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator SequenceCookReview()
    {
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(0, 1, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cook.gameObject.SetActive(false);
        cookReview.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(1, 0 , elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator SequenceBento()
    {
        float elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(0, 1, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }


        //Debug.Log(mainCamera.enabled);
        main.gameObject.SetActive(false);
        cookReview.gameObject.SetActive(false);
        bento.gameObject.SetActive(true);
        //Debug.Log(bentoCamera.enabled);



        yield return new WaitForSeconds(0.2f);

        elapsed = 0;
        while (elapsed < 0.5f)
        {
            fadeOverlay.alpha = Mathf.Lerp(1, 0, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}

﻿using System.Collections;
using UnityEngine;

public class SaveBlock : MonoBehaviour {

    [TextArea]
    public string[] genText;
    [TextArea]
    public string[] saveText;
    [TextArea]
    public string[] errorText;

    public GameObject speechBubble;
    public GameObject saveMenu;

    public AudioClip talkSound;
    public AudioClip skipSound;
    public AudioClip blockSound;

    public Color textTint;
    public float textDelay;

    private RectTransform uiParent;
    private PlayerMachine player;
    private GameObject currentBubble;
    private Animator animator;

    private bool blockAnimPlaying;

    void Start() {
        uiParent = GameObject.FindGameObjectWithTag("UIParent").GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMachine>();
        animator = gameObject.GetComponent<Animator>();
    }

    private void pageFinished(int page){
        SaveMenu saveMenuObject = Instantiate(saveMenu, uiParent).GetComponentInChildren<SaveMenu>();
        saveMenuObject.saveText = saveText;
        saveMenuObject.errorText = errorText;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && currentBubble == null && !blockAnimPlaying) {
            blockAnimPlaying = true;
            player.setCutsceneMode(true);
            StartCoroutine(waitForBlockAnimation());
        }
    }

    IEnumerator waitForBlockAnimation(){
        animator.SetTrigger("Hit");
        yield return new WaitForSeconds(textDelay);
        GameObject bubble = Instantiate(speechBubble, uiParent);
        Typewriter writer = bubble.GetComponent<Typewriter>();
        writer.talkSound = talkSound;
        writer.skipSound = skipSound;
        writer.bubbleText.color = textTint;
        writer.OnPageFinished += pageFinished;
        writer.StartWriting(genText);
        currentBubble = bubble;
        blockAnimPlaying = false;
    }
}
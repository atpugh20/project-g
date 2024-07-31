using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript2 : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    public Animator animator;

    private int index;


    public GameObject text;
    public GameObject image1;

    private bool playfirsttext = true;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
                text.SetActive(false);
                image1.SetActive(false);
                this.GetComponent<Image>().enabled = false;
            if (playfirsttext) { 
                StartCoroutine(yoink());
            }
                
                //gameObject.SetActive(false);

            
        }
    }

    private IEnumerator yoink()
    {
        animator.SetTrigger("isGrabbing");
        yield return new WaitForSeconds(2f);
        text.SetActive(true);
        image1.SetActive(true);
        this.GetComponent<Image>().enabled = true;
        playfirsttext = false;
        string[] templines = {"Well... that was way easier than I expected. AND they put the exit right in front of me? This is the best!"};
        lines = templines;
        textComponent.text = string.Empty;
        StartDialogue();
    }

}

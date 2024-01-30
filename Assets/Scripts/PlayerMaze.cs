using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMaze : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI textBox;

    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;
    private float speed = 3;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        panel.SetActive(true);

        if (col.gameObject.tag == "c")
        {
            textBox.text = "Congratulations on a whimsical triumph! The jester's antics and wit have successfully delivered the coveted egg to the chef. A truly jestful victory!";
        }
        else if (col.gameObject.tag == "w")
        {
            textBox.text = "Well, well, well! Looks like our beloved jester has stumbled upon the crabby old wizard. Who knew mischief had consequences? But hey, who needs a kingdom when you despise the jester gig anyway? On to more thrilling misadventures, right?";
        }
        else if (col.gameObject.tag == "r")
        {
            textBox.text = "Behold the audacious jester, mastermind of mirth and mayhem! With a daring move, the crown snatched from the despondent king, an uprising sparked, and now the kingdom thrives under the jester's rule. Long live the jestful revolution!";
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
    }
}

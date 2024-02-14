using TMPro;
using UnityEngine;

public class TreeIncomScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private bool canAddScore = true;
    private float scoreMultiplier;
    private int score;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        scoreText = GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if(canAddScore)
        {
            canAddScore = false;
            Invoke("addScore", 1);
        }
    }
    private void addScore()
    {
        score = (int) Mathf.Pow(100, 0.65f * scoreMultiplier);

        animator.SetTrigger("Squish");

        ScorePopUpSpawner.instance.SpawnPopup(new Vector3(transform.position.x, transform.position.y + transform.GetChild(3).transform.position.y, transform.position.z), score.ToString(), true);

        scoreText.text = (int.Parse(scoreText.text) + score).ToString();
        canAddScore = true;
    }
    public void setScoreMultiplier(float multiplier)
    {
        scoreMultiplier = multiplier;
    }
}

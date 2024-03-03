using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class TreeIncomScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private bool canAddScore = true;
    private float scoreMultiplier;
    private int score;
    private Animator animator;
    private float timerValue;
    private float t = 0.0f;
    [SerializeField] private AudioSource incomSound;

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

        timerValue = Mathf.Lerp(0, 1, t);
        transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Slider>().value = timerValue;
        transform.GetChild(4).transform.LookAt(Camera.main.transform);
        t += Time.deltaTime;

        if (t > 1.0f)
        {
            t = 0.0f;
        }
    }
    private void addScore()
    {
        incomSound.pitch = (Random.Range(0.9f, 1.1f));
        incomSound.Play();

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

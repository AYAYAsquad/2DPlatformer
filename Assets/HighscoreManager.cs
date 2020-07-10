using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreManager : MonoBehaviour
{
  public int highscore_one;
  public float highscore_time;
  [SerializeField] Text highscoreText;
  [SerializeField] Text highscoreTime;

  private End end;
  // Start is called before the first frame update
  // void Start()
  // {
  //   PlayerPrefs.DeleteAll();
  // }
  void Awake()
  {
    if (PlayerPrefs.HasKey("Score"))
    {
      highscore_one = PlayerPrefs.GetInt("Score");
      highscoreText.text = highscore_one.ToString();
    }
    if (PlayerPrefs.HasKey("Time"))
    {
      highscore_time = PlayerPrefs.GetFloat("Time");
      highscoreTime.text = highscore_time.ToString("0.0");
    }

  }
  // Update is called once per frame
  // void Update()
  // {

  // }
}

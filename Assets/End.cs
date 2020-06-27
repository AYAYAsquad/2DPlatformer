using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class End : MonoBehaviour
{
  [SerializeField] private string sceneName;

public int newScore;
public int oldScore;
public int newTime;
public int oldTime;
  public CoinPicker cp;
  public Timer timer;

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.transform.tag == "Player")
    {
      SaveScore();
      SceneManager.LoadScene(sceneName);

    }
  }

  public void SaveScore()
  {
    if ((PlayerPrefs.HasKey("Score")) || PlayerPrefs.HasKey("Time")){
      oldScore = PlayerPrefs.GetInt("Score");
      newScore = cp.coin;

      if (newScore > oldScore){
        PlayerPrefs.SetInt("Score", newScore);
      }
      else{
        PlayerPrefs.SetInt("Score", oldScore);
      }
    }
    else {
      PlayerPrefs.SetInt("Score", cp.coin);
      PlayerPrefs.SetFloat("Time", timer.currentTime);
    }
  }
}

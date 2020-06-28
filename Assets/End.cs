using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.MLAgents;


public class End : MonoBehaviour
{
  [SerializeField] private string sceneName;

  public int newScore;
  public int oldScore;
  public float newTime;
  public float oldTime;
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
      oldTime = PlayerPrefs.GetFloat("Time");
      newScore = cp.coin;

      if (newScore > oldScore){
        PlayerPrefs.SetInt("Score", newScore);
      }
      else if (oldScore > newScore){
        PlayerPrefs.SetInt("Score", oldScore);
      }
      if (newTime < oldTime) {
        PlayerPrefs.SetFloat("Time", timer.currentTime);
      }
      else if (oldTime < newTime) {
        PlayerPrefs.SetFloat("Time", oldTime);
      }

    }
    else {
      PlayerPrefs.SetInt("Score", cp.coin);
      // PlayerPrefs.SetFloat("Time", timer.currentTime);
    }
  }
}

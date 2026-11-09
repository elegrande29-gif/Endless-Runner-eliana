using UnityEngine;


public class GameManager : MonoBehaviour
{
   public static GameManager Instance { get; private set; }
   public int speed = 0;
   public bool gameOver;
   public bool startGame {get; set;}
   


   void Awake()
   {
       if(Instance == null)
       {
           Instance = this;
       }
       else
       {
           Destroy(gameObject);
       }
       
   }
}

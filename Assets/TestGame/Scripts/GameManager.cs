using System.Collections;
using System.Collections.Generic;
using UnityEngine;


  public class GameManager : MonoBehaviour
  {
        internal bool levelFinished=false;
        public static GameManager gameManager;

        private void Awake()
        {
          if (gameManager != null)
          {
              Destroy(this.gameObject);
          }
           gameManager = this;
        }

}


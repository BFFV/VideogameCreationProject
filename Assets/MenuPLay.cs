using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPLay : MonoBehaviour
{
    // Start is called before the first frame update
    public void PlayGame () {
        
        SceneManager.LoadScene("DemoLevel");


    }
}

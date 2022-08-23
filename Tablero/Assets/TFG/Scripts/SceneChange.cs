using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneChange : MonoBehaviour
{

    //cargaremos con load y llamaremos la escena
    public void Update()
    {
    }

    public void cargarJuegoFacil()
    {
        SceneManager.LoadScene("MinijuegoLna");
    }
 
}

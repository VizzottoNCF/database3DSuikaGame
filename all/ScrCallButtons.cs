using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrCallButtons : MonoBehaviour
{
    public TMP_Text NameText;
    public TMP_Text IDText;
    public ClassDataBase DataBase;

    void Start()
    {
        DataBase = this.GetComponent<ClassDataBase>();
    }
    
    public void rf_OnInserirPressed()
    {
        DataBase.inserir(NameText.text);
    }
    
    public void rf_OnInserirScalarPressed()
    {
        DataBase.inserirScalar(NameText.text);
    }

    public void rf_OnAlterarPressed()
    {
        string trimmedText = new string(IDText.text.Where(char.IsDigit).ToArray()); // Remove non-numeric characters
        //Debug.Log("Filtered text: '" + trimmedText + "'");

        int id;
        if (int.TryParse(trimmedText, out id))
        {
            //Debug.Log("Parsed ID: " + id);
            DataBase.alterar(id, NameText.text);
        }
        else
        {
            Debug.LogError("Invalid ID after filtering: '" + trimmedText + "'");
        }
    }

    public void rf_OnRemoverPressed()
    {
        string trimmedText = new string(IDText.text.Where(char.IsDigit).ToArray()); // Remove non-numeric characters
        //Debug.Log("Filtered text: '" + trimmedText + "'");

        int id;
        if (int.TryParse(trimmedText, out id))
        {
            //Debug.Log("Parsed ID: " + id);
            DataBase.remover(id);
        }
        else
        {
            Debug.LogError("Invalid ID after filtering: '" + trimmedText + "'");
        }
    }
    public void rf_OnConsultarPressed()
    {
        DataBase.consultar();
    }
}

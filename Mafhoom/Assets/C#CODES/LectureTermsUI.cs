using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LectureTermsUI : MonoBehaviour
{
    public TMP_InputField termInputField;
    public InstructorSessionController sessionController;

    public void AddTerm()
    {
        string term = termInputField.text.Trim();

        if (string.IsNullOrEmpty(term))
            return;

        sessionController.lectureTerms.Add(term);
        Debug.Log("Added term: " + term);

        termInputField.text = "";
    }
}
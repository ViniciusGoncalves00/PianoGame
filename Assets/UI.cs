using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text  _hits;
    [SerializeField] private TMP_Text _streak;

    public void UpdateUI(int hits, int streak)
    {
        _hits.text = hits.ToString();
        _streak.text = streak.ToString();
    }
}
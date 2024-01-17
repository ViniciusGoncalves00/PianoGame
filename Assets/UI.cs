using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text hits;
    [SerializeField] private TMP_Text streak;

    public void UpdateUI(int hits, int streak)
    {
        this.hits.text = hits.ToString();
        this.streak.text = streak.ToString();
    }
}
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Piano : MonoBehaviour
{
    [SerializeField] private UI _ui;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private Button[] _pianoKeys;
    
    private readonly Color _highlightColor = new Color(0.75f, 0.50f, 0.25f, 1.0f);
    private readonly Color _correctHighlightColor = new Color(0.25f, 0.50f, 0.75f, 1);
    private readonly Color _wrongHighlightColor = new Color(0.75f, 0.25f, 0.25f, 1);
    
    private readonly List<int> _notesSequence = new List<int>();
    private Image[] _squareSpriteRenderer;

    private const float DelayToNewTurn = 1.0f;
    private const float DelayToNewHighlight = 0.25f;
    private const float DelayToBackToOriginalColor = 0.5f;
    
    private int _keysAmount;
    private int _actualNote;
    private int _notesAmount;
    
    private int _hits;
    private int _streak;

    private void Awake()
    {
        _keysAmount = _pianoKeys.Length;
        _squareSpriteRenderer = new Image[_keysAmount];

        for (int i = 0; i < _keysAmount; i++)
        {
            var squareValue = i;
            _pianoKeys[i].onClick.AddListener(() => ClickButton(squareValue));
            _squareSpriteRenderer[i] = _pianoKeys[i].GetComponent<Image>();
        }
    }

    private void Start()
    {
        ToggleButtonEnabled(false);
        NextTurn();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void ClickButton(int answer)
    {
        _audioManager.PlaySound(answer);
        CheckAnswer(answer);
    }

    private void CheckAnswer(int answer)
    {
        var correctAnswer = _notesSequence[_actualNote];

        if (answer != correctAnswer)
        {
            StartCoroutine(HighlightKeys(answer, _wrongHighlightColor));
            EndGame();
            return;
        }

        StartCoroutine(HighlightKeys(answer, _correctHighlightColor));
            
        _actualNote++;

        if (_actualNote > _hits)
        {
            _hits++;
            if (_hits > _streak)
            {
                _streak++;
            }
            _ui.UpdateUI(_hits, _streak);
            NextTurn();
        }
    }

    private void NextTurn()
    {
        _notesAmount++;
        _actualNote = 0;
        _notesSequence.Clear();
        
        NewSequence();

        StartCoroutine(ShowSequence());
    }

    private void ToggleButtonEnabled(bool boolean)
    {
        foreach (var pianoKey in _pianoKeys)
        {
            pianoKey.enabled = boolean;
        }
    }

    private void NewSequence()
    {
        for (int i = 0; i < _notesAmount; i++)
        {
            var note = Random.Range(0, _keysAmount);
            _notesSequence.Add(note);
        }
    }

    private IEnumerator ShowSequence()
    {
        ToggleButtonEnabled(false);

        yield return new WaitForSeconds(DelayToNewTurn);
            
        for (int i = 0; i < _notesAmount; i++)
        {
            //StartCoroutine(HighlightKeys(i, highlightColor));
            
            var index = _notesSequence[i];
            var originalColor = _squareSpriteRenderer[index].color;
            _squareSpriteRenderer[index].color = _highlightColor;
            
            _audioManager.PlaySound(index);
            
            yield return new WaitForSeconds(DelayToBackToOriginalColor);

            _squareSpriteRenderer[index].color = originalColor;
            yield return new WaitForSeconds(DelayToNewHighlight);
        }

        ToggleButtonEnabled(true);
    }

    private IEnumerator HighlightKeys(int index, Color color)
    {
        var originalColor = _squareSpriteRenderer[index].color;
        _squareSpriteRenderer[index].color = color;
            
        _audioManager.PlaySound(index);
            
        yield return new WaitForSeconds(DelayToBackToOriginalColor/2);

        _squareSpriteRenderer[index].color = originalColor;
        yield return new WaitForSeconds(DelayToNewHighlight/2);
    }

    private void EndGame()
    {
        _notesAmount = 0;
        _hits = 0;
        _ui.UpdateUI(_hits, _streak);
        
        NextTurn();
    }
}
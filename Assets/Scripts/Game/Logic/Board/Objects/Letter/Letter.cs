
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Utils;

public class Letter : BaseObject
{
    public static int movingLetterCount;

    public LetterContent content;
    LetterData _data;

    Board _baseBoard;

    TextMeshPro _textMeshPro;

    LetterSituation _targetSituation;
    StackObject _targetStackObject;

    Vector3     _beginPosition, _targetPosition;
    Vector3     _beginScale, _targetScale;

    public int ID { get => _data.id; private set { } }
    public char Character { get => _data.character[0]; private set { } }

    protected override void Awake()
    {
        base.Awake();

        SetLetterRequirements();
    }

    private void Update()
    {
        if (content.Situation == LetterSituation.DEALLOCATED) return;

        TrySelect();
        TryReachToTarget();
    }

    public void TurnOnObject(Board baseBoard, LetterData data, List<LetterContent> parents, List<LetterContent> children)
    {
        transform.gameObject.SetActive(true);
        _data = data;

        _baseBoard = baseBoard;

        _textMeshPro.text = data.character;

        content.TurnOn(parents, children, this, _data.character[0]);
        ChangeColor();

        transform.localScale = _beginScale;
        transform.position = new Vector3(_data.position.x, _data.position.y, _data.position.z);
        _beginPosition = transform.position;
    }

    public override void TurnOffObject()
    {

        content.TurnOff();
        transform.gameObject.SetActive(false);
    }

    public void Undo()
    {
        if (_targetStackObject != null)
        {
            _targetStackObject = null;
            _targetPosition = _beginPosition;
            _targetScale = _beginScale;

            _targetSituation = LetterSituation.SELECTABLE;

            movingLetterCount++;
            content.UpdateSituation(LetterSituation.HIGHLIGHTED);
        }
    }

    private void SetLetterRequirements()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        _targetStackObject = null;
        _beginScale = transform.localScale;

        content = new LetterContent();
    }

    private void TrySelect()
    {
        if (SelectionSystem.Instance.SelectedTile == this)
        {
            SelectionSystem.Instance.ReleaseObject();

            if (content.Situation != LetterSituation.SELECTABLE ||
                (_targetStackObject = _baseBoard.GetPlaceableStackObject()) == null)
                return;

            _targetPosition = _targetStackObject.transform.position;
            _targetScale = ObjectInfo.StackObjectScale * Constants.FitOnStackRatio;

            _targetSituation = LetterSituation.PLACED;
            _baseBoard.AddToSelectedLetters(this);

            movingLetterCount++;
            content.UpdateSituation(LetterSituation.HIGHLIGHTED);
        }
    }



    private bool MoveLetter()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                _targetPosition, Constants.LetterMoveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetPosition) <= 0.1f)
                 transform.position = _targetPosition;
            return false;
        }
        return true;
    }

    private bool ScaleLetter()
    {
        if (Vector3.Distance(transform.localScale, _targetScale) > 0.1f)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale,
                _targetScale, Constants.LetterScaleSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localScale, _targetScale) <= 0.1f)
                 transform.localScale = _targetScale;
            return false;
        }
        return true;
    }

    private void TryReachToTarget()
    {
        if (content.Situation == LetterSituation.HIGHLIGHTED) ReachToTarget();
    }

    private void ReachToTarget()
    {
        bool isScaled = ScaleLetter();
        bool isMoved = MoveLetter();

        if (isScaled && isMoved) RefreshSituations();
    }

    private void RefreshSituations()
    {
        content.UpdateSituationByControlParent(_targetSituation);
        content.UpdateChildrenSituation();

        movingLetterCount--;
        ChangeColor();
        foreach (LetterContent child in content.Children) child.BaseLetter.ChangeColor();
    }

    public void ChangeColor()
    {
        if (content.Situation == LetterSituation.SELECTABLE) color = Colors.LetterColor;
        else if (content.Situation == LetterSituation.UNSELECTABLE) color = Colors.DarkenLetterColor;
    }

}

using System.Collections.Generic;

public class LetterContent
{
    public List<LetterContent> Parents { get; private set; }
    public List<LetterContent> Children { get; private set; }
    public Letter BaseLetter { get; private set; }
    public LetterSituation Situation { get; private set; }
    public char Character { get; private set; }

    public LetterContent() => TurnOff();
    public void TurnOn(List<LetterContent> parents, List<LetterContent> children, Letter baseLetter, char character)
    {
        Parents = parents;
        Children = children;
        BaseLetter = baseLetter;
        Character = character;

        if (Parents.Count != 0) UpdateSituation(LetterSituation.UNSELECTABLE);
        else UpdateSituation(LetterSituation.SELECTABLE);
    }

    public void TurnOff()
    {
        BaseLetter = null;
        Character = '\0';

        ClearParents();
        ClearChildren();
        UpdateSituation(LetterSituation.DEALLOCATED);
    }

    public void UpdateSituation(LetterSituation situation) => this.Situation = situation;

    public void UpdateSituationByControlParent(LetterSituation situation)
    {
        Situation = situation;
        if (situation != LetterSituation.SELECTABLE) return;

        foreach (LetterContent parent in Parents)
        {
            if (parent.Situation == LetterSituation.SELECTABLE || parent.Situation == LetterSituation.UNSELECTABLE)
            {
                Situation = LetterSituation.UNSELECTABLE;
                return;
            }
        }
    }

    public void UpdateChildrenSituation()
    {
        foreach (LetterContent child in Children) child.UpdateSituationByParent();
    }
    public void UpdateSituationByParent()
    {
        foreach (LetterContent parent in Parents)
        {
            if (parent.Situation != LetterSituation.SELECTABLE && parent.Situation != LetterSituation.UNSELECTABLE)
                    continue;
            if (Situation == LetterSituation.SELECTABLE) UpdateSituation(LetterSituation.UNSELECTABLE);
            return;
        }

        if (Situation == LetterSituation.UNSELECTABLE) UpdateSituation(LetterSituation.SELECTABLE);
    }

    private void ClearChildren()
    {
        if (Children == null) return;
        foreach (LetterContent child in Children) child.Parents.Remove(this);
        Children.Clear();
    }

    private void ClearParents()
    {
        if (Parents == null) return;
        foreach (LetterContent parent in Parents) parent.Children.Remove(this);
        Parents.Clear();
    }
}

using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    public class ObjectInfo
    {
        static private Dictionary<char, int> _points;
        static public Dictionary<char, int> Points
        {
            get
            {
                if (_points != null) return _points;
                _points = new Dictionary<char, int>
                {
                    { 'A', 1 }, { 'E', 1 }, { 'O', 1 }, { 'N', 1 }, { 'R', 1 }, { 'T', 1 }, { 'L', 1 }, { 'S', 1 }, { 'U', 1 },
                    { 'I', 2 }, { 'D', 2 }, { 'G', 2 },
                    { 'B', 3 }, { 'C', 3 }, { 'M', 3 }, { 'P', 3 },
                    { 'F', 4 }, { 'H', 4 }, { 'V', 4 }, { 'W', 4 }, { 'Y', 4 },
                    { 'K', 5 },
                    { 'J', 8 }, { 'X', 8 },
                    { 'Q', 10 }, { 'Z', 10 }
                };
                return _points;
            }
        }

        static public int GetScore(List<Letter> selectedLetters)
        {
            int score = 0;

            for (int i = 0; i < selectedLetters.Count; i++)
                score += 10 * selectedLetters.Count * Points[selectedLetters[i].Character];

            score -= 10 * (7 - selectedLetters.Count);
            return score;
        }

        static public ObjectType AnalyzeType<T>() where T : BaseObject
        {
            if(typeof(T) == typeof(Letter)) return ObjectType.LETTER;
            else return ObjectType.STACK_OBJECT;
        }

        static public Vector3 StackCenterPosition
        {
            get
            {
                Vector3 center;

                center = Inputs.CenterToWorld;

                center.y += Constants.StackCenterYOffset;
                center.x += Constants.StackCenterXOffset;
                center.z += Constants.StackCenterZOffset;

                    return center;
            }
        }

        static private Vector3 _stackSpriteScale;
        static public Vector3 StackObjectScale
        {
            get
            {
                if (_stackSpriteScale != Vector3.zero) return _stackSpriteScale;
                StackObject stackObject = PoolSystem.Instance.Allocate<StackObject>();
                _stackSpriteScale = stackObject.spriteRenderer.sprite.bounds.size;
                _stackSpriteScale.x *= stackObject.transform.localScale.x;
                _stackSpriteScale.y *= stackObject.transform.localScale.y;
                PoolSystem.Instance.Deallocate<StackObject>(stackObject);

                return _stackSpriteScale;
            }
        }

    }
}

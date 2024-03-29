using UnityEngine;

namespace Utils
{
    public class Colors
    {
        static private Color _Lettercolor;
        static private bool _lettercolorSet = false;
        static public Color LetterColor {
            get {
                Letter Letter;

                if (_lettercolorSet == true) return _Lettercolor;
                _lettercolorSet = true;
                Letter = PoolSystem.Instance.Allocate<Letter>();
                _Lettercolor = Letter.color;
                PoolSystem.Instance.Deallocate<Letter>(Letter);
                return _Lettercolor;
            }
        }

        static private Color _darkenLetterColor;
        static private bool _darkenLetterColorSet = false;
        static public Color DarkenLetterColor {
            get {
                if (_darkenLetterColorSet == true) return _darkenLetterColor;

                _darkenLetterColorSet = true;
                _darkenLetterColor.r = LetterColor.r * (1 - Constants.DarknessFactor);
                _darkenLetterColor.g = LetterColor.g * (1 - Constants.DarknessFactor);
                _darkenLetterColor.b = LetterColor.b * (1 - Constants.DarknessFactor);
                _darkenLetterColor.a = 1;
                return _darkenLetterColor;
            }
        }
    }
}

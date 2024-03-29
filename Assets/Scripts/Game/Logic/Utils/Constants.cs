    using UnityEngine;

namespace Utils
{
    public class Constants
    {
        public const string ResourcesPath = "Assets/Resources/";
        public const string LevelPath = "levels";
        public const string LevelPrefix = "/level_";
        public const string WordsPath = "Assets/Resources/words.txt";

        public const int StackSize = 7;

        public const float StackCenterYOffset = 25.0f;
        public const float StackCenterXOffset = 0.5f;
        public const float StackCenterZOffset = 100f;

        public const float LetterMoveSpeed = 200f;
        public const float LetterScaleSpeed = 50f;


        public const float DarknessFactor = 0.5f;
        public const float FitOnStackRatio = 0.5f;

        public const int TotalTargetLoadCount = 350000;

        public static readonly Vector2 ReferenceResolution = new Vector2(1080, 1920);
    }
}

using UnityEngine;

namespace Utils
{
    class Inputs
    {
        static public Vector3 MouseToWorld { get => Camera.main.ScreenToWorldPoint(Input.mousePosition); }

        static public Vector3 CenterToWorld {
            get
            {
                Vector3 center;

                center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
                return Camera.main.ScreenToWorldPoint(center);
            }
        }

        static public BaseObject ScreenToObject {
            get {
                Vector3    clickPosition;

                clickPosition = MouseToWorld;
                clickPosition.z = 0;
                return PositionToObject(clickPosition);
            }
        }

        static public BaseObject PositionToObject(Vector3 position) {
            BaseObject baseObject;
            RaycastHit2D hit;

            hit = Physics2D.Raycast(position, Vector2.zero);
            if (hit.collider != null) {
                baseObject = hit.collider.gameObject.GetComponent<BaseObject>();
                if (baseObject != null) return baseObject;
            }
            return null;
        }
    }
}

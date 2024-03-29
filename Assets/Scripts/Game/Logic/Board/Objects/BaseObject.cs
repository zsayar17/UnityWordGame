
using System.Collections.Generic;
using UnityEngine;


public enum ObjectType
{
    STACK_OBJECT,
    LETTER,
    ObjectTypeCount
}

public abstract class BaseObject : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Color color { get => spriteRenderer.color; set => spriteRenderer.color = value; }


    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public abstract void TurnOffObject();
}

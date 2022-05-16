using UnityEngine;

[System.Serializable]
public abstract class UIValue<T> : MonoBehaviour
{
    public abstract T Set(T value);
}
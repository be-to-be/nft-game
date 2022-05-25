using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    public delegate void ErrorData(string errorData);
    public static ErrorData OnErrorData;

    public void Client_SetErrorData(string errorData)
    {
        OnErrorData(errorData);
    }
}

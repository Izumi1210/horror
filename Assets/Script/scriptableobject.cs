using UnityEngine;

[CreateAssetMenu]
public class TestScriptableObject : ScriptableObject
{

    [SerializeField]
    string testString = "これはScriptableObjectのテストです。";

    public string TestString { get { return testString; } }

}
using UnityEngine;

[CreateAssetMenu]
public class TestScriptableObject : ScriptableObject
{

    [SerializeField]
    string testString = "�����ScriptableObject�̃e�X�g�ł��B";

    public string TestString { get { return testString; } }

}
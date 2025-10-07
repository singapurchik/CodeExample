#if UNITY_EDITOR
using UnityEditor;
using FAS;

[CustomEditor(typeof(FAS.Players.PlayerCostumeCreator))]
public class PlayerCostumeCreatorEditor : CharacterCostumeCreatorEditor
{
}
#endif

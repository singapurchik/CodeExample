using UnityEngine;

[CreateAssetMenu(fileName = "WindowTextureOption", menuName = "FAS/Texture Option", order = 0)]
public class TextureSelectorOption : ScriptableObject
{
	[field: SerializeField] public string DisplayName { get; private set; }
	[field: SerializeField] public Texture Texture { get; private set; }
}
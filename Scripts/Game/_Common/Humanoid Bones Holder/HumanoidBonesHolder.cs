using UnityEngine;
using VInspector;

namespace FAS
{
    public class HumanoidBonesHolder : MonoBehaviour
    {
        [field: SerializeField] public Transform Hips { get; private set; }
        [field: SerializeField] public Transform LeftUpLeg { get; private set; }
        [field: SerializeField] public Transform LeftLeg { get; private set; }
        [field: SerializeField] public Transform LeftFoot { get; private set; }
        [field: SerializeField] public Transform RightUpLeg { get; private set; }
        [field: SerializeField] public Transform RightLeg { get; private set; }
        [field: SerializeField] public Transform RightFoot { get; private set; }
        [field: SerializeField] public Transform Spine { get; private set; }
        [field: SerializeField] public Transform Spine1 { get; private set; }
        [field: SerializeField] public Transform Spine2 { get; private set; }
        [field: SerializeField] public Transform Neck { get; private set; }
        [field: SerializeField] public Transform Head { get; private set; }
        [field: SerializeField] public Transform LeftShoulder { get; private set; }
        [field: SerializeField] public Transform LeftArm { get; private set; }
        [field: SerializeField] public Transform LeftForeArm { get; private set; }
        [field: SerializeField] public Transform LeftHand { get; private set; }
        [field: SerializeField] public Transform RightShoulder { get; private set; }
        [field: SerializeField] public Transform RightArm { get; private set; }
        [field: SerializeField] public Transform RightForeArm { get; private set; }
        [field: SerializeField] public Transform RightHand { get; private set; }
        
        public Transform GetBoneByType(HumanoidBoneType type)
        {
	        return type switch
	        {
		        HumanoidBoneType.Hips => Hips,
		        HumanoidBoneType.LeftUpLeg => LeftUpLeg,
		        HumanoidBoneType.LeftLeg => LeftLeg,
		        HumanoidBoneType.LeftFoot => LeftFoot,
		        HumanoidBoneType.RightUpLeg => RightUpLeg,
		        HumanoidBoneType.RightLeg => RightLeg,
		        HumanoidBoneType.RightFoot => RightFoot,
		        HumanoidBoneType.Spine => Spine,
		        HumanoidBoneType.Spine1 => Spine1,
		        HumanoidBoneType.Spine2 => Spine2,
		        HumanoidBoneType.Neck => Neck,
		        HumanoidBoneType.Head => Head,
		        HumanoidBoneType.LeftShoulder => LeftShoulder,
		        HumanoidBoneType.LeftArm => LeftArm,
		        HumanoidBoneType.LeftForeArm => LeftForeArm,
		        HumanoidBoneType.LeftHand => LeftHand,
		        HumanoidBoneType.RightShoulder => RightShoulder,
		        HumanoidBoneType.RightArm => RightArm,
		        HumanoidBoneType.RightForeArm => RightForeArm,
		        HumanoidBoneType.RightHand => RightHand,
		        _ => null
	        };
        }

#if UNITY_EDITOR
        [Button]
        private void TryFindBones()
        {
	        Hips = FindBone("Hips");
	        LeftUpLeg = FindBone("LeftUpLeg");
	        LeftLeg = FindBone("LeftLeg");
	        LeftFoot = FindBone("LeftFoot");
	        RightUpLeg = FindBone("RightUpLeg");
	        RightLeg = FindBone("RightLeg");
	        RightFoot = FindBone("RightFoot");
	        Spine = FindBone("Spine");
	        Spine1 = FindBone("Spine1");
	        Spine2 = FindBone("Spine2");
	        Neck = FindBone("Neck");
	        Head = FindBone("Head");
	        LeftShoulder = FindBone("LeftShoulder");
	        LeftArm = FindBone("LeftArm");
	        LeftForeArm = FindBone("LeftForeArm");
	        LeftHand = FindBone("LeftHand");
	        RightShoulder = FindBone("RightShoulder");
	        RightArm = FindBone("RightArm");
	        RightForeArm = FindBone("RightForeArm");
	        RightHand = FindBone("RightHand");
        }

        private Transform FindBone(string boneName)
        {
	        var withPrefix = FindDeepChild(transform, "mixamorig:" + boneName);
	        if (withPrefix != null)
		        return withPrefix;

	        return FindDeepChild(transform, boneName);
        }

        private Transform FindDeepChild(Transform parent, string name)
        {
	        foreach (Transform child in parent)
	        {
		        if (child.name == name)
			        return child;

		        var result = FindDeepChild(child, name);
		        
		        if (result != null)
			        return result;
	        }
	        return null;
        }
#endif

    }
}
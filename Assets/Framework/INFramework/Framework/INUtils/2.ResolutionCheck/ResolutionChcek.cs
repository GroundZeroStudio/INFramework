using UnityEngine;

namespace INFramework
{
	public class ResolutionChcek : MonoBehaviour
	{
		public static bool IsLandscape
        {
			get => Screen.width > Screen.height;
        }

		public static bool IsPad
        {
            get
            {
                return IsRatio(4, 3);
            }
        }

        public static bool IsPhone
        {
            get
            {
                return IsRatio(16, 9);
            }
        }

        public static bool IsRatio(float width, float height)
        {
            var ratio = IsLandscape ? Screen.width * 1.0f / Screen.height : Screen.height * 1.0f / Screen.width;
            var destinationRatio = width / height;
            return ratio < destinationRatio + 0.05f && ratio > destinationRatio - 0.05f;
        }
    }
}


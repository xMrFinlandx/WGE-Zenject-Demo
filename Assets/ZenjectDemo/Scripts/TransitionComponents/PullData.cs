using UnityEngine;

namespace ZenjectDemo
{
    public struct PullData
    {
        public readonly Vector3 Force;
        public readonly bool Exists;

        public PullData(Vector3 force)
        {
            Force = force;
            Exists = true;
        }
    }
}
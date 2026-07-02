using UnityEngine;
using WorldGraphEditor;

namespace ZenjectDemo
{
    public class ZenjectTeleport : MonoBehaviour, ITransitionComponent
    {
        
#if UNITY_EDITOR
        public void Refresh(RefreshContext context)
        {
            
        }
#endif

        public Vector3 GetSpawnPosition()
        {
            throw new System.NotImplementedException();
        }

        public string GetGuid()
        {
            throw new System.NotImplementedException();
        }
    }
}
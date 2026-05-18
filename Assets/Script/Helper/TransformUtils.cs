namespace Script.Helper
{
    using System.Collections.Generic;
    using KarpysDev.KarpysUtils;
    using UnityEngine;

    public interface IPosition
    {
        public Vector3 Position { get; }
    }
    
    public static class TransformUtils
    {
        public static Transform GetClosest(this List<Transform> transforms, Vector3 position)
        {
            if (transforms.Count == 0)
                return null;

            Transform closest = transforms[0];
            float closestDist = Vector2.Distance(position, closest.position);
            
            foreach (Transform transform in transforms)
            {
                float dist = Vector2.Distance(position, closest.position); 
                
                if (dist <= closestDist)
                {
                    closest = transform;
                    closestDist = dist;
                }
            }

            return closest;
        }
        
        public static int GetClosestViaId(this List<IPosition> transforms, Vector3 position)
        {
            if (transforms.Count == 0)
                return -1;

            IPosition closest = transforms[0];
            int closestId = 0;
            float closestDist = Vector2.Distance(position, closest.Position);
            
            int id = 0;
            
            foreach (IPosition transform in transforms)
            {
                float dist = Vector2.Distance(position, transform.Position); 
                
                if (dist <= closestDist)
                {
                    closestDist = dist;
                    closestId = id;
                }

                id++;
            }

            return closestId;
        }
    }
}
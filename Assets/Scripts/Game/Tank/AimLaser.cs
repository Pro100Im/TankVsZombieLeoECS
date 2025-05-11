using UnityEngine;

namespace Game.Tank
{
    public sealed class AimLaser : MonoBehaviour
    {
        [SerializeField] private float laserDistance = 100f;
        [Space]
        [SerializeField] private LayerMask laserMask;
        [SerializeField] private LineRenderer lineRenderer;

        private void Update() => CastLaser();

        private void CastLaser()
        {
            var hit = Physics2D.Raycast(transform.position, transform.up, laserDistance, laserMask);

            if(hit)
                DrawLaser(transform.position, hit.point);
            else
                DrawLaser(transform.position, transform.position + transform.up * laserDistance);
        }

        private void DrawLaser(Vector2 startPoint, Vector2 endPoint)
        {
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
    }
}
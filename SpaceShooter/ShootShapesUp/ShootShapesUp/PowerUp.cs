using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace ShootShapesUp
{
    class PowerUp : Entity
    {
        private float acceleration = 0.5f;

        public bool IsActive { get { return IsActive; } }

        public PowerUp(Vector2 position, Vector2 velocity)
        {
            image = GameRoot.puShield;
            Position = position;
            Velocity = velocity;
        }

        public override void Update()
        {
            if (!PlayerShip.Instance.IsDead)
            {
                Velocity += (PlayerShip.Instance.Position - Position) * (acceleration / (PlayerShip.Instance.Position - Position).Length());
                Position += Velocity;
            }
               
            if (Velocity != Vector2.Zero)
                Orientation = Velocity.ToAngle();

            if (this.Position.Y > GameRoot.ScreenSize.Y)
                IsExpired = true;
        }


    }
}

    using Microsoft.Xna.Framework;

    namespace SofEngeneering_project.Interfaces
    {
        public interface IMovable
        {
            Vector2 Position { get; set; }
            Vector2 Velocity { get; set; }
        }
    }
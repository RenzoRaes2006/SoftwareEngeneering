    using Microsoft.Xna.Framework;

    namespace SofEngeneering_project.Interfaces
    {
        // Alleen data die nodig is voor beweging/botsingen
        public interface IMovable
        {
            Vector2 Position { get; set; }
            Vector2 Velocity { get; set; }
        }
    }
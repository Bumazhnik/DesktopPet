using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopPet.Scheduler;
using DesktopPet.Structs;

namespace DesktopPet.Characters
{
    internal class Character : ICharacter
    {
        public CharacterState State => CharacterState.Idle;

        private Vector2 position;
        public Vector2 Position
        {
            get => position; set => position = value;
        }

        public Size2 Size { get; set; }
        public Bounds2 Bounds { get; set; }
        private Vector2 velocity;
        private ActionTaskScheduler scheduler = new ActionTaskScheduler();
        private BehaviourTaskScheduler behaviourScheduler = new BehaviourTaskScheduler();

        Vector2 previousPos;
        double throwSpeed = 30;
        public void Update(GameFrame frame)
        {
            scheduler.Update(frame.delta);
            behaviourScheduler.Update(frame.delta);
            if (dragging)
            {
                previousPos = position;
                Vector2 newPos;
                newPos.x = offsetX + frame.cursor.x;
                newPos.y = offsetY + frame.cursor.y;
                Position = newPos;
                velocity = new((newPos.x - previousPos.x) * throwSpeed, (newPos.y - previousPos.y) * throwSpeed);
            }
            else
            {
                ApplyGravity(frame.delta);
                ApplyVelocity(frame.delta);
                ReduceXVelocity(frame.delta);
                ClampToBounds();
            }
            Debug.WriteLine(velocity);
            Debug.WriteLine(currentBehaviour);
        }
        double offsetX;
        double offsetY;
        bool dragging;

        public Character(Size2 size, Bounds2 bounds)
        {
            Size = size;
            Bounds = bounds;
            scheduler.AddTask(new(AddRandomTask, 1.5));
        }

        private void ReduceXVelocity(double delta)
        {
            double reduceSpeed = 5;
            velocity = Vector2.Lerp(velocity, new(0, velocity.y), delta * reduceSpeed);
            if (velocity.x > -2 && velocity.x < 2)
                velocity.x = 0;
        }
        public void OnMouseDown(Vector2 cursor)
        {
            dragging = true;
            currentBehaviour = CharacterBehaviour.None;
            offsetX = Position.x - cursor.x;
            offsetY = Position.y - cursor.y;
            //ResetGravity();
            ResetVelocity();
            //scheduler.AddTask(new ActionTask(() => { velocity.y = -1500; }, 0.75, 2, true));
        }
        public void OnMouseUp()
        {
            dragging = false;
        }
        private void ResetGravity()
        {

        }
        private void ResetVelocity()
        {
            velocity = new();
        }
        private void ApplyVelocity(double delta)
        {
            position.x += velocity.x * delta;
            position.y += velocity.y * delta;
        }
        double gravity = 980 * 8;
        private void ApplyGravity(double delta)
        {
            if (velocity.y < gravity)
            {
                velocity = Vector2.Lerp(velocity, new(velocity.x, gravity), delta);
            }
        }
        private void ClampToBounds()
        {
            if (position.x < Bounds.position.x)
            {
                position.x = Bounds.position.x;
                velocity.x = Math.Abs(velocity.x);
            }
            if (position.y < Bounds.position.y)
            {
                position.y = Bounds.position.y;
            }
            if (position.x + Size.width > Bounds.position.x + Bounds.size.width)
            {
                position.x = Bounds.position.x + Bounds.size.width - Size.width;
                velocity.x = -Math.Abs(velocity.x);
            }
            if (position.y + Size.height > Bounds.position.y + Bounds.size.height)
            {
                position.y = Bounds.position.y + Bounds.size.height - Size.height;
            }
        }
        CharacterBehaviour _maxBehaviour = Enum.GetValues<CharacterBehaviour>().Last();
        double walkSpeed = 400;
        double jumpForce = -1500;
        CharacterBehaviour currentBehaviour;
        public void AddRandomTask()
        {
            if (dragging) return;
            currentBehaviour = (CharacterBehaviour)Random.Shared.Next((int)_maxBehaviour + 1);
            switch (currentBehaviour)
            {
                case CharacterBehaviour.None:
                    break;
                case CharacterBehaviour.WalkLeft:
                    behaviourScheduler.AddTask(new(() => velocity.x = walkSpeed, 0.5));
                    break;
                case CharacterBehaviour.WalkRight:
                    behaviourScheduler.AddTask(new(() => velocity.x = -walkSpeed, 0.5));
                    break;
                case CharacterBehaviour.Jump:
                    Task.Run(() =>
                    {
                        scheduler.AddTask(new(Jump, 0.5, 2, true));
                    });
                    break;
            }
        }
        public void Jump()
        {
            velocity.y = jumpForce;
        }
    }
}

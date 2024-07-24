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
        public CharacterState State { get; set; }

        private Vector2 Position
        {
            get => CharacterBounds.position; 
            set
            {
                var b = CharacterBounds;
                b.position = value;
                CharacterBounds = b;
            }
        }

        public Bounds2 CharacterBounds { get; set; }
        public Bounds2 FieldBounds { get; set; }

        //private Vector2 position;
        private Vector2 velocity;
        private Vector2 previousPos;
        private ActionTaskScheduler actionScheduler = new ActionTaskScheduler();
        private BehaviourTaskScheduler behaviourScheduler = new BehaviourTaskScheduler();
        private readonly CharacterBehaviour _maxBehaviour = Enum.GetValues<CharacterBehaviour>().Last();
        private CharacterBehaviour currentBehaviour;
        private readonly double walkSpeed = 200;
        private readonly double jumpForce = -1500;
        private readonly double throwSpeed = 30;
        private readonly double gravity = 980 * 8;
        private double offsetX;
        private double offsetY;
        private bool dragging;

        public Character(Bounds2 characterBounds, Bounds2 fieldBounds)
        {
            CharacterBounds = characterBounds;
            FieldBounds = fieldBounds;
            ScheduleRandomTasks();
        }

        public void Update(GameFrame frame)
        {
            actionScheduler.Update(frame.delta);
            behaviourScheduler.Update(frame.delta);
            if (dragging)
            {
                previousPos = Position;
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
            State = CharacterState.Idle;
            offsetX = Position.x - cursor.x;
            offsetY = Position.y - cursor.y;
            ResetVelocity();
            ClearAllTasks();
        }
        private void ClearAllTasks()
        {
            actionScheduler.ClearTasks();
            behaviourScheduler.ClearTasks();
        }
        public void OnMouseUp()
        {
            dragging = false;
            ScheduleRandomTasks();
        }
        private void ResetVelocity()
        {
            velocity = new();
        }
        private void ApplyVelocity(double delta)
        {
            var v = Position;
            v.x += velocity.x * delta;
            v.y += velocity.y * delta;
            Position = v;
        }

        private void ApplyGravity(double delta)
        {
            if (velocity.y < gravity)
            {
                velocity = Vector2.Lerp(velocity, new(velocity.x, gravity), delta);
            }
        }
        private void ClampToBounds()
        {
            if (Bounds2.ClampsToLeft(CharacterBounds,FieldBounds))//Left
            {
                CharacterBounds = Bounds2.StickToLeft(CharacterBounds,FieldBounds);

                velocity.x = Math.Abs(velocity.x);
            }
            if (Bounds2.ClampsToTop(CharacterBounds, FieldBounds))//Top
            {
                CharacterBounds = Bounds2.StickToTop(CharacterBounds, FieldBounds);
                velocity.y = 0;

            }
            if (Bounds2.ClampsToRight(CharacterBounds, FieldBounds))//Right
            {
                CharacterBounds = Bounds2.StickToRight(CharacterBounds, FieldBounds);

                velocity.x = -Math.Abs(velocity.x);
            }
            if (Bounds2.ClampsToBottom(CharacterBounds, FieldBounds))//Bottom
            {
                CharacterBounds = Bounds2.StickToBottom(CharacterBounds, FieldBounds);
                velocity.y = 0;
            }
        }

        private void ScheduleRandomTasks()
        {
            actionScheduler.AddTask(new(AddRandomTask, 1.5));
        }
        private void AddRandomTask()
        {
            if (dragging) return;
            currentBehaviour = (CharacterBehaviour)Random.Shared.Next((int)_maxBehaviour + 1);
            switch (currentBehaviour)
            {
                case CharacterBehaviour.None:
                    break;
                case CharacterBehaviour.WalkLeft:
                    behaviourScheduler.AddTask(new(WalkLeft, 1));
                    break;
                case CharacterBehaviour.WalkRight:
                    behaviourScheduler.AddTask(new(WalkRight, 1));
                    break;
                case CharacterBehaviour.Jump:
                    Jump();
                    break;
            }
        }
        private void WalkLeft()
        {
            velocity.x = -walkSpeed;
            State = CharacterState.WalkingLeft;
        }
        private void WalkRight()
        {
            velocity.x = walkSpeed;
            State = CharacterState.WalkingRight;
        }
        private void Jump()
        {
            velocity.y = jumpForce;
        }

        public void MakeHappy()
        {
            ClearAllTasks();
            ScheduleRandomTasks();
            actionScheduler.AddTask(new(Jump, 0.4, 2, true));
            behaviourScheduler.AddTask(new(() =>
            {
                State = CharacterState.Happy;
            }, 1));
            actionScheduler.AddTask(new(() =>
            {
                State = CharacterState.Idle;
            }, 1.1,1));
        }
    }
}

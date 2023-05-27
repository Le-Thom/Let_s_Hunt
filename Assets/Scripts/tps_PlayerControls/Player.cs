using System;
using UnityEngine;

namespace AkarisuMD
{
    namespace Player
    {
        /// <summary>
        /// State machine for any mob.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        public class StateMachine<T>
        {
            //==============================================================================================================
            // PUBLIC
            //==============================================================================================================
            #region Public

            /// <summary>
            /// All state that is in your machine.
            /// </summary>
            public States<T>[] states;

            /// <summary>
            /// Current state you machine is.
            /// </summary>
            public StateId currentState;
            #endregion

            //==============================================================================================================
            // PRIVATE
            //==============================================================================================================
            #region Private


            /// <summary>
            /// Can be set public but always Init so.... State that your machine will start.
            /// </summary>
            private StateId startingStates => StateId.INIT;
            /// <summary>
            /// what it is currently controlling.
            /// </summary>
            private T agent;
            #endregion

            //==============================================================================================================
            // PUBLIC FONCTION
            //==============================================================================================================
            #region Public fonction

            // All the fonction used for your state need to be in public.

            /// <summary>
            /// Initialise your state machine for your script.
            /// Remember to call a starting state.
            /// </summary>
            /// <param name="agent"></param>
            public StateMachine(T agent)
            {
                this.agent = agent;
                int numStates = System.Enum.GetNames(typeof(StateId)).Length;
                states = new States<T>[numStates];
            }

            /// <summary>
            /// Set a new possible state for your machine.
            /// </summary>
            /// <param name="state"></param>
            public void RegisterState(States<T> state)
            {
                int index = (int)state.GetId();
                states[index] = state;
            }

            /// <summary>
            /// Return a state if it exist in the states list of your IA.
            /// </summary>
            /// <param name="stateId"></param>
            /// <returns></returns>
            public States<T> GetState(StateId stateId)
            {
                int index = (int)stateId;
                return states[index];
            }

            /// <summary>
            /// fonction Update needed to be called manually in a Update of your Monobehaviour
            /// </summary>
            public void Update()
            {
                GetState(currentState)?.Update(agent);
            }

            /// <summary>
            /// Change your state of the machine.
            /// </summary>
            /// <param name="newState"></param>
            public void ChangeState(StateId newState)
            {
                GetState(currentState)?.Exit(agent);
                currentState = newState;
                GetState(currentState)?.Init(agent);
            }

            /// <summary>
            /// Start the machine with the state you need.
            /// </summary>
            /// <param name="newState"></param>
            public void StartingState(StateId newState)
            {
                currentState = newState;
                GetState(currentState)?.Init(agent);
            }
            #endregion
        }

        // state machine of how all controller related to the player will work.
        #region States

        /// <summary>
        /// All the state the controller can be.
        /// </summary>
        public enum StateId
        {
            // At the start of the stateMachine
            INIT,

            // When the game is running
            ISNTLOADED,

            //Performing State
            IDLE,
            DODGE,
            ATK1,
            ATK2,
            EQUIPEMENT,
            HEALING,
            GETHIT,

            // When the game is in pause for the player
            PAUSED,

            // Death of the player
            DEATH,
        }

        /// <summary>
        /// interface of all state of the global controller
        /// </summary>
        public abstract class States<T>
        {
            /// <summary>
            /// Event of state at Init.
            /// </summary>
            public delegate void EventsAtInitOfState();
            public EventsAtInitOfState delegateEventsAtInitOfState;

            /// <summary>
            /// Event of state at update.
            /// </summary>
            public delegate void EventsAtUpdateOfState();
            public EventsAtUpdateOfState delegateEventsAtUpdateOfState;

            /// <summary>
            /// Event of state at Exit.
            /// </summary>
            public delegate void EventsAtExitOfState();
            public EventsAtExitOfState delegateEventsAtExitOfState;

            /// <summary>
            /// Get the Id of the state.
            /// </summary>
            /// <returns></returns>
            public virtual StateId GetId() { Debug.LogError("Didn't set state ID"); return StateId.DEATH; }
            /// <summary>
            /// Set all the need when you enter the state.
            /// </summary>
            /// <param name="agent"></param>
            public virtual void Init(T agent) { delegateEventsAtInitOfState?.Invoke(); }
            /// <summary>
            /// Fonction Update of the state. (like a fonction Update of Monobehaviour)
            /// </summary>
            /// <param name="agent"></param>
            public virtual void Update(T agent) { delegateEventsAtUpdateOfState?.Invoke(); }
            /// <summary>
            /// Set all the need when you exit the state.
            /// </summary>
            /// <param name="agent"></param>
            public virtual void Exit(T agent) { delegateEventsAtExitOfState?.Invoke(); }
        }

        // all states.
        /// <summary>
        /// Initialise mob.
        /// </summary>
        public class StateInit<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.INIT;
            }
        }

        /// <summary>
        /// Player isn't in range.
        /// </summary>
        public class StateIsntLoaded<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.ISNTLOADED;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class StateIdle<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.IDLE;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class StateDodge<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.DODGE;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class StateAtk1<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.ATK1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class StateAtk2<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.ATK2;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class StateEquipement<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.EQUIPEMENT;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class StateHealing<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.HEALING;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class StateGetHit<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.GETHIT;
            }
        }

        /// <summary>
        /// Player is Paused.
        /// </summary>
        public class StatePaused<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.PAUSED;
            }
        }

        /// <summary>
        /// No hp left.
        /// </summary>
        public class StateDead<T> : States<T>
        {
            public override StateId GetId()
            {
                return StateId.DEATH;
            }
        }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EzMidi
{
    public class Connection : MonoBehaviour
    {
        private IntPtr ezmidiContext = IntPtr.Zero;
        private bool isInit;

        public delegate void NoteEvent(int channel, int note, int velocity);
        public delegate void LogEvent(string message);

        public NoteEvent NoteOn;
        public NoteEvent NoteOff;
        public LogEvent Log;

        private Native.Ezmidi_Event midiEvent = new Native.Ezmidi_Event();

        public bool isConnected { get { return GetConnected(); } }
        public HashSet<int> notesOn { get; } = new HashSet<int>();

        void Awake()
        {
            InitNativeLibrary();
        }

        void Update()
        {
            if (!isInit) return;

            while (Native.ezmidi_get_next_event(ezmidiContext, ref midiEvent) != 0)
            {
                switch (midiEvent.type)
                {
                    case Native.Ezmidi_EventType.EZMIDI_NOTE:
                        ProcessNoteEvent(midiEvent);
                        break;
                }
            }
        }

        private void ProcessNoteEvent(Native.Ezmidi_Event ezmidiEvent)
        {
            switch (ezmidiEvent.note_event.detail)
            {
                case Native.Ezmidi_NoteEventId.EZMIDI_NOTEEVENT_ON:
                    notesOn.Add(ezmidiEvent.note_event.note);
                    NoteOn?.Invoke(ezmidiEvent.note_event.channel, ezmidiEvent.note_event.note, ezmidiEvent.note_event.velocity);
                    break;

                case Native.Ezmidi_NoteEventId.EZMIDI_NOTEEVENT_OFF:
                    notesOn.Remove(ezmidiEvent.note_event.note);
                    NoteOff?.Invoke(ezmidiEvent.note_event.channel, ezmidiEvent.note_event.note, ezmidiEvent.note_event.velocity);
                    break;
            }
        }

        private bool GetConnected()
        {
            if (!isInit)
                return false;

            return Native.ezmidi_has_source_connected(ezmidiContext) != 0;
        }

        private static void LogMessage(string message, IntPtr user_data)
        {
            Debug.Log(message);
        }

        private void InitNativeLibrary()
        {
            var config = new Native.Ezmidi_Config();
            Native.ezmidi_config_init(ref config);
            config.log_func = LogMessage;

            ezmidiContext = Native.ezmidi_create(ref config);

            if (ezmidiContext == IntPtr.Zero)
                throw new EzMidi.Exception("Ezmidi Initialization failed.");

            Debug.Log("Created ezmidi Context!");
            isInit = true;
        }

        public List<String> GetSourceNames()
        {
            List<string> sourceNames = new List<string>();

            if (isInit)
            {
                int numSources = Native.ezmidi_get_source_count(ezmidiContext);
                for (int i = 0; i < numSources; i++)
                {
                    IntPtr strPtr = Native.ezmidi_get_source_name(ezmidiContext, i);
                    string name = Marshal.PtrToStringAnsi(strPtr);
                    sourceNames.Add(name);
                }
            }

            return sourceNames;
        }

        public void ConnectSource(int index)
        {
            if (isInit)
                Native.ezmidi_connect_source(ezmidiContext, index);
        }

        public void DisconnectSource()
        {
            if (isInit)
                Native.ezmidi_disconnect_source(ezmidiContext);
        }

        private void OnDestroy()
        {
            if (ezmidiContext != IntPtr.Zero)
            {
                isInit = false;
                Native.ezmidi_destroy(ezmidiContext);
                ezmidiContext = IntPtr.Zero;

                Debug.Log("Destroyed ezmidi Context!");
            }
        }
    }
}


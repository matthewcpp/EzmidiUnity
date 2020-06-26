using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace EzMidi
{
    public static class Plugin
    {
        #region Callback Functions
        public delegate void Ezmidi_LogFunc(string message, IntPtr user_data);
        #endregion

        #region Enumerations
        public enum Ezmidi_Error
        {
            EZMIDI_ERROR_NONE = 0,
            EZMIDI_ERROR_INVALID_SOURCE,
            EZMIDI_ERROR_CONNECTION_FAILED,
            EZMIDI_ERROR_NO_SOURCE_CONNECTED
        }

        public enum Ezmidi_EventType
        {
            EZMIDI_NOTE
        }

        public enum Ezmidi_NoteEventId
        {
            EZMIDI_NOTEEVENT_ON,
            EZMIDI_NOTEEVENT_OFF
        }
        #endregion

        #region Structures
        [StructLayout(LayoutKind.Sequential)]
        public struct Ezmidi_Config
        {
            public Ezmidi_LogFunc log_func;
            public IntPtr user_data;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Ezmidi_NoteEvent
        {
            public Ezmidi_EventType type;
            public Ezmidi_NoteEventId detail;
            public int note;
            public int velocity;
            public int channel;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct Ezmidi_Event
        {
            [FieldOffset(0)]
            public Ezmidi_EventType type;

            [FieldOffset(0)]
            public Ezmidi_NoteEvent note_event;
        }
        #endregion

        #region Public API

        public static IntPtr ezmidi_create(ref Ezmidi_Config config)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_create(ref config);
            else
                return Dll.ezmidi_create(ref config);
        }

        public static void ezmidi_config_init(ref Ezmidi_Config config)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                IOS.ezmidi_config_init(ref config);
            else
                Dll.ezmidi_config_init(ref config);
        }

        public static void ezmidi_destroy(IntPtr context)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                IOS.ezmidi_destroy(context);
            else
                Dll.ezmidi_destroy(context);
        }

        public static int ezmidi_get_source_count(IntPtr context)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_get_source_count(context);
            else
                return Dll.ezmidi_get_source_count(context);
        }

        public static IntPtr ezmidi_get_source_name(IntPtr context, int source_index)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_get_source_name(context, source_index);
            else
                return Dll.ezmidi_get_source_name(context, source_index);
        }

        public static Ezmidi_Error ezmidi_connect_source(IntPtr context, int source)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_connect_source(context, source);
            else
                return Dll.ezmidi_connect_source(context, source);
        }

        public static Ezmidi_Error ezmidi_disconnect_source(IntPtr context)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_disconnect_source(context);
            else
                return Dll.ezmidi_disconnect_source(context);
        }

        public static int ezmidi_has_source_connected(IntPtr context)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_has_source_connected(context);
            else
                return Dll.ezmidi_has_source_connected(context);
        }

        public static int ezmidi_get_next_event(IntPtr context, ref Ezmidi_Event ezMidiEvent)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                return IOS.ezmidi_get_next_event(context, ref ezMidiEvent);
            else
                return Dll.ezmidi_get_next_event(context, ref ezMidiEvent);
        }
        
        #endregion
        
        #region Native Interface
        
        public static class IOS
        {
            [DllImport("__Internal")]
            public static extern IntPtr ezmidi_create(ref Ezmidi_Config config);

            [DllImport("__Internal")]
            public static extern void ezmidi_config_init(ref Ezmidi_Config config);

            [DllImport("__Internal")]
            public static extern void ezmidi_destroy(IntPtr context);

            [DllImport("__Internal")]
            public static extern int ezmidi_get_source_count(IntPtr context);

            [DllImport("__Internal")]
            public static extern IntPtr ezmidi_get_source_name(IntPtr context, int source_index);

            [DllImport("__Internal")]
            public static extern Ezmidi_Error ezmidi_connect_source(IntPtr context, int source);

            [DllImport("__Internal")]
            public static extern Ezmidi_Error ezmidi_disconnect_source(IntPtr context);

            [DllImport("__Internal")]
            public static extern int ezmidi_has_source_connected(IntPtr context);

            [DllImport("__Internal")]
            public static extern int ezmidi_get_next_event(IntPtr context, ref Ezmidi_Event ezMidiEvent);
        }

        public static class Dll
        {
            [DllImport("ezmidi")]
            public static extern IntPtr ezmidi_create(ref Ezmidi_Config config);

            [DllImport("ezmidi")]
            public static extern void ezmidi_config_init(ref Ezmidi_Config config);

            [DllImport("ezmidi")]
            public static extern void ezmidi_destroy(IntPtr context);

            [DllImport("ezmidi")]
            public static extern int ezmidi_get_source_count(IntPtr context);

            [DllImport("ezmidi")]
            public static extern IntPtr ezmidi_get_source_name(IntPtr context, int source_index);

            [DllImport("ezmidi")]
            public static extern Ezmidi_Error ezmidi_connect_source(IntPtr context, int source);

            [DllImport("ezmidi")]
            public static extern Ezmidi_Error ezmidi_disconnect_source(IntPtr context);

            [DllImport("ezmidi")]
            public static extern int ezmidi_has_source_connected(IntPtr context);

            [DllImport("ezmidi")]
            public static extern int ezmidi_get_next_event(IntPtr context, ref Ezmidi_Event ezMidiEvent);
        }

        #endregion
    }

}

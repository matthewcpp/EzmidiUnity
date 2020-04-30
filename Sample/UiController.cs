using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour {

    public EzMidi.Connection EzMidi;
    public Dropdown MidiSourcesDropdown;
    public Text ButtonText;

	void Start ()
    {
        var dropdownOptions = new List<Dropdown.OptionData>();
        var midiSources = EzMidi.GetSourceNames();

        foreach (var midiSource in midiSources)
            dropdownOptions.Add(new Dropdown.OptionData(midiSource));

        MidiSourcesDropdown.options = dropdownOptions;
    }

    public void OnConnectionButtonClick()
    {
        if (EzMidi.isConnected)
        {
            DisconnectSource();
            ButtonText.text = "Connect";
        }
        else
        {
            ConnectSelectedSource();
            ButtonText.text = "Disconnect";
        }
    }
	
    public void ConnectSelectedSource()
    {
        EzMidi.NoteOn += DebugNoteOn;
        EzMidi.NoteOff += DebugNoteOff;

        Debug.Log(string.Format("Connect source: {0}", MidiSourcesDropdown.value));
        EzMidi.ConnectSource(MidiSourcesDropdown.value);
    }

    public void DisconnectSource()
    {
        EzMidi.NoteOn -= DebugNoteOn;
        EzMidi.NoteOff -= DebugNoteOff;

        EzMidi.DisconnectSource();
    }


    void DebugNoteOn(int channel, int note, int velocity)
    {
        Debug.Log(string.Format("Note On: {0}, velocity: {1}, channel: {2}", note, velocity, channel));
    }

    void DebugNoteOff(int channel, int note, int velocity)
    {
        Debug.Log(string.Format("Note Off: {0}, velocity: {1}, channel: {2}", note, velocity, channel));
    }
}

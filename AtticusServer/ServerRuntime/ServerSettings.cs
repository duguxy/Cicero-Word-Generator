using System;
using System.Collections.Generic;
using System.Text;
using DataStructures;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AtticusServer
{
    /// <summary>
    /// Intended to store informaiton that the server needs on start up.
    /// </summary>
    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class ServerSettings : ServerSettingsInterface
    {
        private string serverName = "Unnamed Server";

        [Description("Name of the server. Note: This string is used to identify which hardware channel objects map to which physical outputs. Changing the field will also require updating hardware map."),
        Category("Global")]
        public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }

        private List<GpibRampCommandConverter> gpibRampConverters;

        [Description("Collection of customized GPIB command converters, to convert amplitude/frequency ramps to GPIB commands for devices which Atticus does not have built-in support for."),
        Category("GPIB")]
        public List<GpibRampCommandConverter> GpibRampConverters
        {
            get {
                if (gpibRampConverters == null)
                    gpibRampConverters = new List<GpibRampCommandConverter>();
                return gpibRampConverters; }
            set { gpibRampConverters = value; }
        }

        private string deviceToSyncSoftwareTimedTasksTo;

        [Description("Name of the device whose (hardware timed) output task the software timed tasks will be syncronized to. Software timed tasks include GPIB and RS232 output tasks."),
        Category("Timing")]
        public string DeviceToSyncSoftwareTimedTasksTo
        {
            get { return deviceToSyncSoftwareTimedTasksTo; }
            set { deviceToSyncSoftwareTimedTasksTo = value; }
        }

        public enum SoftwareTaskTriggerType { SampleClockEvent, PollBufferPosition };

        private SoftwareTaskTriggerType softwareTaskTriggerMethod;

        [Description("Mechanism by which the software timed tasks will be syncronized to a hardware timed tasks. The most generally useful mechanism is PollBufferPosition."),
        Category("Timing")]
        public SoftwareTaskTriggerType SoftwareTaskTriggerMethod
        {
            get { return softwareTaskTriggerMethod; }
            set { softwareTaskTriggerMethod = value; }
        }

        private bool useWatchdogTimerMonitoringTask;

        [Description("NOT YET IMPLEMENTED. Set this field to true if you would like Atticus to create a software-timed watchdog timer task. The watchdog timer task will periodically poll the device specified in DeviceToSyncSoftwareTimedTasksTo, and attempt to detect if the number of samples generated by that task has deviated from the expected number."),
        Category("Timing")]
        public bool UseWatchdogTimerMonitoringTask
        {
            get { return useWatchdogTimerMonitoringTask; }
            set { useWatchdogTimerMonitoringTask = value; }
        }


        private string readyInput;
        [Description("Hardware address of a digital input port to be used as a ready input. Atticus will poll this input continuously before triggering its sequence, waiting for the input to go high."),
        Category("Timing")]
        public string ReadyInput
        {
            get { return readyInput; }
            set { readyInput = value; }
        }

        private int readyTimeout;
        [Description("Timeout, in ms, to wait for the ready input to go high. Infinite timeout for value of 0."),
        Category("Timing")]
        public int ReadyTimeout
        {
            get { return readyTimeout; }
            set { readyTimeout = value; }
        }

        private bool readyTimeoutRunAnyway;

        [Description("If true, sequence will be run anyway after a timeout waiting for the ready input to go high. If false, it will not. Timeout duration specified in ReadyTimeout."),
        Category("Timing")]
        public bool ReadyTimeoutRunAnyway
        {
            get { return readyTimeoutRunAnyway; }
            set { readyTimeoutRunAnyway = value; }
        }

        private List<HardwareChannel> excludedChannels;

        [Description("A list of Hardware Channels which, even if they are detected, are to be excluded from the list of channels."),
        Category("Hardware")]
        public List<HardwareChannel> ExcludedChannels
        {
            get
            {
                if (excludedChannels == null)
                {
                    excludedChannels = new List<HardwareChannel>();
                }
                return excludedChannels;
            }
            set { excludedChannels = value; }
        }

        private string triggerOutputChannel;

        [Description("A string specifying the trigger output channel. Set to empty if there is no trigger output channel. (Note -- trigger ouput channel is not necessary for device synchronization. Instead, internally route the StartTrigger signal of a software triggered device to the StartTrigger signals of the other devices."),
        Category("Timing")]
        public string TriggerOutputChannel
        {
            get { return triggerOutputChannel; }
            set { triggerOutputChannel = value; }
        }

        private string variableTimebaseOutputChannel;

        [Description("Sets the channel over which the a variable timebase will be output."),
        Category("Timing")]
        public string VariableTimebaseOutputChannel
        {
            get { return variableTimebaseOutputChannel; }
            set { variableTimebaseOutputChannel = value; }
        }

        private SequenceData.VariableTimebaseTypes variableTimebaseType;

        [Description("Sets the variable timebase type."),
        Category("Timing")]
        public SequenceData.VariableTimebaseTypes VariableTimebaseType
        {
            get { return variableTimebaseType; }
            set { variableTimebaseType = value; }
        }

        private bool triggerSoftwareTasksAfterTimebaseTask;

        [Description("If set to true, software-timed tasks will be triggered after (instead of before) the variable timebase task is triggered (if there is a variable timebase task on this server). This setting only applies if the sync mechanism provided by DeviceToSyncSoftwareTimedTasksTo is not in use. On some configurations, the variable timebase task has a long delay between being told to trigger and when it actually does trigger, so setting this to true can improve the synchronization of starts between software timed tasks and hardware timed tasks on this server in the absence of the other sync mechanism."),
        Category("Timing")]
        public bool TriggerSoftwareTasksAfterTimebaseTask
        {
            get { return triggerSoftwareTasksAfterTimebaseTask; }
            set { triggerSoftwareTasksAfterTimebaseTask = value; }
        }

        private int variableTimebaseMasterFrequency;

        [Description("Sets the frequency of the master clock of the variable timebase."),
        Category("Timing")]
        public int VariableTimebaseMasterFrequency
        {
            get { return variableTimebaseMasterFrequency; }
            set { variableTimebaseMasterFrequency = value; }
        }

        private string variableTimebaseTriggerInput;

        [Description("The source for a start trigger to start the variable timebase clock. This is useful if you want to use a variable timebase, but still want to sync the start of your sequence to some external signal. Set to empty to have the variable timebase clock not require a start trigger."),
        Category("Timing")]
        public string VariableTimebaseTriggerInput
        {
            get
            {
                if (variableTimebaseTriggerInput == null)
                    variableTimebaseTriggerInput = "";
                return variableTimebaseTriggerInput;
            }
            set { variableTimebaseTriggerInput = value; }
        }

        private List<TerminalPair> connections;

        [Description("A list of terminal pairs which are to be connected internally, using the daqMx drivers. This list is to be used to route clock and trigger signals appropriately."),
        Category("Hardware")]
        public List<TerminalPair> Connections
        {
            get { return connections; }
            set { connections = value; }
        }

        private int aIFrequency=1000;

        [Description("Sampling frequency for the Analog In channels"),
        Category("Analog In")]
        public int AIFrequency
        {
            get { return aIFrequency; }
            set { aIFrequency = value; }
        }

        private List<AnalogInChannels> aIChannels;

        [Description("The list of Analog In channels which are to be used"),
        Category("Analog In")]
        public List<AnalogInChannels> AIChannels
        {
            get { return aIChannels; }
            set { aIChannels = value; }
        }

        private List<AnalogInNames> aINames;

        [Description("Custom names for the Analog In channels"),
        Category("Analog In")]
        public List<AnalogInNames> AINames
        {
            get { return aINames; }
            set { aINames = value; }
        }

        private List<AnalogInLogTime> aILogTimes;

        [Description("A list of timesteps at the beginning of which data will be saved"),
        Category("Analog In")]
        public List<AnalogInLogTime> AILogTimes
        {
            get { return aILogTimes; }
            set { aILogTimes = value; }
        }

        private bool useMultiThreadedDaqmxBufferGeneration;

        [Description("If true, daqmx buffers will be generated using multi-threading. This speeds up buffer generation time, especially on multi-processor machines. Downsides: uses more memory, and buffer errors will cause Atticus to crash rather than being handled intelligently."),
        Category("Software")]
        public bool UseMultiThreadedDaqmxBufferGeneration
        {
            get { return useMultiThreadedDaqmxBufferGeneration; }
            set { useMultiThreadedDaqmxBufferGeneration = value; }
        }




        /// <summary>
        /// Determines whether to automaticall turn on remoting when starting server.
        /// </summary>
        private bool connectOnStartup = false;

        [Description("Determines whether the server starts up in its connected state automatically"),
        Category("Global")]
        public bool ConnectOnStartup
        {
            get { return connectOnStartup; }
            set { connectOnStartup = value; }
        }

        /// <summary>
        /// This dictionary is indexed by device name.
        /// </summary>
        private Dictionary<string, DeviceSettings> serverDeviceSettings;

        [Description("A collection of the device settings for all of the devices attached to the server, indexed by the string device identifier."),
        Category("Hardware")]
        public Dictionary<string, DeviceSettings> myDevicesSettings
        {
            get { return serverDeviceSettings; }
            set { serverDeviceSettings = value; }
        }

        private List<rfsgDeviceName> rfsgDeviceNames;

        [TypeConverter(typeof(ExpandableObjectConverter)), Serializable]
        public class rfsgDeviceName
        {
            private string deviceName;

            public string DeviceName
            {
                get { return deviceName; }
                set { deviceName = value; }
            }
            public rfsgDeviceName()
            {
                deviceName = "";
            }

            public override string ToString()
            {
                return deviceName;
            }
        }

        [Description("A list of the MAX Device names for RFSG signal generators to be used on this server (for example, PXI-5650 cards). Atticus is not able to auto-detect these devices, and thus must be informed of their existence by this list. It is recommended to name your RFSG devices RF1, RF2, etc."),
        Category("Hardware")]
        public List<rfsgDeviceName> RfsgDeviceNames
        {
            get {
                if (rfsgDeviceNames == null)
                {
                    rfsgDeviceNames = new List<rfsgDeviceName>();
                }
                return rfsgDeviceNames; }
            set { rfsgDeviceNames = value; }
        }

        private bool useOpalKellyFPGA;
        [Description("Set to true if you are using an Opal Kelly FPGA module, and want Atticus to scan for such modules. Atticus is able to use these FPGA modules to efficiently create variable frequency clocks."),
        Category("Opal Kelly FPGA")]
        public bool UseOpalKellyFPGA
        {
            get { return useOpalKellyFPGA; }
            set { useOpalKellyFPGA = value; }
        }

        private bool silenceSoftwareTimebaseDelayError;

        [Category("Error Reporting")]
        public bool SilenceSoftwareTimebaseDelayError
        {
            get { return silenceSoftwareTimebaseDelayError; }
            set { silenceSoftwareTimebaseDelayError = value; }
        }

        private bool useFpgaMistriggerDetection;

        [Category("Opal Kelly FPGA")]
        public bool UseFpgaMistriggerDetection
        {
            get { return useFpgaMistriggerDetection; }
            set { useFpgaMistriggerDetection = value; }
        }

        private bool useFpgaRfModulatedClockOutput;

        [Category("Opal Kelly FPGA")]
        public bool UseFpgaRfModulatedClockOutput
        {
            get { return useFpgaRfModulatedClockOutput; }
            set { useFpgaRfModulatedClockOutput = value; }
        }

        private bool useFpgaAssymetricDutyCycleClocking;

        [Category("Opal Kelly FPGA")]
        public bool UseFpgaAssymetricDutyCycleClocking
        {
            get { return useFpgaAssymetricDutyCycleClocking; }
            set { useFpgaAssymetricDutyCycleClocking = value; }
        }

        public ServerSettings()
        {
            this.serverDeviceSettings = new Dictionary<string, DeviceSettings>();
            this.connections = new List<TerminalPair>();
            this.aILogTimes = new List<AnalogInLogTime>();
            this.aIChannels = new List<AnalogInChannels>();
            this.aINames = new List<AnalogInNames>();
            this.versionNumberAtFirstCreation = DataStructuresVersionNumber.CurrentVersion;
            this.versionNumberAtLastSerialization = DataStructuresVersionNumber.CurrentVersion;
        }


        #region Version Number Tracking

        private DataStructuresVersionNumber versionNumberAtFirstCreation;

        public DataStructuresVersionNumber VersionNumberAtFirstCreation
        {
            get { return versionNumberAtFirstCreation; }
        }


        private DataStructuresVersionNumber versionNumberAtLastSerialization;

        public DataStructuresVersionNumber VersionNumberAtLastSerialization
        {
            get { return versionNumberAtLastSerialization; }
        }

        [OnSerializing]
        private void setSerializationVersionNumber(StreamingContext sc)
        {
            this.versionNumberAtLastSerialization = DataStructuresVersionNumber.CurrentVersion;
        }

        #endregion

    }
}

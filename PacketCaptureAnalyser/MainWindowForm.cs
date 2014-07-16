// $Id$
// $URL$
// <copyright file="MainWindowForm.cs" company="Public Domain">
//     Released into the public domain
// </copyright>

// This file is part of the C# Packet Capture application. It is free and
// unencumbered software released into the public domain as detailed in
// the UNLICENSE file in the top level directory of this distribution

namespace PacketCaptureAnalyser
{
    /// <summary>
    /// This class provides the main window form
    /// </summary>
    public partial class MainWindowForm : System.Windows.Forms.Form
    {
        /// <summary>
        /// The path of the selected packet capture
        /// </summary>
        private string theSelectedPacketCapturePath;

        /// <summary>
        /// The type of the selected packet capture
        /// </summary>
        private MainWindowFormPacketCaptureTypeEnumeration theSelectedPacketCaptureType;

        /// <summary>
        /// Initializes a new instance of the MainWindowForm class
        /// </summary>
        public MainWindowForm()
        {
            this.InitializeComponent();

            // Clear the selected packet capture on window form creation
            this.ClearSelectedPacketCapture(this);
        }

        /// <summary>
        /// Enumerated list of the types of packet captures supported by the main window form
        /// </summary>
        private enum MainWindowFormPacketCaptureTypeEnumeration
        {
            /// <summary>
            /// PCAP Next Generation capture
            /// </summary>
            PCAPNextGeneration = 0,

            /// <summary>
            /// PCAP packet capture
            /// </summary>
            PCAP = 1,

            /// <summary>
            /// NA Sniffer (DOS) packet capture
            /// </summary>
            NASnifferDOS = 2,

            /// <summary>
            /// Invalid value for type of packet capture
            /// </summary>
            Incorrect = 3,

            /// <summary>
            /// Unknown value for type of packet capture
            /// </summary>
            Unknown = 4
        }

        //// Button and label click actions

        /// <summary>
        /// Processes the button click event for the "Select Packet Capture" button
        /// </summary>
        /// <param name="sender">The sender for the button click event</param>
        /// <param name="e">The arguments for the button click event</param>
        private void SelectPacketCaptureButton_Click(object sender, System.EventArgs e)
        {
            // Open the packet capture selection dialog box
            System.Windows.Forms.DialogResult theSelectedPacketCaptureDialogResult =
                this.theSelectedPacketCaptureForAnalysisDialog.ShowDialog();

            if (theSelectedPacketCaptureDialogResult == System.Windows.Forms.DialogResult.OK)
            {
                // Read the path of the selected packet capture and store it for use by later processing
                this.theSelectedPacketCapturePath =
                    this.theSelectedPacketCaptureForAnalysisDialog.FileName;

                // Determine the type of the packet capture
                this.DeterminePacketCaptureType();

                // Check the type of the packet capture
                this.CheckPacketCaptureType();

                // Update the window to reflect the selected packet capture
                this.ReflectSelectedPacketCapture(sender);

                System.Diagnostics.Debug.WriteLine("Selection of the " +
                    System.IO.Path.GetFileName(this.theSelectedPacketCapturePath) +
                    " packet capture");
            }
        }

        /// <summary>
        /// Processes the button click event for the "Clear Selected Packet Capture" button
        /// </summary>
        /// <param name="sender">The sender for the button click event</param>
        /// <param name="e">The arguments for the button click event</param>
        private void ClearSelectedPacketCaptureButton_Click(object sender, System.EventArgs e)
        {
            // Clear the selected packet capture on user request
            this.ClearSelectedPacketCapture(sender);
        }

        /// <summary>
        /// Processes the button click event for the "Open Selected Packet Capture" button
        /// </summary>
        /// <param name="sender">The sender for the button click event</param>
        /// <param name="e">The arguments for the button click event</param>
        private void OpenSelectedPackageCaptureButton_Click(object sender, System.EventArgs e)
        {
            if (System.IO.File.Exists(this.theSelectedPacketCapturePath))
            {
                try
                {
                    System.Diagnostics.Process.Start(this.theSelectedPacketCapturePath);
                }
                catch (System.ComponentModel.Win32Exception f)
                {
                    System.Diagnostics.Debug.WriteLine("The exception " +
                        f.GetType().Name +
                        " with the following message: " +
                        f.Message +
                        " was raised as there is no application registered that can open the " +
                        System.IO.Path.GetFileName(this.theSelectedPacketCapturePath) +
                        " packet capture!!!");
                }
            }
        }

        /// <summary>
        /// Processes the check changed event for the "Enable" check box in the "Debug Information" group box
        /// </summary>
        /// <param name="sender">The sender for the check changed event</param>
        /// <param name="e">The arguments for the check changed event</param>
        private void EnableDebugInformationCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // Guard against infinite recursion
            if ((sender as System.Windows.Forms.Control).ContainsFocus)
            {
                if (this.theEnableDebugInformationCheckBox.Checked)
                {
                    this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                    this.EnablePacketCaptureAnalysisCheckBoxes(sender);
                }
                else
                {
                    this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                    this.DisablePacketCaptureAnalysisCheckBoxes(sender);
                }
            }
        }

        /// <summary>
        /// Processes the check changed event for the "Perform" check box in the "Latency Analysis" group box
        /// </summary>
        /// <param name="sender">The sender for the check changed event</param>
        /// <param name="e">The arguments for the check changed event</param>
        private void PerformLatencyAnalysisCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // Guard against infinite recursion
            if ((sender as System.Windows.Forms.Control).ContainsFocus)
            {
                if (this.thePerformLatencyAnalysisCheckBox.Checked)
                {
                    this.theOutputLatencyAnalysisHistogramCheckBox.Checked = false;
                    this.theOutputLatencyAnalysisHistogramCheckBox.Enabled = true;

                    this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Checked = false;
                    this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Enabled = true;
                }
                else
                {
                    this.theOutputLatencyAnalysisHistogramCheckBox.Checked = false;
                    this.theOutputLatencyAnalysisHistogramCheckBox.Enabled = false;

                    this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Checked = false;
                    this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Processes the check changed event for the "Perform" check box in the "Burst Analysis" group box
        /// </summary>
        /// <param name="sender">The sender for the check changed event</param>
        /// <param name="e">The arguments for the check changed event</param>
        private void PerformBurstAnalysisCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // Guard against infinite recursion
            if ((sender as System.Windows.Forms.Control).ContainsFocus)
            {
                if (this.thePerformBurstAnalysisCheckBox.Checked)
                {
                    this.theOutputBurstAnalysisHistogramCheckBox.Checked = false;
                    this.theOutputBurstAnalysisHistogramCheckBox.Enabled = true;

                    this.theOutputAdditionalBurstAnalysisInformationCheckBox.Checked = false;
                    this.theOutputAdditionalBurstAnalysisInformationCheckBox.Enabled = true;
                }
                else
                {
                    this.theOutputBurstAnalysisHistogramCheckBox.Checked = false;
                    this.theOutputBurstAnalysisHistogramCheckBox.Enabled = false;

                    this.theOutputAdditionalTimeAnalysisInformationCheckBox.Checked = false;
                    this.theOutputAdditionalTimeAnalysisInformationCheckBox.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Processes the check changed event for the "Perform" check box in the "Time Analysis" group box
        /// </summary>
        /// <param name="sender">The sender for the check changed event</param>
        /// <param name="e">The arguments for the check changed event</param>
        private void PerformTimeAnalysisCheckBox_CheckedChanged(object sender, System.EventArgs e)
        {
            // Guard against infinite recursion
            if ((sender as System.Windows.Forms.Control).ContainsFocus)
            {
                if (this.thePerformTimeAnalysisCheckBox.Checked)
                {
                    this.theOutputTimeAnalysisHistogramCheckBox.Checked = false;
                    this.theOutputTimeAnalysisHistogramCheckBox.Enabled = true;

                    this.theOutputAdditionalTimeAnalysisInformationCheckBox.Checked = false;
                    this.theOutputAdditionalTimeAnalysisInformationCheckBox.Enabled = true;
                }
                else
                {
                    this.theOutputTimeAnalysisHistogramCheckBox.Checked = false;
                    this.theOutputTimeAnalysisHistogramCheckBox.Enabled = false;

                    this.theOutputAdditionalTimeAnalysisInformationCheckBox.Checked = false;
                    this.theOutputAdditionalTimeAnalysisInformationCheckBox.Enabled = false;
                }
            }
        }

        /// <summary>
        /// Processes the button click event for the "Run Analysis On Packet Capture" button
        /// </summary>
        /// <param name="sender">The sender for the button click event</param>
        /// <param name="e">The arguments for the button click event</param>
        private void RunAnalysisOnPacketCaptureButton_Click(object sender, System.EventArgs e)
        {
            this.AnalysePacketCapture();
        }

        /// <summary>
        /// Processes the button click event for the "Exit" button
        /// </summary>
        /// <param name="sender">The sender for the button click event</param>
        /// <param name="e">The arguments for the button click event</param>
        private void ExitButton_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        //// Packet capture support functions

        /// <summary>
        /// Reflects the currently selected packet capture by setting up the associated items accordingly
        /// </summary>
        /// <param name="sender">The sender for the initiating event</param>
        private void ReflectSelectedPacketCapture(object sender)
        {
            switch (this.theSelectedPacketCaptureType)
            {
                case MainWindowFormPacketCaptureTypeEnumeration.PCAPNextGeneration:
                    {
                        //// Analysis of a PCAP Next Generation packet capture is supported

                        // Enable the "Selected Packet Capture" group box
                        this.theSelectedPacketCaptureGroupBox.Enabled = true;

                        // Populate the path for the selected packet capture
                        this.theSelectedPacketCapturePathTextBox.Text =
                            System.IO.Path.GetDirectoryName(this.theSelectedPacketCapturePath);

                        // Populate the name for the selected packet capture
                        this.theSelectedPacketCaptureNameTextBox.Text =
                            System.IO.Path.GetFileName(this.theSelectedPacketCapturePath);

                        // This is a PCAP Next Generation packet capture
                        this.theSelectedPacketCaptureTypeTextBox.Text = "PCAP Next Generation";

                        // Enable the buttons
                        this.EnablePacketCaptureAnalysisButtons();

                        // Reset and enable the check boxes
                        this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                        this.EnablePacketCaptureAnalysisCheckBoxes(sender);

                        break;
                    }

                case MainWindowFormPacketCaptureTypeEnumeration.PCAP:
                    {
                        //// Analysis of a PCAP packet capture is supported

                        // Enable the "Selected Packet Capture" group box
                        this.theSelectedPacketCaptureGroupBox.Enabled = true;

                        // Populate the path for the selected packet capture
                        this.theSelectedPacketCapturePathTextBox.Text =
                            System.IO.Path.GetDirectoryName(this.theSelectedPacketCapturePath);

                        // Populate the name for the selected packet capture
                        this.theSelectedPacketCaptureNameTextBox.Text =
                            System.IO.Path.GetFileName(this.theSelectedPacketCapturePath);

                        // This is a PCAP packet capture
                        this.theSelectedPacketCaptureTypeTextBox.Text = "PCAP (libpcap/tcpdump)";

                        // Enable the buttons
                        this.EnablePacketCaptureAnalysisButtons();

                        // Reset and enable the check boxes
                        this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                        this.EnablePacketCaptureAnalysisCheckBoxes(sender);

                        break;
                    }

                case MainWindowFormPacketCaptureTypeEnumeration.NASnifferDOS:
                    {
                        //// Analysis of an NA Sniffer (DOS) packet capture is supported

                        // Enable the "Selected Packet Capture" group box
                        this.theSelectedPacketCaptureGroupBox.Enabled = true;

                        // Populate the path for the selected packet capture
                        this.theSelectedPacketCapturePathTextBox.Text =
                            System.IO.Path.GetDirectoryName(this.theSelectedPacketCapturePath);

                        // Populate the name for the selected packet capture
                        this.theSelectedPacketCaptureNameTextBox.Text =
                            System.IO.Path.GetFileName(this.theSelectedPacketCapturePath);

                        // This is an NA Sniffer (DOS) packet capture
                        this.theSelectedPacketCaptureTypeTextBox.Text = "NA Sniffer (DOS)";

                        // Enable the buttons
                        this.EnablePacketCaptureAnalysisButtons();

                        // Reset and enable the check boxes
                        this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                        this.EnablePacketCaptureAnalysisCheckBoxes(sender);

                        break;
                    }

                case MainWindowFormPacketCaptureTypeEnumeration.Incorrect:
                    {
                        //// Analysis of this packet capture is not supported

                        // Enable the "Selected Packet Capture" group box
                        this.theSelectedPacketCaptureGroupBox.Enabled = true;

                        // Populate the path for the selected packet capture
                        this.theSelectedPacketCapturePathTextBox.Text =
                            System.IO.Path.GetDirectoryName(this.theSelectedPacketCapturePath);

                        // Populate the name for the selected packet capture
                        this.theSelectedPacketCaptureNameTextBox.Text =
                            System.IO.Path.GetFileName(this.theSelectedPacketCapturePath);

                        // This packet capture has an incorrect file extension for the form of packet capture it contains
                        this.theSelectedPacketCaptureTypeTextBox.Text = "<Incorrect Packet Capture Type For File Extension>";

                        // Reset the buttons
                        this.ResetPacketCaptureAnalysisButtons();

                        // Reset and disable the check boxes
                        this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                        this.DisablePacketCaptureAnalysisCheckBoxes(sender);

                        break;
                    }

                case MainWindowFormPacketCaptureTypeEnumeration.Unknown:
                default:
                    {
                        //// Analysis of this packet capture is not supported

                        // Enable the "Selected Packet Capture" group box
                        this.theSelectedPacketCaptureGroupBox.Enabled = true;

                        // Populate the path for the selected packet capture
                        this.theSelectedPacketCapturePathTextBox.Text =
                            System.IO.Path.GetDirectoryName(this.theSelectedPacketCapturePath);

                        // Populate the name for the selected packet capture
                        this.theSelectedPacketCaptureNameTextBox.Text =
                            System.IO.Path.GetFileName(this.theSelectedPacketCapturePath);

                        // This packet capture is either an unsupported form of packet capture or is another type of file
                        this.theSelectedPacketCaptureTypeTextBox.Text = "<Unknown Packet Capture Type>";

                        // Reset the buttons
                        this.ResetPacketCaptureAnalysisButtons();

                        // Reset and disable the check boxes
                        this.ResetPacketCaptureAnalysisCheckBoxes(sender);
                        this.DisablePacketCaptureAnalysisCheckBoxes(sender);

                        break;
                    }
            }
        }

        /// <summary>
        /// Clears the currently selected packet capture and resets the associated items
        /// </summary>
        /// <param name="sender">The sender for the initiating event</param>
        private void ClearSelectedPacketCapture(object sender)
        {
            this.theSelectedPacketCapturePath = null;

            this.theSelectedPacketCaptureType =
                MainWindowFormPacketCaptureTypeEnumeration.Unknown;

            // Disable the "Selected Packet Capture" group box
            this.theSelectedPacketCaptureGroupBox.Enabled = false;

            // Clear the path, name and type for the selected packet capture
            this.theSelectedPacketCapturePathTextBox.Text = string.Empty;
            this.theSelectedPacketCaptureNameTextBox.Text = string.Empty;
            this.theSelectedPacketCaptureTypeTextBox.Text = string.Empty;

            // Disable the button to clear the selected package capture
            this.theClearSelectedPacketCaptureButton.Enabled = false;

            // Disable the button to open the package capture
            this.theOpenSelectedPackageCaptureButton.Enabled = false;

            // Disable the button to start the analysis on the packet capture
            this.theRunAnalysisOnSelectedPackageCaptureButton.Enabled = false;

            // Reset and disable the check boxes
            this.ResetPacketCaptureAnalysisCheckBoxes(sender);
            this.DisablePacketCaptureAnalysisCheckBoxes(sender);
        }

        //// Packet capture type

        /// <summary>
        /// Determines the type of the packet capture from the initial bytes
        /// </summary>
        private void DeterminePacketCaptureType()
        {
            //// Determine the type of the packet capture

            // Open a file stream for the packet capture for reading
            using (System.IO.FileStream theFileStream =
                System.IO.File.OpenRead(this.theSelectedPacketCapturePath))
            {
                // Open a binary reader for the file stream for the packet capture
                using (System.IO.BinaryReader theBinaryReader =
                    new System.IO.BinaryReader(theFileStream))
                {
                    switch (theBinaryReader.ReadUInt32())
                    {
                        case (uint)PacketCapture.PCAPNGPackageCapture.Constants.BlockType.SectionHeaderBlock:
                            {
                                // This is a PCAP Next Generation packet capture
                                this.theSelectedPacketCaptureType =
                                    MainWindowFormPacketCaptureTypeEnumeration.PCAPNextGeneration;

                                break;
                            }

                        case (uint)PacketCapture.PCAPPackageCapture.Constants.LittleEndianMagicNumber:
                        case (uint)PacketCapture.PCAPPackageCapture.Constants.BigEndianMagicNumber:
                            {
                                // This is a PCAP packet capture
                                this.theSelectedPacketCaptureType =
                                    MainWindowFormPacketCaptureTypeEnumeration.PCAP;

                                break;
                            }

                        case (uint)PacketCapture.SnifferPackageCapture.Constants.ExpectedMagicNumberHighest:
                            {
                                // This is a NA Sniffer (DOS) packet capture
                                this.theSelectedPacketCaptureType =
                                    MainWindowFormPacketCaptureTypeEnumeration.NASnifferDOS;

                                break;
                            }

                        default:
                            {
                                // This packet capture is either an unsupported form of packet capture or is another type of file
                                this.theSelectedPacketCaptureType =
                                    MainWindowFormPacketCaptureTypeEnumeration.Unknown;

                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Checks the type of the packet capture to ensure that it is of the expected type for the file extension
        /// </summary>
        private void CheckPacketCaptureType()
        {
            //// Check the type of the packet capture

            // Determine the expected type of the packet capture from the file extension
            switch (System.IO.Path.GetExtension(this.theSelectedPacketCapturePath))
            {
                case ".pcapng":
                case ".ntar":
                    {
                        // This should be a PCAP Next Generation packet capture
                        if (this.theSelectedPacketCaptureType != MainWindowFormPacketCaptureTypeEnumeration.PCAPNextGeneration)
                        {
                            System.Diagnostics.Debug.WriteLine("The " +
                                System.IO.Path.GetFileName(this.theSelectedPacketCapturePath) +
                                " packet capture should be a PCAP Next Generation packet capture based on its file extension, but it is not!!!");

                            this.theSelectedPacketCaptureType =
                                MainWindowFormPacketCaptureTypeEnumeration.Incorrect;
                        }

                        break;
                    }

                case ".pcap":
                case ".libpcap":
                case ".cap":
                    {
                        // This should be a PCAP packet capture
                        if (this.theSelectedPacketCaptureType != MainWindowFormPacketCaptureTypeEnumeration.PCAP)
                        {
                            System.Diagnostics.Debug.WriteLine("The " +
                                System.IO.Path.GetFileName(this.theSelectedPacketCapturePath) +
                                " packet capture should be a PCAP packet capture based on its file extension, but it is not!!!");

                            this.theSelectedPacketCaptureType =
                                MainWindowFormPacketCaptureTypeEnumeration.Incorrect;
                        }

                        break;
                    }

                case ".enc":
                    {
                        // This should be an NA Sniffer (DOS) packet capture
                        if (this.theSelectedPacketCaptureType != MainWindowFormPacketCaptureTypeEnumeration.NASnifferDOS)
                        {
                            System.Diagnostics.Debug.WriteLine("The " +
                                System.IO.Path.GetFileName(this.theSelectedPacketCapturePath) +
                                " packet capture should be a NA Sniffer (DOS) packet capture based on its file extension, but it is not!!!");

                            this.theSelectedPacketCaptureType =
                                MainWindowFormPacketCaptureTypeEnumeration.Incorrect;
                        }

                        break;
                    }

                default:
                    {
                        // This packet capture is either an unsupported form of packet capture or is another type of file
                        this.theSelectedPacketCaptureType =
                            MainWindowFormPacketCaptureTypeEnumeration.Unknown;

                        break;
                    }
            }
        }

        /// <summary>
        /// Performs analysis of the currently selected packet capture
        /// </summary>
        private void AnalysePacketCapture()
        {
            bool theResult = true;

            // Create an instance of the progress window form
            using (ProgressWindowForm theProgressWindowForm = new PacketCaptureAnalyser.ProgressWindowForm())
            {
                // Show the progress window form now the analysis has started
                theProgressWindowForm.Show();
                theProgressWindowForm.Activate();

                // Update the label now the reading of the packet capture has started - the progress bar will stay at zero
                theProgressWindowForm.ProgressBarLabel = "Reading Packet Capture Into System Memory";
                theProgressWindowForm.ProgressBar = 0;
                theProgressWindowForm.Refresh();

                theProgressWindowForm.ProgressBar = 5;

                string thePacketCaptureFileName = System.IO.Path.GetFileName(this.theSelectedPacketCapturePath);

                theProgressWindowForm.ProgressBar = 10;

                using (Analysis.DebugInformation theDebugInformation =
                    new Analysis.DebugInformation(
                        this.theSelectedPacketCapturePath + ".txt",
                        this.theEnableDebugInformationCheckBox.Checked,
                        this.theEnableInformationEventsInDebugInformationCheckBox.Checked,
                        this.theRedirectDebugInformationToOutputCheckBox.Checked))
                {
                    theProgressWindowForm.ProgressBar = 20;

                    // Start the processing of the packet capture
                    theDebugInformation.WriteTestRunEvent("Processing of the " +
                        thePacketCaptureFileName +
                        " packet capture started");

                    theDebugInformation.WriteBlankLine();

                    theDebugInformation.WriteTextLine(new string('=', 144));

                    theDebugInformation.WriteBlankLine();

                    Analysis.LatencyAnalysis.Processing theLatencyAnalysisProcessing = null;
                    Analysis.BurstAnalysis.Processing theBurstAnalysisProcessing = null;
                    Analysis.TimeAnalysis.Processing theTimeAnalysisProcessing = null;

                    theProgressWindowForm.ProgressBar = 30;

                    // Only perform the latency analysis if the check box was selected for it on the main window form
                    if (this.thePerformLatencyAnalysisCheckBox.Checked)
                    {
                        theLatencyAnalysisProcessing =
                            new Analysis.LatencyAnalysis.Processing(
                                theDebugInformation,
                                this.theOutputLatencyAnalysisHistogramCheckBox.Checked,
                                this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Checked,
                                this.theSelectedPacketCapturePath);

                        // Initialise the functionality to perform latency analysis on the messages found
                        theLatencyAnalysisProcessing.Create();
                    }

                    theProgressWindowForm.ProgressBar = 35;

                    // Only perform the burst analysis if the check box was selected for it on the main window form
                    if (this.thePerformBurstAnalysisCheckBox.Checked)
                    {
                        theBurstAnalysisProcessing =
                            new Analysis.BurstAnalysis.Processing(
                                theDebugInformation,
                                this.theOutputBurstAnalysisHistogramCheckBox.Checked,
                                this.theOutputAdditionalBurstAnalysisInformationCheckBox.Checked,
                                this.theSelectedPacketCapturePath);

                        // Initialise the functionality to perform burst analysis on the messages found
                        theBurstAnalysisProcessing.Create();
                    }

                    theProgressWindowForm.ProgressBar = 40;

                    // Only perform the time analysis if the check box was selected for it on the main window form
                    if (this.thePerformTimeAnalysisCheckBox.Checked)
                    {
                        theTimeAnalysisProcessing =
                            new Analysis.TimeAnalysis.Processing(
                                theDebugInformation,
                                this.theOutputTimeAnalysisHistogramCheckBox.Checked,
                                this.theOutputAdditionalTimeAnalysisInformationCheckBox.Checked,
                                this.theSelectedPacketCapturePath);

                        // Initialise the functionality to perform time analysis on the messages found
                        theTimeAnalysisProcessing.Create();
                    }

                    theProgressWindowForm.ProgressBar = 45;

                    switch (this.theSelectedPacketCaptureType)
                    {
                        case MainWindowFormPacketCaptureTypeEnumeration.PCAPNextGeneration:
                            {
                                theDebugInformation.WriteInformationEvent("This is a PCAP Next Generation packet capture");

                                PacketCapture.PCAPNGPackageCapture.Processing thePCAPNGPackageCaptureProcessing =
                                    new PacketCapture.PCAPNGPackageCapture.Processing(
                                        theProgressWindowForm,
                                        theDebugInformation,
                                        this.thePerformLatencyAnalysisCheckBox.Checked,
                                        theLatencyAnalysisProcessing,
                                        this.thePerformBurstAnalysisCheckBox.Checked,
                                        theBurstAnalysisProcessing,
                                        this.thePerformTimeAnalysisCheckBox.Checked,
                                        theTimeAnalysisProcessing,
                                        this.theSelectedPacketCapturePath,
                                        this.theUseAlternativeSequenceNumberCheckBox.Checked,
                                        this.theMinimizeMemoryUsageCheckBox.Checked);

                                theProgressWindowForm.ProgressBar = 50;

                                theResult = thePCAPNGPackageCaptureProcessing.ProcessPacketCapture();

                                break;
                            }

                        case MainWindowFormPacketCaptureTypeEnumeration.PCAP:
                            {
                                theDebugInformation.WriteInformationEvent("This is a PCAP packet capture");

                                PacketCapture.PCAPPackageCapture.Processing thePCAPPackageCaptureProcessing =
                                    new PacketCapture.PCAPPackageCapture.Processing(
                                        theProgressWindowForm,
                                        theDebugInformation,
                                        this.thePerformLatencyAnalysisCheckBox.Checked,
                                        theLatencyAnalysisProcessing,
                                        this.thePerformBurstAnalysisCheckBox.Checked,
                                        theBurstAnalysisProcessing,
                                        this.thePerformTimeAnalysisCheckBox.Checked,
                                        theTimeAnalysisProcessing,
                                        this.theSelectedPacketCapturePath,
                                        this.theUseAlternativeSequenceNumberCheckBox.Checked,
                                        this.theMinimizeMemoryUsageCheckBox.Checked);

                                theProgressWindowForm.ProgressBar = 50;

                                theResult = thePCAPPackageCaptureProcessing.ProcessPacketCapture();

                                break;
                            }

                        case MainWindowFormPacketCaptureTypeEnumeration.NASnifferDOS:
                            {
                                theDebugInformation.WriteInformationEvent("This is an NA Sniffer (DOS) packet capture");

                                PacketCapture.SnifferPackageCapture.Processing theSnifferPackageCaptureProcessing =
                                    new PacketCapture.SnifferPackageCapture.Processing(
                                        theProgressWindowForm,
                                        theDebugInformation,
                                        this.thePerformLatencyAnalysisCheckBox.Checked,
                                        theLatencyAnalysisProcessing,
                                        this.thePerformBurstAnalysisCheckBox.Checked,
                                        theBurstAnalysisProcessing,
                                        this.thePerformTimeAnalysisCheckBox.Checked,
                                        theTimeAnalysisProcessing,
                                        this.theSelectedPacketCapturePath,
                                        this.theUseAlternativeSequenceNumberCheckBox.Checked,
                                        this.theMinimizeMemoryUsageCheckBox.Checked);

                                theProgressWindowForm.ProgressBar = 50;

                                theResult = theSnifferPackageCaptureProcessing.ProcessPacketCapture();

                                break;
                            }

                        case MainWindowFormPacketCaptureTypeEnumeration.Unknown:
                        default:
                            {
                                theDebugInformation.WriteErrorEvent(
                                    "The" +
                                    thePacketCaptureFileName +
                                    " packet capture is of an unknown type!!!");

                                theResult = false;

                                break;
                            }
                    }

                    // Dependent on the result of the processing above, display a debug message to indicate success or otherwise
                    if (theResult)
                    {
                        // Display a debug message to indicate parsing of the packet capture completed successfully
                        theDebugInformation.WriteTestRunEvent("Parsing of the " +
                            thePacketCaptureFileName +
                            " packet capture completed successfully");

                        int theScaling = 0;

                        if (this.thePerformLatencyAnalysisCheckBox.Checked ||
                            this.thePerformTimeAnalysisCheckBox.Checked ||
                            true)
                        {
                            // Update the label now the analysis of the packet capture has started - the progress bar will stay at zero
                            theProgressWindowForm.ProgressBarLabel = "Performing Analysis Of Packet Capture";
                            theProgressWindowForm.ProgressBar = 0;
                            theProgressWindowForm.Refresh();

                            // Calculate the scaling to use for the progress bar
                            if (this.thePerformLatencyAnalysisCheckBox.Checked &&
                                !this.thePerformTimeAnalysisCheckBox.Checked)
                            {
                                // Only one set of analysis will be run
                                theScaling = 1;
                            }
                            else if (!this.thePerformLatencyAnalysisCheckBox.Checked &&
                                this.thePerformTimeAnalysisCheckBox.Checked)
                            {
                                // Only one set of analysis will be run
                                theScaling = 1;
                            }
                            else
                            {
                                // Both sets of analysis will be run
                                theScaling = 2;
                            }
                        }

                        // Only perform the latency analysis if the check box was selected for it on the main window form
                        if (this.thePerformLatencyAnalysisCheckBox.Checked)
                        {
                            theDebugInformation.WriteBlankLine();

                            theDebugInformation.WriteTextLine(new string('=', 144));

                            theDebugInformation.WriteBlankLine();

                            //// Finalise the latency analysis on the messages found including printing the results to debug output
                            //// Only perform this action if the analysis of the packet capture completed successfully

                            // Read the start time to allow later calculation of the duration of the latency analysis finalisation
                            System.DateTime theLatencyAnalysisStartTime = System.DateTime.Now;

                            theDebugInformation.WriteTestRunEvent("Latency analysis for the " +
                                thePacketCaptureFileName +
                                " packet capture started");

                            theProgressWindowForm.ProgressBar += 20 / theScaling;

                            theLatencyAnalysisProcessing.Finalise();

                            theProgressWindowForm.ProgressBar += 60 / theScaling;

                            //// Compute the duration between the start and the end times

                            System.DateTime theLatencyAnalysisEndTime = System.DateTime.Now;

                            System.TimeSpan theLatencyAnalysisDuration =
                                theLatencyAnalysisEndTime - theLatencyAnalysisStartTime;

                            theDebugInformation.WriteTestRunEvent("Latency analysis for the " +
                                thePacketCaptureFileName +
                                " packet capture completed in " +
                                theLatencyAnalysisDuration.TotalSeconds.ToString() +
                                " seconds");

                            theProgressWindowForm.ProgressBar += 20 / theScaling;
                        }

                        // Only perform the burst analysis if the check box was selected for it on the main window form
                        if (this.thePerformBurstAnalysisCheckBox.Checked)
                        {
                            theDebugInformation.WriteBlankLine();

                            theDebugInformation.WriteTextLine(new string('=', 144));

                            theDebugInformation.WriteBlankLine();

                            //// Finalise the burst analysis on the messages found including printing the results to debug output
                            //// Only perform this action if the analysis of the packet capture completed successfully

                            // Read the start time to allow later calculation of the duration of the burst analysis finalisation
                            System.DateTime theBurstAnalysisStartTime = System.DateTime.Now;

                            theDebugInformation.WriteTestRunEvent("Burst analysis for the " +
                                thePacketCaptureFileName +
                                " packet capture started");

                            //// theProgressWindowForm.ProgressBar += 20 / theScaling;

                            theBurstAnalysisProcessing.Finalise();

                            //// theProgressWindowForm.ProgressBar += 60 / theScaling;

                            //// Compute the duration between the start and the end times

                            System.DateTime theBurstAnalysisEndTime = System.DateTime.Now;

                            System.TimeSpan theBurstAnalysisDuration =
                                theBurstAnalysisEndTime - theBurstAnalysisStartTime;

                            theDebugInformation.WriteTestRunEvent("Burst analysis for the " +
                                thePacketCaptureFileName +
                                " packet capture completed in " +
                                theBurstAnalysisDuration.TotalSeconds.ToString() +
                                " seconds");

                            //// theProgressWindowForm.ProgressBar += 20 / theScaling;
                        }

                        // Only perform the time analysis if the check box was selected for it on the main window form
                        if (this.thePerformTimeAnalysisCheckBox.Checked)
                        {
                            theDebugInformation.WriteBlankLine();

                            theDebugInformation.WriteTextLine(new string('=', 144));

                            theDebugInformation.WriteBlankLine();

                            //// Finalise the time analysis on the messages found including printing the results to debug output
                            //// Only perform this action if the analysis of the packet capture completed successfully

                            // Read the start time to allow later calculation of the duration of the time analysis finalisation
                            System.DateTime theTimeAnalysisStartTime = System.DateTime.Now;

                            theDebugInformation.WriteTestRunEvent("Time analysis for the " +
                                thePacketCaptureFileName +
                                " packet capture started");

                            theProgressWindowForm.ProgressBar += 20 / theScaling;

                            theTimeAnalysisProcessing.Finalise();

                            theProgressWindowForm.ProgressBar += 60 / theScaling;

                            //// Compute the duration between the start and the end times

                            System.DateTime theTimeAnalysisEndTime = System.DateTime.Now;

                            System.TimeSpan theTimeAnalysisDuration =
                                theTimeAnalysisEndTime - theTimeAnalysisStartTime;

                            theDebugInformation.WriteTestRunEvent("Time analysis for the " +
                                thePacketCaptureFileName +
                                " packet capture completed in " +
                                theTimeAnalysisDuration.TotalSeconds.ToString() +
                                " seconds");

                            theProgressWindowForm.ProgressBar += 20 / theScaling;
                        }

                        theDebugInformation.WriteBlankLine();

                        theDebugInformation.WriteTextLine(new string('=', 144));

                        theDebugInformation.WriteBlankLine();

                        // Completed the processing of the packet capture
                        theDebugInformation.WriteTestRunEvent("Processing of the " +
                            thePacketCaptureFileName +
                            " packet capture completed");
                    }
                    else
                    {
                        theDebugInformation.WriteBlankLine();

                        theDebugInformation.WriteTextLine(new string('=', 144));

                        theDebugInformation.WriteBlankLine();

                        // Display a debug message to indicate processing of the packet capture failed
                        theDebugInformation.WriteErrorEvent("Processing of the " +
                            thePacketCaptureFileName +
                            " packet capture failed!!!");
                    }

                    if (theProgressWindowForm != null)
                    {
                        // Hide and close the progress window form now the analysis has completed
                        theProgressWindowForm.Hide();
                        theProgressWindowForm.Close();
                    }

                    //// Dependent on the result of the processing above, display a message box to indicate success or otherwise

                    System.Windows.Forms.DialogResult theMessageBoxResult;

                    if (theResult)
                    {
                        if (this.theEnableDebugInformationCheckBox.Checked)
                        {
                            // Display a message box to indicate analysis of the packet capture completed successfully and ask whether to open the output file
                            theMessageBoxResult =
                                System.Windows.Forms.MessageBox.Show(
                                "Analysis of the " +
                                thePacketCaptureFileName +
                                " packet capture completed successfully!" +
                                System.Environment.NewLine +
                                System.Environment.NewLine +
                                "Do you want to open the output file?",
                                "Run Analysis On Selected Packet Capture",
                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Question);
                        }
                        else
                        {
                            // Display a message box to indicate analysis of the packet capture completed successfully
                            theMessageBoxResult =
                                System.Windows.Forms.MessageBox.Show(
                                "Analysis of the " +
                                thePacketCaptureFileName +
                                " packet capture completed successfully!",
                                "Run Analysis On Selected Packet Capture",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        if (this.theEnableDebugInformationCheckBox.Checked)
                        {
                            // Display a message box to indicate analysis of the packet capture failed and ask whether to open the output file
                            theMessageBoxResult =
                                System.Windows.Forms.MessageBox.Show(
                                "Analysis of the " +
                                thePacketCaptureFileName +
                                " packet capture failed!!!" +
                                System.Environment.NewLine +
                                System.Environment.NewLine +
                                "Do you want to open the output file?",
                                "Run Analysis On Selected Packet Capture",
                                System.Windows.Forms.MessageBoxButtons.YesNo,
                                System.Windows.Forms.MessageBoxIcon.Error);
                        }
                        else
                        {
                            // Display a message box to indicate analysis of the packet capture failed
                            theMessageBoxResult =
                                System.Windows.Forms.MessageBox.Show(
                                "Analysis of the " +
                                thePacketCaptureFileName +
                                " packet capture failed!!!",
                                "Run Analysis On Selected Packet Capture",
                                System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Error);
                        }
                    }

                    // Dependent on the button selection at the message box, open the output file
                    if (theMessageBoxResult == System.Windows.Forms.DialogResult.Yes)
                    {
                        theDebugInformation.Open();
                    }
                }
            }
        }

        //// Button support functions

        /// <summary>
        /// Enables the buttons concerned with packet capture analysis on the main window form
        /// </summary>
        private void EnablePacketCaptureAnalysisButtons()
        {
            //// Enable the buttons

            // 1) Enable the button to clear the selected package capture
            this.theClearSelectedPacketCaptureButton.Enabled = true;

            // 2) Enable the button to open the package capture
            this.theOpenSelectedPackageCaptureButton.Enabled = true;

            // 3) Disable the button to start the analysis on the packet capture - enabled on selection of the output file
            this.theRunAnalysisOnSelectedPackageCaptureButton.Enabled = true;
        }

        /// <summary>
        /// Resets the buttons concerned with packet capture analysis on the main window form
        /// </summary>
        private void ResetPacketCaptureAnalysisButtons()
        {
            //// Reset the buttons

            this.theClearSelectedPacketCaptureButton.Enabled = true;
            this.theOpenSelectedPackageCaptureButton.Enabled = false;
            this.theRunAnalysisOnSelectedPackageCaptureButton.Enabled = false;
        }

        //// Check box support functions

        /// <summary>
        /// Resets the check boxes concerned with packet capture analysis on the main window form
        /// </summary>
        /// <param name="sender">The sender for the initiating event</param>
        private void ResetPacketCaptureAnalysisCheckBoxes(object sender)
        {
            //// Reset the check boxes

            // Guard against infinite recursion
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theEnableDebugInformationCheckBox.Checked = false;
            }

            // Match the state of the "Enable Information Events" check box to the state of the "Enable" check box
            this.theEnableInformationEventsInDebugInformationCheckBox.Checked =
                this.theEnableDebugInformationCheckBox.Checked;
            this.theEnableInformationEventsInDebugInformationCheckBox.Enabled = false;

            this.theRedirectDebugInformationToOutputCheckBox.Checked = false;
            this.theRedirectDebugInformationToOutputCheckBox.Enabled = false;

            this.theLatencyAnalysisGroupBox.Enabled = false;

            this.thePerformLatencyAnalysisCheckBox.Checked = false;
            this.thePerformLatencyAnalysisCheckBox.Enabled = false;

            this.theOutputLatencyAnalysisHistogramCheckBox.Checked = false;
            this.theOutputLatencyAnalysisHistogramCheckBox.Enabled = false;

            this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Checked = false;
            this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Enabled = false;

            this.theBurstAnalysisGroupBox.Enabled = false;

            this.thePerformBurstAnalysisCheckBox.Checked = false;
            this.thePerformBurstAnalysisCheckBox.Enabled = false;

            this.theOutputBurstAnalysisHistogramCheckBox.Checked = false;
            this.theOutputBurstAnalysisHistogramCheckBox.Enabled = false;

            this.theOutputAdditionalBurstAnalysisInformationCheckBox.Checked = false;
            this.theOutputAdditionalBurstAnalysisInformationCheckBox.Enabled = false;

            this.theTimeAnalysisGroupBox.Enabled = false;

            this.thePerformTimeAnalysisCheckBox.Checked = false;
            this.thePerformTimeAnalysisCheckBox.Enabled = false;

            this.theOutputTimeAnalysisHistogramCheckBox.Checked = false;
            this.theOutputTimeAnalysisHistogramCheckBox.Enabled = false;

            this.theOutputAdditionalTimeAnalysisInformationCheckBox.Checked = false;
            this.theOutputAdditionalTimeAnalysisInformationCheckBox.Enabled = false;

            // Do not change the "Minimize Memory Usage" check box on a change to the "Enable" check box in the "Debug Information" group box
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theUseAlternativeSequenceNumberCheckBox.Checked = false;
                this.theUseAlternativeSequenceNumberCheckBox.Enabled = false;

                this.theMinimizeMemoryUsageCheckBox.Checked = false;
                this.theMinimizeMemoryUsageCheckBox.Enabled = false;
            }
        }

        /// <summary>
        /// Enables the check boxes concerned with packet capture analysis on the main window form
        /// </summary>
        /// <param name="sender">The sender for the initiating event</param>
        private void EnablePacketCaptureAnalysisCheckBoxes(object sender)
        {
            //// Enable the check boxes

            // Guard against infinite recursion
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theEnableDebugInformationCheckBox.Enabled = true;

                this.theDebugInformationGroupBox.Enabled = true;
            }

            // Only enable the debug information and analysis check boxes on a change to the "Enable" check box in the "Debug Information" group box
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theEnableInformationEventsInDebugInformationCheckBox.Enabled = false;
                this.theRedirectDebugInformationToOutputCheckBox.Enabled = false;

                this.theLatencyAnalysisGroupBox.Enabled = false;
                this.thePerformLatencyAnalysisCheckBox.Enabled = false;

                this.theBurstAnalysisGroupBox.Enabled = false;
                this.thePerformBurstAnalysisCheckBox.Enabled = false;

                this.theTimeAnalysisGroupBox.Enabled = false;
                this.thePerformTimeAnalysisCheckBox.Enabled = false;
            }
            else
            {
                this.theEnableInformationEventsInDebugInformationCheckBox.Enabled = true;
                this.theRedirectDebugInformationToOutputCheckBox.Enabled = true;

                this.theLatencyAnalysisGroupBox.Enabled = true;
                this.thePerformLatencyAnalysisCheckBox.Enabled = true;

                this.theBurstAnalysisGroupBox.Enabled = true;
                this.thePerformBurstAnalysisCheckBox.Enabled = true;

                this.theTimeAnalysisGroupBox.Enabled = true;
                this.thePerformTimeAnalysisCheckBox.Enabled = true;
            }

            // Do not change the "Minimize Memory Usage" check box on a change to the "Enable" check box in the "Debug Information" group box
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theUseAlternativeSequenceNumberCheckBox.Enabled = true;

                this.theMinimizeMemoryUsageCheckBox.Enabled = true;
            }
        }

        /// <summary>
        /// Disables the check boxes concerned with packet capture analysis on the main window form
        /// </summary>
        /// <param name="sender">The sender for the initiating event</param>
        private void DisablePacketCaptureAnalysisCheckBoxes(object sender)
        {
            //// Disable the check boxes

            // Guard against infinite recursion
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theEnableDebugInformationCheckBox.Enabled = false;

                this.theDebugInformationGroupBox.Enabled = false;
            }

            this.theEnableInformationEventsInDebugInformationCheckBox.Enabled = false;
            this.theRedirectDebugInformationToOutputCheckBox.Enabled = false;

            this.theLatencyAnalysisGroupBox.Enabled = false;
            this.thePerformLatencyAnalysisCheckBox.Enabled = false;
            this.theOutputLatencyAnalysisHistogramCheckBox.Enabled = false;
            this.theOutputAdditionalLatencyAnalysisInformationCheckBox.Enabled = false;

            this.theBurstAnalysisGroupBox.Enabled = false;
            this.thePerformBurstAnalysisCheckBox.Enabled = false;
            this.theOutputBurstAnalysisHistogramCheckBox.Enabled = false;
            this.theOutputAdditionalBurstAnalysisInformationCheckBox.Enabled = false;

            this.theTimeAnalysisGroupBox.Enabled = false;
            this.thePerformTimeAnalysisCheckBox.Enabled = false;
            this.theOutputTimeAnalysisHistogramCheckBox.Enabled = false;
            this.theOutputAdditionalTimeAnalysisInformationCheckBox.Enabled = false;

            // Do not change the "Minimize Memory Usage" check box on a change to the "Enable" check box in the "Debug Information" group box
            if (sender != this.theEnableDebugInformationCheckBox)
            {
                this.theUseAlternativeSequenceNumberCheckBox.Enabled = false;

                this.theMinimizeMemoryUsageCheckBox.Enabled = false;
            }
        }
    }
}

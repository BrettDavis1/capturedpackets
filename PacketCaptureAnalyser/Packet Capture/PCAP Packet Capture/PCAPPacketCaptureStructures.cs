﻿//This is free and unencumbered software released into the public domain.

//Anyone is free to copy, modify, publish, use, compile, sell, or
//distribute this software, either in source code form or as a compiled
//binary, for any purpose, commercial or non-commercial, and by any
//means.

//In jurisdictions that recognize copyright laws, the author or authors
//of this software dedicate any and all copyright interest in the
//software to the public domain. We make this dedication for the benefit
//of the public at large and to the detriment of our heirs and
//successors. We intend this dedication to be an overt act of
//relinquishment in perpetuity of all present and future rights to this
//software under copyright law.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

//For more information, please refer to <http://unlicense.org/>

namespace PacketCaptureProcessingNamespace
{
    class PCAPPackageCaptureStructures
    {
        //
        //PCAP packet capture global header - 24 bytes
        //

        [System.Runtime.InteropServices.StructLayout
            (System.Runtime.InteropServices.LayoutKind.Explicit,
            Size = PCAPPackageCaptureConstants.PCAPPackageCaptureGlobalHeaderLength)]

        public struct PCAPPackageCaptureGlobalHeaderStructure
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public System.UInt32 MagicNumber; //Magic number

            [System.Runtime.InteropServices.FieldOffset(4)]
            public System.UInt16 VersionMajor; //Major version number

            [System.Runtime.InteropServices.FieldOffset(6)]
            public System.UInt16 VersionMinor; //Minor version number

            [System.Runtime.InteropServices.FieldOffset(8)]
            public System.Int32 ThisTimeZone; //GMT to local correction

            [System.Runtime.InteropServices.FieldOffset(12)]
            public System.UInt32 SignificantFigures; //Accuracy of timestamps

            [System.Runtime.InteropServices.FieldOffset(16)]
            public System.UInt32 SnapshotLength; //Max length of captured packets, in octets

            [System.Runtime.InteropServices.FieldOffset(20)]
            public System.UInt32 NetworkDataLinkType; //Network data link type
        }

        //
        //PCAP packet capture packet header - 16 bytes
        //

        [System.Runtime.InteropServices.StructLayout
            (System.Runtime.InteropServices.LayoutKind.Explicit,
            Size = PCAPPackageCaptureConstants.PCAPPackageCapturePacketHeaderLength)]

        public struct PCAPPackageCapturePacketHeaderStructure
        {
            [System.Runtime.InteropServices.FieldOffset(0)]
            public System.UInt32 TimestampSeconds; //Timestamp seconds

            [System.Runtime.InteropServices.FieldOffset(4)]
            public System.UInt32 TimestampMicroseconds; //Timestamp microseconds

            [System.Runtime.InteropServices.FieldOffset(8)]
            public System.UInt32 SavedLength; //Number of octets of packet saved in packet capture

            [System.Runtime.InteropServices.FieldOffset(12)]
            public System.UInt32 ActualLength; //Actual length of packet
        }
    }
}
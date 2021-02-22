using System;
using System.Collections;
using System.IO;

using TagLib.Utils;


namespace TagLib.ID3.Lib
{
    class MpegInfo
    {
        public int LocationIndex;
        public int HeaderSize;
        public int FileSize;

        public string VersionEncoded;
        public string LayerEncoded;
        public string BitrateEncoded;
        public string SamplingRateEncoded;
        public string ChannelModeEncoded;
        public string ModeExtensionEncoded;
        public string EmphasisEncoded;

        public int Bitrate;
        public string Duration;
        public string SamplingRate;
        public int FramSize = 1152;
        public int FrameLength;
        public string Layer;
        public string Version;
        public int Kbps;
        public bool ProtectionBit;
        public bool PaddingBit;
        public bool PrivateBit;
        public string ChannelMode;
        public bool Copyright;
        public bool Original;
        public string Emphasis;
        public string RawResults;

        public MpegInfo(FileInfo fileInfo, int startIndex) {
            FileStream mReader = fileInfo.OpenRead();
            byte[] mFramesync = new byte[11];
            byte[] mMpegAudioVersionID = new byte[2];
            byte[] mLayerDescription = new byte[2];
            byte[] mProtectionBit = new byte[1];
            byte[] mBitrateIndex = new byte[4];
            byte[] mSamplingRateFrequencyIndex = new byte[2];
            byte[] mPaddingBit = new byte[1];
            byte[] mPrivateBit = new byte[1];
            byte[] mChannelMode = new byte[2];
            byte[] mModeExtension = new byte[2];
            byte[] mCopyright = new byte[1];
            byte[] mOriginal = new byte[1];
            byte[] mEmphasis = new byte[2];

            this.FileSize = Convert.ToInt32(fileInfo.Length);

            mReader.Position = 0;

            int mStartIndex = startIndex;
            int mEndIndex = startIndex + 20000;
            int mBufferLength = 10000;
            int mBufferOffset = 0;
            byte mSyncByte = 255;
            ArrayList mTryHeaders = new ArrayList();

            mReader.Position = mStartIndex;
            while (!(mReader.Position >= mEndIndex))
            {
                byte[] mBytes = new byte[mBufferLength];
                mReader.Read(mBytes, 0, mBufferLength);

                Console.WriteLine("Reading next chunk of " + mBufferLength + " bytes...");
                int mTryIndex = 0;
                int mTryIndexOffset = 0;
                while(!(mTryIndexOffset >= mBufferLength))
                {
                    mTryIndex = Array.IndexOf(mBytes, mSyncByte, mTryIndexOffset + 1);
                    if ( mTryIndex == -1 )
                    {
                        mTryIndexOffset = mBufferLength;
                    }
                    else
                    {
                        mTryIndexOffset = mTryIndex;
                        try
                        {
                            if (mBytes[mTryIndexOffset + 1] >= 224)
                            {
                                byte[] mTryHeader = new byte[4];
                                Buffer.BlockCopy(mBytes, mTryIndexOffset, mTryHeader, 0, 4);
                                if (IsValidMpegHeaderBytes(mTryHeader) == "VALID")
                                {
                                    mTryHeaders.Add(mTryHeader);
                                    ParseMpegHeader(mTryHeader);
                                    int mFrameLength = Convert.ToInt32(Math.Ceiling((decimal)((144 * (this.Bitrate / Convert.ToInt32(this.SamplingRate))) + Convert.ToInt32(this.PaddingBit))));
                                    var mResults = "Detected MPEG header freame sync byte at byte index " + (mStartIndex + mTryIndexOffset + mBufferOffset) + " or arry index " + mTryIndexOffset + " with a binary value of " + Mp3Utils.BytesToBinaryString(mTryHeader) + " or decoded/interpreted values of Bitrate: " + this.Bitrate + " Sampling rate: " + this.SamplingRate + mFrameLength + " Frame duration in milliseconds: " + (1152 / Convert.ToInt32(this.SamplingRate)) * 1000;
                                    this.RawResults += Environment.NewLine + mResults;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                mBufferOffset += mBufferLength;
            }
            mReader.Close();
            mReader.Dispose();
        }

        string IsValidMpegHeaderBytes(Byte[] b)
        {
            string result = "VALID";
            string s = Mp3Utils.BytesToBinaryString(b);

            if (s.Substring(11, 2) == "01") result = "INVALID A";
            if (s.Substring(13, 2) == "00") result = "INVALID B";
            if (s.Substring(16, 4) == "1111") result = "INVALID C";
            if (s.Substring(20, 2) == "11") result = "INVALID D";
            if (s.Substring(30, 2) == "10") result = "INVALID E";
            return result;
        }

        void ParseMpegHeader(byte[] argBytes)
        {
            string s = Mp3Utils.BytesToBinaryString(argBytes);

            this.BitrateEncoded = s.Substring(16, 4);
            this.SamplingRateEncoded = s.Substring(20, 2);
            PaddingBit = Convert.ToBoolean(s.Substring(22, 1));
            PrivateBit = Convert.ToBoolean(s.Substring(23, 1));
            this.ChannelMode = s.Substring(24, 2);
            this.ModeExtensionEncoded = s.Substring(26, 2);
            Copyright = Convert.ToBoolean(s.Substring(28, 1));
            Original = Convert.ToBoolean(s.Substring(29, 1));
            this.EmphasisEncoded = s.Substring(30, 1);

            switch(this.VersionEncoded)
            {
                case "00":
                    this.Version = "2.5";
                    break;
                case "01":
                    this.Version = "0";
                    break;
                case "10":
                    this.Version = "2";
                    break;
                case "11":
                    this.Version = "1";
                    break;
            }

            switch(this.LayerEncoded)
            {
                case "00":
                    this.Layer = "0";
                    break;
                case "01":
                    this.Layer = "3";
                    break;
                case "10":
                    this.Layer = "2";
                    break;
                case "11":
                    this.Layer = "1";
                    break;
            }

            switch(this.BitrateEncoded)
            {
                case "0000":
                    this.Bitrate = 0;
                    break;
                case "0001":
                    this.Bitrate = 32000;
                    break;
                case "0010":
                    this.Bitrate = 40000;
                    break;
                case "0011":
                    this.Bitrate = 48000;
                    break;
                case "0100":
                    this.Bitrate = 56000;
                    break;
                case "0101":
                    this.Bitrate = 64000;
                    break;
                case "0110":
                    this.Bitrate = 80000;
                    break;
                case "0111":
                    this.Bitrate = 96000;
                    break;
                case "1000":
                    this.Bitrate = 112000;
                    break;
                case "1001":
                    this.Bitrate = 128000;
                    break;
                case "1010":
                    this.Bitrate = 160000;
                    break;
                case "1011":
                    this.Bitrate = 192000;
                    break;
                case "1100":
                    this.Bitrate = 224000;
                    break;
                case "1101":
                    this.Bitrate = 256000;
                    break;
                case "1110":
                    this.Bitrate = 320000;
                    break;
                case "1111":
                    this.Bitrate = 0;
                    break;
            }
            this.Kbps = this.Bitrate / 1000;

            switch (this.SamplingRateEncoded)
            {
                case "00":
                    this.SamplingRate = "44100";
                    break;
                case "01":
                    this.SamplingRate = "48000";
                    break;
                case "10":
                    this.SamplingRate = "32000";
                    break;                
                case "11":
                    this.SamplingRate = "0";
                    break;
            }

            switch (this.ChannelModeEncoded)
            {
                case "00":
                    this.ChannelMode = "Stereo";
                    break;
                case "01":
                    this.ChannelMode = "Joint stereo (Stereo)";
                    break;
                case "10":
                    this.ChannelMode = "Dual channel (stereo)";
                    break;
                case "11":
                    this.ChannelMode = "Single channel (Mono)";
                    break;
            }

            switch (this.EmphasisEncoded)
            {
                case "00":
                    this.Emphasis = "none";
                    break;
                case "01":
                    this.Emphasis = "50/15ms";
                    break;
                case "10":
                    this.Emphasis = "0";
                    break;
                case "11":
                    this.Emphasis = "CCIT J.17";
                    break;
            }

            this.Duration = (((this.FileSize / 1024) * 8) / this.Kbps).ToString();
            this.Duration = ((this.FileSize / this.Kbps) / 60).ToString();
            this.Duration = (this.FileSize / (this.Bitrate * 8)).ToString();
        }
    }
}

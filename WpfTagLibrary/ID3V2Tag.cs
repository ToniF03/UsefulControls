using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Windows.Controls;
using TagLib.Utils;

namespace TagLib.ID3.ID3v2
{
    class ID3v2Tag
    {
        public class FrameBase
        {
            public string ID;
            public int Size;
            public string Flags;
            public byte[] ValueBytes;
            public string Value()
            {
                return Mp3Utils.BytesToText(ValueBytes);
            }
        }

        public System.IO.FileStream gTagReader;
        public string FileIdentifier;
        public string TagVersion;
        public string TagSize;
        public string HeaderFlags;
        public ArrayList Frames = new ArrayList();

        string GetFrameValue(string FrameName)
        {
            for (int i = 0; i <= this.Frames.Count; i++)
            {
                FrameBase mFrame;
                if (Frames.Count > i)
                    mFrame = (FrameBase)Frames[i];
                else break;
                if (mFrame.ID == FrameName) return mFrame.Value();
            }
            return null;
        }

        Bitmap GetFrameImage(string FrameName)
        {
            for (int i = 0; i <= this.Frames.Count; i++)
            {
                FrameBase mFrame;
                if (Frames.Count > i)
                    mFrame = (FrameBase)Frames[i];
                else break;
                if (mFrame.ID == FrameName) return GetCover(mFrame);
            }
            return null;
        }

        public ID3v2Tag(System.IO.FileInfo fileInfo)
        {
            ParseTag(fileInfo);
        }

        private System.Windows.Controls.Image ConvertImageToWpfImage(System.Drawing.Image image)
        {
            if (image == null)
                throw new ArgumentNullException("image", "Image darf nicht null sein.");

            using (System.Drawing.Bitmap dImg = new System.Drawing.Bitmap(image))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    dImg.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                    System.Windows.Media.Imaging.BitmapImage bImg = new System.Windows.Media.Imaging.BitmapImage();

                    bImg.BeginInit();
                    bImg.StreamSource = new MemoryStream(ms.ToArray());
                    bImg.EndInit();

                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Source = bImg;

                    return img;
                }
            }
        }

        public System.Windows.Controls.Image Cover
        {
            get
            {
                return ConvertImageToWpfImage(GetFrameImage("APIC"));
            }
        }

        private Bitmap GetCover(FrameBase argFrame)
        {
            Bitmap img = null;
            byte mApicTextEncoding;
            ArrayList mApicMimeType = new ArrayList();
            byte mApicPictureType;
            string mApicDescription = "";
            int mApicDataStart = 14;

            string c = "";
            int i = 0;

            mApicTextEncoding = argFrame.ValueBytes[i];
            i++;

            c = "";
            while (c != "0")
            {
                c = argFrame.ValueBytes[i].ToString();
                mApicMimeType.Add(c);
                i++;
            }

            mApicPictureType = argFrame.ValueBytes[i];
            i++;

            c = "";
            while (c != "0")
            {
                c = argFrame.ValueBytes[i].ToString();
                mApicDescription += c.ToString();
                i++;
            }

            //string temp = "c:\\Temp\\Testbild.jpg";
            //int length = argFrame.ValueBytes.Length - mApicDataStart;
            //byte[] buffer = new byte[length];
            //Array.Copy(argFrame.ValueBytes, mApicDataStart, buffer, 0, length);
            //File.WriteAllBytes(temp, buffer);

            int length = argFrame.ValueBytes.Length - mApicDataStart;
            MemoryStream ms = new MemoryStream(argFrame.ValueBytes, mApicDataStart, length);
            img = (Bitmap)System.Drawing.Image.FromStream(ms);

            return img;
        }

        public string Title()
        {
            return GetFrameValue("TIT2");
        }

        public string Artist()
        {
            string val = this.GetFrameValue("TPE1");
            if (val == null)
                val = this.GetFrameValue("TP1");
            return val;
        }

        public string Year()
        {
            string val = this.GetFrameValue("TYER");
            if (val == null)
                val = this.GetFrameValue("TDRC");
            if (val == null)
                val = this.GetFrameValue("TRD");
            return val;
        }

        public string AlbumArtist()
        {
            string val = this.GetFrameValue("TPE2");
            if (val == null)
                val = this.GetFrameValue("TP2");
            return val;
        }

        public string Album()
        {
            string val = this.GetFrameValue("TALB");
            if (val == null) val = this.GetFrameValue("TAL");
            return val;
        }

        public string TrackNumber()
        {
            string val = this.GetFrameValue("TRCK");
            if (val == null) val = this.GetFrameValue("TRK");
            return val;
        }

        public string Comment()
        {
            string val = this.GetFrameValue("COMM");
            if (val == null) val = this.GetFrameValue("COM");
            if (val != null) return val.Substring(3, val.Length - 3);
            return null;
        }

        public string Genre()
        {
            string val = this.GetFrameValue("TCON");
            if (val == null) val = this.GetFrameValue("TCO");
            return val;
        }

        public string Composer()
        {
            string val = this.GetFrameValue("TCOM");
            return val;
        }

        public string Disc()
        {
            string val = this.GetFrameValue("TPOS");
            return val;
        }

        /// <summary>
        /// Writes Tags to Mp3 File
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="ID">The ID of the Tag</param>
        /// <param name="Value">The Value of the Tag</param>
        void WriteTag(FileInfo fileInfo, string ID, string Value)
        {
            gTagReader = fileInfo.OpenRead();
            switch (this.TagVersion)
            {
                case "2.0":
                    //WriteHeaderV2();
                    break;
                case "3.0":
                    break;
                case "4.0":
                    break;
                case "Unknown Descriptor":
                    break;
            }
        }

        void ParseTag(System.IO.FileInfo fileInfo)
        {
            gTagReader = fileInfo.OpenRead();
            ParseTagVersion();
            switch (this.TagVersion)
            {
                case "2.0":
                    ParseHeaderV2();
                    ParseFramesV2();
                    break;
                case "3.0":
                    ParseHeaderV4();
                    ParseFramesV3();
                    break;
                case "4.0":
                    ParseHeaderV4();
                    ParseFramesV4();
                    break;
                case "Unknown Descriptor":
                    break;
            }
            gTagReader.Close();
            gTagReader.Dispose();
        }

        void ParseTagVersion()
        {
            byte[] mTagDescriptor = new byte[3];
            byte[] mTagVersionBytes = new byte[2];
            this.gTagReader.Read(mTagDescriptor, 0, 3);
            this.gTagReader.Read(mTagVersionBytes, 0, 2);
            if (Mp3Utils.BytesToText(mTagDescriptor) == "ID3")
            {
                this.FileIdentifier = "ID3";
                this.TagVersion = (mTagVersionBytes[0]) + "." + (mTagVersionBytes[1]);
            }
            else
                this.TagVersion = "";
        }

        void ParseFramesV2()
        {
            gTagReader.Position = 10;
            while (!(gTagReader.Position >= Convert.ToInt32(this.TagSize)))
            {
                FrameBase mFrame = new FrameBase();
                byte[] mFrameIdBytes = new byte[3];
                byte[] mFrameSizeBytes = new byte[3];

                gTagReader.Read(mFrameIdBytes, 0, 3);
                gTagReader.Read(mFrameSizeBytes, 0, 3);

                if (mFrameIdBytes[0] == 0) { break; }

                string mFrameID = Mp3Utils.BytesToText(mFrameIdBytes);
                mFrame.ID = mFrameID;

                int mSizeV2 = Mp3Utils.GetID3EncodedSizeV2(mFrameSizeBytes);
                mFrame.Size = mSizeV2;
                Array.Resize(ref mFrame.ValueBytes, mFrame.Size);
                gTagReader.Read(mFrame.ValueBytes, 0, mFrame.Size);
                string mVal = mFrame.Value();
                this.Frames.Add(mFrame);
            }
        }

        void ParseFramesV3()
        {
            gTagReader.Position = 10;
            while (!(gTagReader.Position >= Convert.ToInt32(this.TagSize)))
            {
                FrameBase mFrame = new FrameBase();
                byte[] mFrameIdBytes = new byte[4];
                byte[] mFrameSizeBytes = new byte[4];
                byte[] mFrameFlagsBytes = new byte[4];

                gTagReader.Read(mFrameIdBytes, 0, 4);
                gTagReader.Read(mFrameSizeBytes, 0, 4);
                gTagReader.Read(mFrameFlagsBytes, 0, 2);

                if (mFrameIdBytes[0] == 0) { break; }

                string mFrameID = Mp3Utils.BytesToText(mFrameIdBytes);
                mFrame.ID = mFrameID;

                int mSizeV3 = Mp3Utils.GetID3EncodedSizeV2(mFrameSizeBytes);
                mFrame.Size = mSizeV3;
                string flags = GetBitString(mFrameFlagsBytes);

                if (BitManipulator.ExamineBit(mFrameFlagsBytes[1], 8) == true) mFrame.Flags += "Tag Alter Preservation, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[1], 7) == true) mFrame.Flags += "File Alter Preservation, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[1], 6) == true) mFrame.Flags += "Read Only, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 8) == true) mFrame.Flags += "Compression, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 7) == true) mFrame.Flags += "Encryption, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 6) == true) mFrame.Flags += "Group Identity";

                Array.Resize(ref mFrame.ValueBytes, mFrame.Size);
                gTagReader.Read(mFrame.ValueBytes, 0, mFrame.Size);
                string mVal = mFrame.Value();
                this.Frames.Add(mFrame);
            }
        }

        void ParseFramesV4()
        {
            gTagReader.Position = 10;
            while (!(gTagReader.Position >= Convert.ToInt32(this.TagSize)))
            {
                FrameBase mFrame = new FrameBase();
                byte[] mFrameIdBytes = new byte[4];
                byte[] mFrameSizeBytes = new byte[4];
                byte[] mFrameFlagsBytes = new byte[2];

                gTagReader.Read(mFrameIdBytes, 0, 4);
                gTagReader.Read(mFrameSizeBytes, 0, 4);
                gTagReader.Read(mFrameFlagsBytes, 0, 2);

                if (mFrameIdBytes[0] == 0) { break; }

                string mFrameID = Mp3Utils.BytesToText(mFrameIdBytes);
                mFrame.ID = mFrameID;

                int mSizeV2 = Mp3Utils.GetID3EncodedSizeV2(mFrameSizeBytes);
                mFrame.Size = mSizeV2;
                int mSizeV3 = Mp3Utils.GetID3EncodedSizeV2(mFrameSizeBytes);
                mFrame.Size = mSizeV3;
                int mSizeV4 = Mp3Utils.GetID3EncodedSizeV2(mFrameSizeBytes);
                mFrame.Size = mSizeV4;

                string flags = GetBitString(mFrameFlagsBytes);

                if (BitManipulator.ExamineBit(mFrameFlagsBytes[1], 7) == true) mFrame.Flags += "Tag Alter Preservation, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[1], 6) == true) mFrame.Flags += "File Alter Preservation, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[1], 5) == true) mFrame.Flags += "Read Only, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 7) == true) mFrame.Flags += "Group Identity, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 4) == true) mFrame.Flags += "Compression, ";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 3) == true) mFrame.Flags += "Encryption";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 2) == true) mFrame.Flags += "Unsynchronization";
                if (BitManipulator.ExamineBit(mFrameFlagsBytes[0], 1) == true) mFrame.Flags += "Data Length Indicator";

                Array.Resize(ref mFrame.ValueBytes, mFrame.Size);
                gTagReader.Read(mFrame.ValueBytes, 0, mFrame.Size);
                string mVal = mFrame.Value();
                this.Frames.Add(mFrame);
            }
        }

        public static string GetBitString(byte[] argBytes)
        {
            string s = "";
            for (int i = 0; i <= argBytes.Length - 1; i++)
            {
                s += BitManipulator.ExamineBit(argBytes[i], 0);
                s += BitManipulator.ExamineBit(argBytes[i], 1);
                s += BitManipulator.ExamineBit(argBytes[i], 2);
                s += BitManipulator.ExamineBit(argBytes[i], 3);
                s += BitManipulator.ExamineBit(argBytes[i], 4);
                s += BitManipulator.ExamineBit(argBytes[i], 5);
                s += BitManipulator.ExamineBit(argBytes[i], 6);
                s += BitManipulator.ExamineBit(argBytes[i], 7);
                s += BitManipulator.ExamineBit(argBytes[i], 8);
            }
            return s;
        }

        void ParseHeaderV2()
        {
            byte[] mHeaderFlags = new byte[2];
            byte[] mTagSize = new byte[4];

            gTagReader.Read(mHeaderFlags, 0, 1);

            if (BitManipulator.ExamineBit(mHeaderFlags[0], 8) == true) this.HeaderFlags = "Unsynchronization";
            if (BitManipulator.ExamineBit(mHeaderFlags[0], 7) == true) this.HeaderFlags += ", " + "Extended Header";
            if (BitManipulator.ExamineBit(mHeaderFlags[0], 6) == true) this.HeaderFlags += ", " + "Experimental Indicator";

            gTagReader.Read(mTagSize, 0, 4);
            this.TagSize = Mp3Utils.GetID3EncodedSizeV4(mTagSize).ToString();
        }

        void ParseHeaderV4()
        {
            byte[] mHeaderFlags = new byte[2];
            byte[] mTagSize = new byte[4];

            gTagReader.Read(mHeaderFlags, 0, 1);

            if (BitManipulator.ExamineBit(mHeaderFlags[0], 8) == true) this.HeaderFlags = "Unsynchronization";
            if (BitManipulator.ExamineBit(mHeaderFlags[0], 7) == true) this.HeaderFlags += ", " + "Extended Header";
            if (BitManipulator.ExamineBit(mHeaderFlags[0], 6) == true) this.HeaderFlags += ", " + "Experimental Indicator";
            if (BitManipulator.ExamineBit(mHeaderFlags[0], 5) == true) this.HeaderFlags += ", " + "Footer";

            gTagReader.Read(mTagSize, 0, 4);
            this.TagSize = Mp3Utils.GetID3EncodedSizeV4(mTagSize).ToString();
        }
    }
}

